using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity
{
    public abstract object Accept(IVisitor visitor);
}

public class CardClass : GameEntity
{
    public Prop type;
    public Prop name;
    public Prop faction;
    public Prop power;
    public Prop range;
    public OnActivation onActivation;

    public CardClass(Prop type, Prop name, Prop faction, Prop power, Prop range, OnActivation onActivation)
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
        return visitor.VisitCardClass(this);
    }    
}

public class EffectClass : GameEntity
{

    public Prop name;
    public Method parameters;
    public Method action;

    public EffectClass(Prop name, Method parameters, Method action)
    {
        this.name = name;
        this.parameters = parameters;
        this.action = action;
    }

    public override object Accept(IVisitor visitor)
    {
        return visitor.VisitEffectClass(this);
    }
}