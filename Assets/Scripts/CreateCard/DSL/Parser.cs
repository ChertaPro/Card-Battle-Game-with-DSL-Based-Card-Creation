using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class Parser
{
    IExpression parse()
    {
        try
        {
            return expression();
        }
        catch (ParseError error)
        {
            return null;
        }
    }
    public class ParseError : System.Exception
    {}  
    List<Token> tokens;
    int current = 0;

    Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    IExpression expression()
    {
        return equality();
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
            return new Unary(unaryoperator, right);
        }
        return primary();
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
