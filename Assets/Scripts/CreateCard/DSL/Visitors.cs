using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public interface IVisitor
{
    //*Visit Expr
    object VisitLiteral(Literal literal);
    object VisitUnary(Unary unary);
    object VisitBinary(Binary binary);
    object VisitGrouping(Grouping grouping);
    object VisitVariableExpr(Variable variable); 
    object VisitCallExpr(Call call);
    object VisitAccessExpr(Access access);
    object VisitSetExpr(Set set);
    object VisitPostoperation(Postoperation oper);
    object VisitPreoperation(Preoperation oper);

    
    //*Visit Stmt 
    object VisitExpressionStmt(Expression stmt);
    object VisitVarStmt(Var stmt);
    object VisitBlockStmt(Block stmt);
    object VisitWhileStmt(While stmt);
    object VisitForStmt(For stmt);
    
    //*VisitPropandMethods
    object VisitTypeProp(Type type);
    object VisitNameProp(Name name);
    object VisitFactionProp(Faction faction);
    object VisitPowerProp(Power power);
    object VisitSourceProp(Source source);
    object VisitSingleProp(Single single);
    object VisitPredicateProp(Predicate predicate);
    object VisitParamValueProp(ParamValue param);
    object VisitRangeProp(Range range);
    object VisitParamDeclarationProp(ParamDeclaration param);
    object VisitSelectorMethod(Selector selector);
    object VisitEffectMethod(Effect effect);
    object VisitOnActivationMethod(OnActivation onActivation);
    object VisitOnActBodyMethod(OnActBody onActBody);
    object VisitParamsMethod(Params param);
    object VisitActionMethod(Action action);

    //*Visit GameEntities
    object VisitCardClass(CardClass prop);
    object VisitEffectClass(EffectClass prop);
    
}
