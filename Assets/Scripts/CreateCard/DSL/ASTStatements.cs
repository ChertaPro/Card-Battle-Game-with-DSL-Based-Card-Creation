using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IStatement
{
    public object Accept(IVisitor visitor);

}

public class Expression : IStatement
{
    public Expression(IExpression expression)
    {
        this.expression = expression;
    }
    public IExpression expression;

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitExpressionStmt(this);
    }
}

public class Var : IStatement
{
    public Var(Token name, IExpression initializer)
    {
        this.name = name;
        //this.type = type;
        this.initializer = initializer;
    }

    public Token name;
    //public Token type;
    public IExpression initializer;

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitVarStmt(this);
    }
}

public class Block : IStatement
{
    public Block(List<IStatement> statements)
    {
        this.statements = statements;
    }
    public List<IStatement> statements;

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitBlockStmt(this);
    }
}

public class While : IStatement
{
    public While(IExpression condition, IStatement body)
    {
        this.condition = condition;
        this.body = body;
    }

    public IExpression condition;
    public IStatement body;

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitWhileStmt(this);
    }
}

