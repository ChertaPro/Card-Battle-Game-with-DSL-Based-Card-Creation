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



