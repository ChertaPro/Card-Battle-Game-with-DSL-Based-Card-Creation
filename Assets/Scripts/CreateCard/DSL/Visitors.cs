using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitor
{
    object VisitLiteral(Literal literal);
    object VisitUnary(Unary unary);
    object VisitBinary(Binary binary);
    object VisitGrouping(Grouping grouping);
}
