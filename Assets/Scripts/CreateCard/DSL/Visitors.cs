using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public interface IVisitor
{
    object VisitLiteral(Literal literal);
    object VisitUnary(Unary unary);
    object VisitBinary(Binary binary);
    object VisitGrouping(Grouping grouping);
    object VisitVariableExpr(Variable variable);
    object VisitExpressionStmt(Expression stmt);
    object VisitVarStmt(Var stmt);
    object VisitAssignExpr(Assign assign);
    object VisitBlockStmt(Block stmt);
    object VisitLogicalExpr(Logical expr);
    object VisitWhileStmt(While stmt);
    object VisitCallExpr(Call call);
    object VisitForStmt(For stmt);
    
    object VisitTypeProp(Type prop);
    object VisitNameProp(Name prop);
    object VisitFactionProp(Faction prop);
    object VisitPowerProp(Power prop);
    object VisitSourceProp(Source prop);
    object VisitSingleProp(Single prop);
    object VisitPredicateProp(Predicate prop);
    object VisitParamValueProp(ParamValue param);
    object VisitRangeProp(Range param);
    object VisitSelectorMethod(Selector selector);
    object VisitEffectMethod(Effect effect);
    object VisitOnActivationMethod(OnActivation prop);
    object VisitOnActBodyMethod(OnActBody prop);
    object VisitCardClassClass(CardClass prop);
    
}
