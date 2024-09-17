using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using UnityEngine;
// DSL.print();
public static class DSL 
{
    static bool hadError = false;
    public static void Error(int line, int column, string message) 
    {
        Report(line, column, "", message);
        hadError = true;
    }
    public static void Report(int line, int column, string where, string message) 
    {
        System.Console.WriteLine($"[Line {line}, Column {column}] Error {where}: " + message);
    }

    public static void error(Token token, string message)
    {
        if (token.type == TokenType.EOF)
        {
            Report(token.line, token.column, " at end", message);
        }
        else
        {
            Report(token.line, token.column, " at '" + token.lexeme + "'", message);
        }
    }

    
}

