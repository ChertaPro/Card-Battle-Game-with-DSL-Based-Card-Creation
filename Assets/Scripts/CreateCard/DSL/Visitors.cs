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
    // object VisitClassStmt(Class stmt);
    // object VisitFunctionStmt(Function stmt);
    // object VisitReturnStmt(Return stmt);
    
}
