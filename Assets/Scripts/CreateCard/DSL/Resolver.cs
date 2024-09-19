using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Resolver : IVisitor
{
    Resolver(Interpreter interpreter)
    {
        this.interpreter = interpreter;
    }
    public Interpreter interpreter;
    public Stack<Dictionary<string,bool>> scopes = new Stack<Dictionary<string,bool>>();

    void resolve(List<IStatement> statements)
    {
        foreach(IStatement statement in statements)
        {
            resolve(statement);
        }
    }


    public object VisitBlockStmt(Block stmt)
    {
        beginScope();
        resolve(stmt.statements);
        endScope();
        return null;
    }

    public object VisitExpressionStmt(Expression stmt) 
    {
        resolve(stmt.expression);
        return null;
    }
    public object VisitVarStmt(Var stmt)
    {
        declare(stmt.name);
        if(stmt.initializer != null)
        {
            resolve(stmt.initializer);
        }
        define(stmt.name);
        return null;
    }
    public object VisitWhileStmt(While stmt) 
    {
        resolve(stmt.condition);
        resolve(stmt.body);
        return null;
    }
    public object VisitAssignExpr(Assign expr) 
    {
        resolve(expr.value);
        resolveLocal(expr, expr.name);
        return null;
    }
    public object VisitBinary(Binary expr) 
    {
        resolve(expr.left);
        resolve(expr.right);
        return null;
    }

    public object VisitCallExpr(Call expr)
    {
        resolve(expr.callee);
        
        foreach (var argument in expr.arguments)
        {
            resolve(argument);
        }
        
        return null;
    }   
    public object VisitGrouping(Grouping expr) 
    {
        resolve(expr.expression);
        return null;
    }
    public object VisitLiteral(Literal expr) 
    {
        return null;
    }
    public object VisitLogicalExpr(Logical expr) 
    {
        resolve(expr.left);
        resolve(expr.right);
        return null;
    }       
    public object VisitUnary(Unary expr) 
    {
        resolve(expr.right);
        return null;
    } 
    public object VisitVariableExpr(Variable expr)
    {
        if(!IsEmpty() && scopes.Peek()[expr.name.lexeme] == false)
        {
            DSL.error(expr.name, "Can't read local variable in its own initializer.");
        }
        resolveLocal(expr, expr.name);
        return null;
    }
    public void resolve(IStatement stmt)
    {
        stmt.Accept(this);
    }

    public void resolve(IExpression expr)
    {
        expr.Accept(this);
    }

    void beginScope()
    {
        scopes.Push( new Dictionary<string, bool>());
    }

    void endScope()
    {
        scopes.Pop();
    }

    void declare(Token name)
    {
        if (IsEmpty()) return ;
        Dictionary<string, bool> scope = scopes.Peek();
        if (scope.ContainsKey(name.lexeme))
        {
            DSL.error(name,"Already variable with this name in this scope.");
        }
        scope.Add(name.lexeme, false);
    }
    void define(Token name)
    {
        if(IsEmpty()) return ;
        scopes.Peek().Add(name.lexeme, true);
    }

    void resolveLocal(IExpression expr, Token name)
    {
        int depth = 0;
        foreach (var scope in scopes.Reverse()) // Recorremos la pila en orden inverso
        {
            if (scope.ContainsKey(name.lexeme))  // Verificamos si el diccionario tiene la clave
            {
                interpreter.resolve(expr, depth);
                return;
            }
            depth++;
        }
    }

    public bool IsEmpty()
    {
        return scopes.Count == 0;
    }



}
