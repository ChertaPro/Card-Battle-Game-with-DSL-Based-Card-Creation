using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICallable 
{
    int Arity();
    object call(Interpreter interpreter, List<object> arguments);
}
