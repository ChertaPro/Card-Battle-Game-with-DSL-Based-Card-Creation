using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IExpression
{
    public object Accept(IVisitor visitor); // El método Accept toma un visitor

}

public class Literal : IExpression
{
    public object value { get; }

    public Literal(object value)
    {
        this.value = value;
    }
    public  object Accept(IVisitor visitor)
    {
        return visitor.VisitLiteral(this);
    }
}

public class Unary : IExpression
{
    public Token unaryoperator;
    public IExpression right;

    public Unary(Token unaryoperator, IExpression right)
    {
        this.unaryoperator = unaryoperator;
        this.right = right;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitUnary(this);
    }

}
public class Logical : IExpression
{
    public Logical(IExpression left, Token logicaloperator, IExpression right)
    {
        this.left = left;
        this.logicaloperator = logicaloperator;
        this.right = right;
    }

    public IExpression left;
    public Token logicaloperator;
    public IExpression right;

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitLogicalExpr(this);
    }
}
public class Assign : IExpression
{
    public Assign(Token name, IExpression value)
    {
        this.name = name;
        this.value = value;
    }

    public Token name;
    public IExpression value;

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitAssignExpr(this);
    }
}
public class Binary : IExpression
{
    public IExpression left;
    public IExpression right;
    public Token binaryoperator;
    public Binary(IExpression left,Token binaryoperator, IExpression right)
    {
        this.left = left;
        this.right = right;
        this.binaryoperator = binaryoperator;   
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitBinary(this);
    }
}

public class Call : IExpression
{
    public IExpression callee;
    public Token paren;
    public List<IExpression> arguments;

    public Call(IExpression callee, Token paren, List<IExpression> arguments)
    {
        this.callee = callee;
        this.paren = paren;
        this.arguments = arguments;
    }	

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitCallExpr(this);
    }
}
public class Grouping : IExpression
{
    public IExpression expression { get; }

    public Grouping(IExpression expression)
    {
        this.expression = expression;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitGrouping(this);
    }    
}

public class Variable : IExpression
{
    public Token name;
    public IExpression initializer;

    public Variable(Token name)
    {
        this.name = name;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitVariableExpr(this);
    }
}


