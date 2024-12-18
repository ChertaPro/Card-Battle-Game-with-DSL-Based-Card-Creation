using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
public static class DSL 
{
    static CompileButton console ;
    private static  Interpreter interpreter = new Interpreter();
    static bool hadError = false;
    static bool hadRuntimeError = false;
    public static List<Card> Cardscreated = new List<Card> ();
    public static Dictionary<string, Method> onActs { get; set; }
    public static List<EffectClass> effects { get; set; }
    public static void Error(int line, int column, string message) 
    {
        Report(line, column, "", message);
    }
    public static void Report(int line, int column, string where, string message) 
    {
        Debug.Log($"[Line {line}, Column {column}] Error {where}: " + message);
        hadError = true;
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

    public static void runtimeError(RuntimeError error)
    {
        Console.Error.WriteLine(error.Message + 
        "\n[line " + error.token.line + ", column " + error.token.column + "]");
        hadRuntimeError = true;
    }

    public static void Compile(string code)
    {   
        console = GameObject.Find("Compile").GetComponent<CompileButton>();

        hadError = false;

        if (code == "")
        {
            Debug.LogError("Empty code");
            Debug.LogError("Invalid code\n");
            return;
        }

        Lexer lexer = new Lexer(code);

        var tokens = lexer.ScanTokens();
        // Debug.Log(code);
        // foreach (var token in tokens)
        // {
        //     Debug.Log(token.ToString());
        // }
        
        if(hadError)
        {
            Debug.LogError("Invalid code\n");
            return;
        }

        Parser parser = new Parser(tokens);
        List<GameEntity> GameEntities = parser.Parse();


        if(hadError){
            Debug.LogError("Invalid code\n");
            return;
        }

        Interpreter interpreter = new Interpreter();
        Dictionary<Card, OnActivation> pairs = interpreter.CreateCards(GameEntities);

        if(hadError){
            Debug.LogError("Invalid code\n");
            return;
        }

        onActs = new Dictionary<string, Method>();
        foreach(var pair in pairs)
        {
            Cardscreated.Add(pair.Key);
            onActs[pair.Key.cardname] = pair.Value;
        }


        Debug.Log("Successfull Compilation");
    }

}

