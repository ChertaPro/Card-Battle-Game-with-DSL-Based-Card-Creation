using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  Prop
{
    public object Accept(IVisitor visitor);
}

public class Name : Prop
{
    public Token name;
    public IExpression value;

    public Name(Token name, IExpression value)
    {
        this.name = name;
        this.value = value;
    }

    public object Accept(IVisitor visitor) 
    {
        return visitor.VisitNameProp(this);
    }
}
public class Type : Prop
{
    public Token type;
    public IExpression value;

    public Type(Token type, IExpression value)
    {
        this.type = type;
        this.value = value;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitTypeProp(this);
    }
}    

public class Faction : Prop
{
    public Token faction;
    public IExpression value;

    public Faction(Token faction, IExpression value)
    {
        this.faction = faction;
        this.value = value;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitFactionProp(this);
    }
}

public class Power : Prop
{
    public Token power;
    public IExpression value;

    public Power(Token power, IExpression value)
    {
        this.power = power;
        this.value = value;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitPowerProp(this);
    }
}

public class Range : Prop
{
    public Token range;
    public List<IExpression> list;

    public Range(Token range, List<IExpression> list)
    {
        this.range = range;
        this.list = list;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitRangeProp(this);
    }
}

public class Source : Prop
{
    public Token source;
    public IExpression value;

    public Source(Token source, IExpression value)
    {
        this.source = source;
        this.value = value;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitSourceProp(this);
    }
}

public class Single : Prop
{
    public Token single;
    public IExpression value;

    public Single(Token single, IExpression value)
    {
        this.single = single;
        this.value = value;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitSingleProp(this);
    }
}

public class Predicate : Prop
{
    public Token predicate;
    public Token card;
    public IExpression condition;

    public Predicate(Token predicate, Token card, IExpression condition)
    {
        this.predicate = predicate;
        this.card = card;
        this.condition = condition;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitPredicateProp(this);
    }
}

public class ParamValue : Prop
{
    public Token name;
    public IExpression value;

    public ParamValue(Token name, IExpression value)
    {
        this.name = name;
        this.value = value;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitParamValueProp(this);
    } 
}

public class ParamDeclaration : Prop
{
    public Token name;
    public Token value;

    public ParamDeclaration(Token name, Token value)
    {
        this.name = name;
        this.value = value;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitParamDeclarationProp(this);
    } 
}