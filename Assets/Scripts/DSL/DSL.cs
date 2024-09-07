using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DSL 
{
    static bool hadError = false;
    public static void Error(int line, string message) 
    {
        Report(line, "", message);
        hadError = true;
    }
    public static void Report(int line, string where, string message) 
    {
        System.Console.WriteLine($"[Line {line}] Error {where}: " + message);
    }
}
