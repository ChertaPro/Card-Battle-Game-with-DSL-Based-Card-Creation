using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Environment
{
    public Environment enclosing;
    private Dictionary<string,object> values = new Dictionary<string,object>();

    public Environment()
    {
        enclosing = null;
    }
    public Environment(Environment enclosing)
    {
        this.enclosing = enclosing;
    }

    public object Get(Token name)
    {
        if(values.ContainsKey(name.lexeme))
        {
            return values[name.lexeme];
        }
        if (enclosing != null)
        {
            return enclosing.Get(name);
        }
        throw new RuntimeError (name, "Undefined variable '" + name.lexeme + "'.");
    }

    public void assign(Token name, object value)
    {
        if(values.ContainsKey(name.lexeme))
        {
            values[name.lexeme] = value;
            return;
        }
        if (enclosing != null)
        {
            enclosing.assign(name, value);
            return;
        }
        throw new RuntimeError(name,"Undefined variable '" + name.lexeme + "'.");
    }

    public void Define(string name, object value)
    {
        values.Add(name, value);
    }

    public object GetAt(int? distance, Token name)
    {
        return ancestor(distance).Get(name);
    }
    public Environment ancestor(int? distance)
    {
        Environment environment = this;
        for (int i = 0; i<distance; i++)
        {
            environment = environment.enclosing;
        }
        return environment;
    }
}
