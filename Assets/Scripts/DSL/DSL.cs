using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using UnityEngine;
// DSL.print();
public static class DSL 
{
//     static string code =  @"
// card {
//     Type: ""Oro"",
//     Name: ""Ciri"",
//     Faction: ""Neutral"",
//     Power: 10,
//     Range: [""Melee"", ""Ranged""],
//     OnActivation: [
//         {
//             Effect: ""Teleport"",  // Error: Teleport no está declarado
//             Amount: 2,
//             Selector: {
//                 Source: ""board"",
//                 Single: true,
//                 Predicate: (unit) => unit.Faction == ""Nilfgaard""
//             }
//         }
//     ]
// }
// ";
//     public static void print()
//     {
//         Lexer lexer = new Lexer(code);
//         foreach (Token token in lexer.tokens) System.Console.WriteLine(token.ToString());
//     }
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
}

