using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Class
{
    public abstract object Accept(IVisitor visitor);
}

public class CardClass : Class
{
    public Prop type;
    public Prop name;
    public Prop faction;
    public Prop power;
    public Prop range;
    public Method onActivation;

    public CardClass(Prop type, Prop name, Prop faction, Prop power, Prop range, Method onActivation)
    {
        this.type = type;
        this.name = name;
        this.faction = faction;
        this.power = power;
        this.range = range;
        this.onActivation = onActivation;
    }

    public override object Accept(IVisitor visitor)
    {
        return visitor.VisitCardClassClass(this);
    }    
}