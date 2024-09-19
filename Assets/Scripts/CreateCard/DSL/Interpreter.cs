using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


public class Interpreter : IVisitor
{
    public Environment globals = new Environment();
    public Environment environment = new Environment();
    public Dictionary<IExpression,int> locals = new Dictionary<IExpression,int>();
    internal Interpreter()
    {
        environment = globals;
    }
    void interpret(List<IStatement> statements)
    {
        try
        {
            foreach(IStatement statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError error)
        {
            DSL.runtimeError(error);
        }
    }   

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
            CheckNumberOperand(expr.unaryoperator,right);
                return -(long)right;
        }
        return null;
    }

    void CheckNumberOperand(Token unaryoperator, object operand)
    {
        if (operand is long) return;
        throw new RuntimeError(unaryoperator,"Operand must be a number.");
    }

    public object VisitBinary(Binary expr)
    {
        object left = Evaluate(expr.left);   
        object right = Evaluate(expr.right);
        
        switch (expr.binaryoperator.type)
        {
            //Comparison
            case TokenType.Greater:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left > (long)right;

            case TokenType.GreaterEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left >= (long)right;

            case TokenType.Less:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left < (long)right;

            case TokenType.LessEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left <= (long)right;

            case TokenType.EqualEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return IsEqual(left, right);

            case TokenType.BangEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return !IsEqual(left, right);

            //Operations
            case TokenType.Minus:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left - (long)right;

            case TokenType.Plus:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left + (long)right;

            case TokenType.Slash:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left / (long)right;

            case TokenType.Mul:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (long)left * (long)right;

            case TokenType.Pow:
            CheckNumberOperands(expr.binaryoperator, left, right);
                return Math.Pow((long)left, (long)right); 

            case TokenType.AtSymbol: return left.ToString() + right.ToString();
            case TokenType.AtSymbolAtSymbol: return left.ToString() + " " + right.ToString();
        }

        return null;
    }

    public object VisitCallExpr(Call expr)
    {
        object callee = Evaluate(expr.callee);
        List<object> arguments = new List<object>();    

        foreach (IExpression argument in expr.arguments)
        {
            arguments.Add(Evaluate(argument));
        }
        if (!(callee is ICallable))
        {
            throw new RuntimeError(expr.paren, "Can only call functions.");
        }
        ICallable function = (ICallable)callee;
        if (arguments.Count != function.Arity())
        {
            throw new RuntimeError(expr.paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
        }
        return function.call(this,arguments); 
    }

    void CheckNumberOperands(Token binaryoperator, object left, object right)
    {
        if (left is long && right is long) return;
        throw new RuntimeError(binaryoperator, "Operands must be numbers.");

    }

    public object VisitVariableExpr(Variable expr)
    {
        return lookUpVariable(expr.name, expr);
    }

    object lookUpVariable(Token name, IExpression expr)
    {
        int? distance = locals[expr];
        if(distance != null)
        {
            return environment.GetAt(distance, name);
        }
        else
        {
            return globals.Get(name);
        }
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

    void Execute(IStatement statement)
    {
        statement.Accept(this);
    }
    public void resolve(IExpression expr, int depth) 
    {
        locals.Add(expr, depth);
    }

    public void executeBlock(List<IStatement> statements, Environment environment)
    {
        Environment previous = this.environment;
        this.environment = environment;
        foreach (IStatement stmt in statements)
        {
            Execute(stmt);
        }
        this.environment = previous;
    }

    public object VisitBlockStmt(Block stmt)
    {
        executeBlock(stmt.statements, new Environment(environment));
        return null;
    }
    public object VisitExpressionStmt(Expression stmt)
    {
        Evaluate(stmt.expression);
        return null;
    }

    public object VisitVarStmt(Var stmt)
    {
        object value = null;
        if (stmt.initializer != null)
        {
            value = Evaluate(stmt.initializer);
        }

        environment.Define(stmt.name.lexeme, value);
        return null;
    }

    public object VisitAssignExpr(Assign expr)
    {
        object value = Evaluate(expr.value);
        environment.assign(expr.name, value);
        return value;
    }

    public object VisitLogicalExpr(Logical expr)
    {
        object left = Evaluate(expr.left);

        if(expr.logicaloperator.type == TokenType.Or)
        {
            if (IsTruthy(left)) return left;
        }
        else
        {
            if(!IsTruthy(left)) return left;
        }
        return Evaluate(expr.right);
    }

    public object VisitWhileStmt(While stmt)
    {
        while(IsTruthy(Evaluate(stmt.condition)))
        {
            Execute(stmt.body);
        }
        return null;
    }

}
