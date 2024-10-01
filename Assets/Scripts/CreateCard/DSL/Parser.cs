using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Xml;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class Parser
{
    public List<Token> tokens;
    int current = 0;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }
    public List<Class> Parse()
    {
        List<Class> classes = new List<Class>();
        while (!IsAtEnd())
        {
            Class _class = Class();
            if (_class == null) break;
            classes.Add(_class);
        }
        return classes;
    }
    private Class Class()
    {
        if (Check(TokenType.Card))
        {
            return CardClass();
        }
        DSL.error(Peek(), "Only 'card' and 'effect' are accepted as class names.");
        return null;
    }
    private Class CardClass()
    {
        if (!Consume(TokenType.Card, "Expected 'card' declaration.")) return null;
        if (!Consume(TokenType.LeftBrace, "Expected '{' after 'card'.")) return null;
        Prop type = Type();
        if (!Consume(TokenType.Comma, "Expected ',' after 'Type' declaration.")) return null;
        Prop name = Name();
        if (!Consume(TokenType.Comma, "Expected ',' after 'Name' declaration.")) return null;
        Prop faction = Faction();
        if (!Consume(TokenType.Comma, "Expected ',' after 'Faction' declaration.")) return null;
        Prop power = Power();
        if (!Consume(TokenType.Comma, "Expected ',' after 'Power' declaration.")) return null;
        Prop range = Range();
        if (!Consume(TokenType.Comma, "Expected ',' after 'Range' declaration.")) return null;
        Method onActivation = OnActivation();
        if (!Consume(TokenType.RightBrace, "Expected '}' at the end of 'card'.")) return null;
        return new CardClass(type, name, faction, power, range, onActivation);
    }

    private Method OnActivation()
    {
        if (!Consume(TokenType.OnActivation, "Expected 'OnActivation' declaration.")) return null;
        if (!Consume(TokenType.Colon, "Expected ':' after 'OnActivation'.")) return null;
        if (!Consume(TokenType.LeftBracket, "Expected '['.")) return null;
        if (Match(TokenType.RightBracket)) return null;
        List<Method> onActBodies = new List<Method>();
        
        do
        {
            onActBodies.Add(OnActBody());
        } while (Match(TokenType.Comma));

        if (!Consume(TokenType.RightBracket, "Expected ']'.")) return null;
        return null;
    }    

    private Method OnActBody()
    {
        if (!Consume(TokenType.LeftBrace, "Expected '{'.")) return null;
        Method effect = Effect();
        if (!Match(TokenType.Comma))
        {
            if (!Consume(TokenType.RightBrace, "Expected '}'.")) return null;
            return new OnActBody(effect, null, null);
        }
        Method postAction = null;
        if (Match(TokenType.PostAction))
        {
            if (!Consume(TokenType.Colon, "Expected ':' after 'PostAction'.")) return null;
            postAction = OnActBody();
            if (!Consume(TokenType.RightBrace, "Expected '}'.")) return null;
            return new OnActBody(effect, null, postAction);
        }
        Method selector = Selector();
        if (!Check(TokenType.Comma))
        {
            if (!Consume(TokenType.RightBrace, "Expected '}'.")) return null;
            return new OnActBody(effect, selector, null);
        }
        if (!Consume(TokenType.Comma, "Expected ',' after 'Selector'.")) return null;
        if (!Consume(TokenType.PostAction, "Expected 'PostAction'.")) return null;
        if (!Consume(TokenType.Colon, "Expected ':' after 'PostAction'.")) return null;
        postAction = OnActBody();
        if (!Consume(TokenType.RightBrace, "Expected '}'.")) return null;
        return new OnActBody(effect, selector, postAction);
    }

    private Method Effect()
    {
        if (!Consume(TokenType.Effect, "Expected 'Effect' declaration.")) return null;
        Token effect = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Effect'.")) return null;
        if (!Match(TokenType.LeftBrace))
        {
            IExpression effectName = expression();
            return new Effect(effect, effectName);
        }
        Prop name = Name();
        List<Prop> paramsvalues = new List<Prop>();
        while (Match(TokenType.Comma))
        {
            paramsvalues.Add(ParamValue());
        }
        if (!Consume(TokenType.RightBrace, "Expected '}'.")) return null;
        return new Effect(effect, name, paramsvalues);
    }
    private Prop ParamValue()
    {
        if (!Consume(TokenType.Identifier, "Expected parameter declaration.")) return null;
        Token name = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after parameter name.")) return null;
        IExpression value = expression();
        return new ParamValue(name, value);
    }
    private Method Selector()
    {
        if (!Consume(TokenType.Selector, "Expected 'Selector' declaration.")) return null;
        Token selector = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Selector'.")) return null;
        if (!Consume(TokenType.LeftBrace, "Expected '{'.")) return null;
        Prop source = Source();
        if (!Consume(TokenType.Comma, "Expected ',' after 'Source'.")) return null;
        Prop single = Single();
        if (!Consume(TokenType.Comma, "Expected ',' after 'Single'.")) return null;
        Prop predicate = Predicate();
        if (!Consume(TokenType.RightBrace, "Expected '}'.")) return null;
        return new Selector(selector, source, single, predicate);
    }

    private Prop Source()
    {
        if (!Consume(TokenType.Source, "Expected 'Source' declaration.")) return null;
        Token source = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Source'.")) return null;
        IExpression value = expression();
        return new Source(source, value);
    }

    private Prop Single()
    {
        if (!Consume(TokenType.Single, "Expected 'Single' declaration.")) return null;
        Token single = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Single'.")) return null;
        IExpression value = expression();
        return new Single(single, value);
    }

    private Prop Predicate()
    {
        if (!Consume(TokenType.Predicate, "Expected 'Predicate' declaration.")) return null;
        Token predicate = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Predicate'.")) return null;
        if (!Consume(TokenType.LeftParen, "Expected '('.")) return null;
        if (!Consume(TokenType.Identifier, "Expected identifier.")) return null;
        Token card = previous();
        if (!Consume(TokenType.RightParen, "Expected ')'.")) return null;
        if (!Consume(TokenType.Arrow, "Expected \"=>\".")) return null;
        IExpression condition = expression();
        return new Predicate(predicate, card, condition);
    }

    private Prop Name()
    {
        if (!Consume(TokenType.Name, "Expected 'Name' declaration.")) return null;
        Token name = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Name'.")) return null;
        IExpression value =expression();
        return new Name(name, value);
    }

    private Prop Type()
    {
        if (!Consume(TokenType.Type, "Expected 'Type' declaration.")) return null;
        Token type = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Type'.")) return null;
        IExpression value = expression();
        return new Type(type, value);
    }

    private Prop Faction()
    {
        if (!Consume(TokenType.Faction, "Expected 'Faction' declaration.")) return null;
        Token faction = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Faction'.")) return null;
        IExpression value = expression();
        return new Faction(faction, value);
    }

    private Prop Power()
    {
        if (!Consume(TokenType.Power, "Expected 'Power' declaration.")) return null;
        Token power = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Power'.")) return null;
        IExpression value = expression();
        return new Power(power, value);
    }

    private Prop Range()
    {
        if (!Consume(TokenType.Range, "Expected 'Range' declaration.")) return null;
        Token range = previous();
        if (!Consume(TokenType.Colon, "Expected ':' after 'Range'.")) return null;
        if (!Consume(TokenType.LeftBracket, "Expected '['.")) return null;
        List<IExpression> list = new List<IExpression>();
        do
        {
            list.Add(expression());
        } while (Match(TokenType.Comma));
        if (!Consume(TokenType.RightBracket, "Expected ']'.")) return null;
        return new Range(range, list);
    }
    




    // public class ParseError : System.Exception
    // {}

    IExpression expression()
    {
        return logic();
    }

    IExpression logic ()
    {
            IExpression expr = equality();
            List<TokenType> operators = new List<TokenType> { TokenType.And, TokenType.Or };
            while (Match(operators))
            {
                Token oper = previous();
                IExpression right = equality();
                expr = new Binary(expr, oper, right);
            }
            return expr;
    }

    IExpression equality() 
    {
        IExpression expr = comparison();
        List<TokenType> types = new List<TokenType>() {TokenType.BangEqual, TokenType.EqualEqual};
        while (Match(types)) 
        {
            Token binaryoperator = previous();
            IExpression right = comparison();
            expr = new Binary(expr, binaryoperator, right);
        }
        return expr;
    }

    IExpression comparison()
    {
        IExpression expr = term();
        List<TokenType> types = new List<TokenType> () {TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual};
        while (Match(types))
        {
            Token binaryoperator = previous();
            IExpression right  = term();
            expr = new Binary(expr, binaryoperator, right); 
        }
        return expr;
    }

    IExpression term()
    {
        IExpression expr = factor();
        List<TokenType> types = new List<TokenType> () {TokenType.Plus, TokenType.Minus, TokenType.AtSymbol, TokenType.AtSymbolAtSymbol};
        while (Match(types))
        {
            Token binaryoperator = previous();
            IExpression right = factor();
            expr = new Binary(expr, binaryoperator,right);
        }
        return expr;
    }

    IExpression factor()
    {
        IExpression expr = pow();
        List<TokenType> types = new List<TokenType> () { TokenType.Mul,TokenType.Slash};
        while (Match(types))
        {
            Token binaryoperator = previous();
            IExpression right = pow();
            expr = new Binary(expr, binaryoperator,right);
        }
        return expr;
    }

    IExpression pow()
    {
        IExpression expr = unary();
        List<TokenType> types = new List<TokenType> () {TokenType.Pow};
        while (Match(types))
        {
            Token binaryoperator = previous();
            IExpression right = unary();
            expr = new Binary(expr, binaryoperator,right);
        }
        return expr;
    }

    IExpression unary()
    {
        List<TokenType> types = new List<TokenType> () {TokenType.Minus,TokenType.Bang};
        if (Match(types))
        {
            Token unaryoperator = previous();
            IExpression right = unary();
            return new Unary(unaryoperator,right);
        }
        return call();
    }

    IExpression call()
    {
        IExpression expr = primary();
        while(true)
        {
            if(Match(TokenType.LeftParen))
            {
                expr = finishcall(expr);
            }
            else if (Match(TokenType.Dot))
            {
                List<TokenType> properties = new List<TokenType> {TokenType.Faction, TokenType.Type, TokenType.Power, TokenType.Range};
                int c = 0;
                foreach(TokenType tokentype in properties)
                {
                    
                    if (Check(tokentype)) c++;
                }
                if (c == 0)
                {
                    if (!Consume(TokenType.Identifier, "Expected property name after '.'.")) return null;
                }
                advance();
                Token name = previous();
                expr = new Access(expr,name);
            }
            else
            {
                break;
            }
        }
        return expr;

    }

    IExpression finishcall(IExpression callee)
    {   
        List<IExpression> arguments = new List<IExpression> ();
        if(!Check(TokenType.RightParen))
        {
            do
            {
                arguments.Add(expression());
            }
            while(Match(TokenType.Comma));
        }

        if (!Consume(TokenType.RightParen, "Expected ')' after arguments.")) return null;
        Token paren = previous();
        return new Call(callee, paren, arguments);   
    }

    IExpression primary()
    {
        if (Match(TokenType.False)) return new Literal(false);
        if (Match(TokenType.True)) return new Literal(true);
        List<TokenType> types = new List<TokenType> () { TokenType.NumberLiteral, TokenType.StringLiteral};
        if (Match(types)) 
        {
        return new Literal(previous().literal);    
        }

        if (Match(TokenType.LeftParen))
        {
            IExpression expr = expression();
            if (!Consume(TokenType.RightParen, "Expected ')' after expression.")) return null;
            return new Grouping(expr);
        }

        if (Match(TokenType.Identifier)) 
        {
            return new Variable(previous());
        }   
        DSL.error(Peek(), "Expect expression.");
        return null;
    }

    IStatement declaration()
    {
        try
        {
            if( Match(TokenType.Identifier)) return VarDeclaration();
            return statement();
        }
        catch(Exception error)
        {
            synchronize();
            return null;
        }
    }
    IStatement statement()
    {   
        if(Match(TokenType.For)) return forStatement();
        if(Match(TokenType.While))  return whileStatement();
        if(Match(TokenType.LeftBrace)) return new Block(block());
        return expressionStatement();
    }


    IStatement forStatement()
    {
        if (!Consume(TokenType.Identifier, "Expected 'identifier'.")) return null;
        Token iter = previous();
        if (!Consume(TokenType.In, "Expected 'in' in 'for' declaration.")) return null;
        IExpression list = call();
        IStatement body = declaration();
        return new For(iter, list, body);
    }

    IStatement expressionStatement()
    {
        IExpression expr = expression();
        consume(TokenType.Semicolon, "Expect ';' after expression.");
        return new Expression(expr);
    }

    List<IStatement> block()
    {
        List<IStatement> statements = new List<IStatement>();

        while(!Check(TokenType.RightBrace) && !IsAtEnd())
        {
            statements.Add(declaration());
        }
        consume(TokenType.RightBrace,  "Expect '}' after block.");
        consume(TokenType.Semicolon, "Expected ';'.");
        return statements;
    }
    IExpression assignment()
    {
        IExpression expr = or();
        Token equal = previous();
        if( Match(TokenType.Equal))
        {
            IExpression value = assignment();
            if (expr is Variable)
            {
                Token name = ((Variable)expr).name;
                return new Assign(name, value);
            }
            else
            {
                DSL.error(equal, "Invalid assignment target.");
            }
        }        
        return expr;
    }

    IExpression or()
    {
        IExpression expr = and();
        while(Match(TokenType.Or))
        {
            Token logicaloperator = previous();
            IExpression right = and();
            expr = new Logical(expr,logicaloperator, right);
        }
        return expr;
    }

    IExpression and()
    {
        IExpression expr = equality();
        while(Match(TokenType.And))
        {
            Token logicaloperator = previous();
            IExpression right = equality();
            expr = new Logical(expr,logicaloperator, right);
        }
        return expr;
    }
    IStatement VarDeclaration()
    {
        Token name = consume(TokenType.Identifier, "Expect variable name.");
        Token oper = Peek();
        IExpression initializer = null;
        if(Match(TokenType.Equal))
        {
            initializer = expression();
        }
        consume(TokenType.Semicolon,  "Expect ';' after variable declaration.");
        return new Var(name,oper,initializer);
    }

    IStatement whileStatement()
    {
        consume(TokenType.LeftParen, "Expect '(' after 'while'.");
        IExpression condition = expression();
        consume(TokenType.RightParen, "Expect ')' after condition.");
        IStatement body = declaration();

        return new While(condition, body);
    }
    //Rules
    

    //Parsing Functions
    bool Match(List<TokenType> types)
    {
        foreach(TokenType type in types)
        {
            if (Check(type)) 
            {
                advance();
                return true;
            }
        }
        return false;
    }

    bool Match(TokenType type)
    {
        if (Check(type))
        {
            advance();
            return true;
        }
        return false;
    }

    Token consume(TokenType type, string message)
    {
        if (Check(type)) return advance();
        DSL.error(Peek(), message);
        return null;
    }
    private bool Consume(TokenType type, string message)
    {
        if (Check(type))
        {
            advance();
            return true;
        }
        DSL.error(Peek(), message);
        synchronize();
        return false;
    }

    bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().type == type;
    }
    private bool Check(string oper)
    {
        if (IsAtEnd()) return false;
        return Peek().lexeme == oper;
    }
    Token advance()
    {
        if (!IsAtEnd()) current++;
        return previous();
    }

    bool IsAtEnd() 
    {
        return Peek().type == TokenType.EOF;
    }
    Token Peek() 
    {
        return tokens[current];
    }
    Token previous() 
    {
        return tokens[current - 1];
    }    

    // ParseError error(Token token, string message)
    // {
    //     DSL.error(token,message);
    //     return new ParseError();
    // }

    void synchronize()
    {
        advance();
            
            Dictionary<string, TokenType> StatementBeginning = Lexer.keywords;
            while (!IsAtEnd())
            {
                if (previous().type == TokenType.Semicolon || StatementBeginning.ContainsValue(Peek().type) || Peek().type == TokenType.Identifier) return;
                advance();
            }
    }
}
