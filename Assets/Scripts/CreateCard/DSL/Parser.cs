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
    List<IStatement> parse()
    {
        List<IStatement> statements= new List<IStatement>();

        while (!IsAtEnd()) 
        {
            statements.Add(declaration());
        }
        return statements;
    }
    public class ParseError : System.Exception
    {}  
    public List<Token> tokens;
    int current = 0;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    IExpression expression()
    {
        return assignment();
    }

    IStatement declaration()
    {
        try
        {
            if( Match(TokenType.Identifier)) return VarDeclaration();
            return statement();
        }
        catch(ParseError error)
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
        consume(TokenType.LeftParen, "Expect '(' after 'for'.");
        IStatement initializer;
        if(Match(TokenType.Semicolon))
        {
            initializer = null;
        }
        else if (Match(TokenType.Identifier))
        {
            initializer = VarDeclaration();
        }
        else
        {
            initializer = expressionStatement();
        }
        IExpression condition = null;
        if (!Check(TokenType.Semicolon))
        {
            condition = expression();
        }
        consume(TokenType.Semicolon, "Expect ';' after loop condition.");
        IExpression increment = null;
        if(!Check(TokenType.RightParen))
        {
            increment = expression();
        }
        consume(TokenType.RightParen, "Expect ')' after for clauses.");
        IStatement body = statement();
        if(increment != null)
        {
            body = new Block(new List<IStatement> { body, new Expression(increment) });
        }
        if (condition == null) condition = new Literal(true); // Si no hay condición, será `true` por defecto.
        body = new While(condition, body);

        if (initializer != null)
        {
            body = new Block(new List<IStatement> { initializer, body });
        }

        return body;
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
        }

        error(equal, "Invalid assignment target.");

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

        IExpression initializer = null;
        if(Match(TokenType.Equal))
        {
            initializer = expression();
        }
        consume(TokenType.Semicolon,  "Expect ';' after variable declaration.");
        return new Var(name,initializer);
    }

    IStatement whileStatement()
    {
        consume(TokenType.LeftParen, "Expect '(' after 'while'.");
        IExpression condition = expression();
        consume(TokenType.RightParen, "Expect ')' after condition.");
        IStatement body = statement();

        return new While(condition, body);
    }
    //Rules
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
        List<TokenType> types = new List<TokenType> () {TokenType.Plus, TokenType.Minus};
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
            return call();
        }
        return primary();
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

        Token paren = consume(TokenType.RightParen, "Exprect ')' after arguments.");
        return new Call(callee,paren,arguments);    
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
            consume(TokenType.RightParen,  "Expect ')' after expression.");
            return new Grouping(expr);
        }

        if (Match(TokenType.Identifier)) 
        {
            return new Variable(previous());
        }   
        throw error(Peek(), "Expect expression.");
    }


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
        throw error(Peek(), message);
    }
    bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().type == type;
    }

    Token advance()
    {
        if (IsAtEnd()) current++;
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

    ParseError error(Token token, string message)
    {
        DSL.error(token,message);
        return new ParseError();
    }

    void synchronize()
    {
        advance();
        while (!IsAtEnd()) 
        {
            if (previous().type == TokenType.Semicolon) return;
        }

        switch (Peek().type)
        {
            case TokenType.For:
            case TokenType.While:    
            return;
        }

        advance();
    }
}
