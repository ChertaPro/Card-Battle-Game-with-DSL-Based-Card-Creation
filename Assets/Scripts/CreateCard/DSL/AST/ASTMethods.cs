using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Method
{
    public object Accept(IVisitor visitor);
}

public class OnActivation : Method
{
    public List<Method> onActBodies;

    public OnActivation(List<Method> onActBodies)
    {
        this.onActBodies = onActBodies;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitOnActivationMethod(this);
    } 
}
public class OnActBody : Method
{
    public Method effect;
    public Method selector;
    public Method postAction;

    public OnActBody(Method effect, Method selector, Method postAction)
    {
        this.effect = effect;
        this.selector = selector;
        this.postAction = postAction;
    }

    public object Accept(IVisitor visitor)
    { 
        return visitor.VisitOnActBodyMethod(this);
    }
}

public class Effect : Method
{
    public Token effect;
    public Prop name;
    public IExpression exprName;
    public List<Prop> paramsv;

    public Effect(Token effect, IExpression name)
    {
        this.effect = effect;
        this.exprName = name;
        this.name = null;
        this.paramsv = null;
    }

    public Effect(Token effect, Prop name, List<Prop> paramsv)
    {
        this.effect = effect;
        this.name = name;
        this.paramsv = paramsv;
        this.exprName = null;
    }

    public object Accept(IVisitor visitor)
    {
        return visitor.VisitEffectMethod(this);
    }     
}

public class Selector : Method
{
    public Token selector;
    public Prop source;
    public Prop single;
    public Prop predicate;

    public Selector(Token selector, Prop source, Prop single, Prop predicate)
    {
        this.selector = selector;
        this.source = source;
        this.single = single;
        this.predicate = predicate;
    }
    public object Accept(IVisitor visitor)
    {
        return visitor.VisitSelectorMethod(this);
    }
}