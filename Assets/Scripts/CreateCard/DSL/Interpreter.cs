using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;


public class Interpreter : IVisitor
{
    
    public object VisitLiteral(Literal expr)
    {
        return expr.value;
    }

    public object VisitGrouping(Grouping expr)
    {
        return Evaluate(expr.expression);
    }

    public object VisitUnary(Unary expr)
    {
        object right = Evaluate(expr.right);
        switch (expr.unaryoperator.type)
        {
            case TokenType.Bang:
                return !IsTruthy(right);
            case TokenType.Minus:
                return -(long)right;
        }
        return null;
    }

    public object VisitBinary(Binary expr)
    {
        object left = Evaluate(expr.left);   
        object right = Evaluate(expr.right);
        
        switch (expr.binaryoperator.type)
        {
            case TokenType.Greater:
                return (long)left > (long)right;

            case TokenType.GreaterEqual:
                return (long)left >= (long)right;

            case TokenType.Less:
                return (long)left < (long)right;

            case TokenType.LessEqual:
                return (long)left <= (long)right;

            case TokenType.Minus:
                return (long)left - (long)right;

            case TokenType.EqualEqual:
                return IsEqual(left, right);

            case TokenType.BangEqual:
                return !IsEqual(left, right);
            case TokenType.Plus:
                if (left is long && right is long)
                    return (long)left + (long)right;
                if (left is string && right is string)
                    return (string)left + (string)right; // Concatenar cadenas
                throw new ("Error: Tipos incompatibles para '+'."); // TODO Fix ParseError

            case TokenType.Slash:
                return (long)left / (long)right;

            case TokenType.Mul:
                return (long)left * (long)right;

            case TokenType.Pow:
                return Math.Pow((long)left, (long)right);    
        }

        return null;
    }

    private bool IsTruthy(object obj)
    {
        if (obj == null) return false;
        if (obj is bool) return (bool)obj;
        return true;
    }
    
    private bool IsEqual(object left, object right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        return left.Equals(right);
    }
    private object Evaluate(IExpression expr)
    {
        return expr.Accept(this);
    }
}
