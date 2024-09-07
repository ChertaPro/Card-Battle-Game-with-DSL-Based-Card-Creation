using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexer 
{
    
}
public enum TokenType 
{
    //1. Single Character Tokens
    LeftParen, RightParen, //Paréntesis izquierdo y derecho (, )
    LeftBracket, RightBracket, //Corchetes izquierdo y derecho [, ]
    LeftBrace, RightBrace, //Llaves izquierda y derecha {, }
    Comma, //Coma ,
    Dot, //Punto .
    Colon,//Dos puntos :
    Semicolon, //Punto y coma ;
    Slash, //Barra /
    Star, //Asterisco *
    Mod, //Operador módulo %
    Pow, //Operador potencia ^ 
    AtSymbol, //Arroba @

    //2. One or Two Character Tokens
    Bang, //Signo de exclamación !
    BangEqual, //Signo de exclamación seguido de igual !=
    Plus, //Signo de suma +
    PlusEqual, //Suma seguida de igual +=
    PlusPlus, //Incremento ++
    Minus, //Signo de resta -
    MinusEqual, //Resta seguida de igual -=
    MinusMinus, //Decremento --
    Equal, //Signo de igual =
    EqualEqual, //Comparación de igualdad ==
    Arrow, //Flecha -> 
    Greater, //Mayor que >
    GreaterEqual, //Mayor o igual que >=
    Less, //Menor que <
    LessEqual, //Menor o igual que <=
    AtSymbolAtSymbol, //Arroba doble @@
    Or, //Operador lógico OR ||
    And, //Operador lógico AND &&

    //3. Literals
    Identifier, //Identificadores, que son nombres de variables, funciones, etc
    StringLiteral, //Literales de cadena, e.g., "example"
    NumberLiteral, //Literales numéricos, e.g., 42
    BoolLiteral, //Literales booleanos, true o false

    //4. Reserved Words
    Card, //Palabra clave para definir una carta
    effect, //Efecto de la carta

    //Effect:
    Name, //Nombre de la carta o efecto
    Params, //Parámetros del efecto
    Action, //Acción que realiza el efecto
    Targets, //Objetivos del efecto
    Context, //Contexto en el que se aplica el efecto
    
    //Loops:
    While, //Bucle while
    For, //Bucle for
    In, //Palabra clave usada en bucles for (por ejemplo, for x in ...)
    
    //Context Properties:
    TriggerPlayer, //Jugador que activa el efecto
    HandOfPlayer, //Mano del jugador
    FieldOfPlayer, //Campo del jugador
    GraveyardOfPlayer, //Cementerio del jugador
    DeckOfPlayer, //Mazo del jugador
    Hand, //Mano (diminutivo)
    Field, //Campo (diminutivo)
    Graveyard, //Cementerio (diminutivo)
    Deck, //Mazo (diminutivo)
    
    //Methods:
    Push, //Método para añadir elementos al tope de la lista
    SendBottom, //Método para enviar un elemento al fondo de la lista
    Pop, //Método para quitar un elemnto que esta al tope de la lista y devolverla
    Remove, //Método para eliminar un elemento
    Shuffle, //Método para mezclar
    Find, //Método para encontrar un elemento
    
    //Card:
    Type, //Tipo de carta.
    Faction, //Facción de la carta
    Power, //Poder de la carta
    Range, //Rango de la carta
    OnActivation, //Eventos que ocurren al activar la carta
    
    //OnActivation:
    Effect, //Efecto que ocurre al activar
    Selector, //Selector para elegir objetivos
    Source, //Fuente del efecto
    Single, //Selección de un único objetivo
    Predicate, //Condición para el efecto
    PostAction, //Acción a realizar después del efecto
    
    EOF //Indica el final del archivo
}

class Token 
{
    public TokenType type;
    public string lexeme;
    public object literal;
    public int line;
    Token(TokenType type, string lexeme, object literal, int line) 
    {
    this.type = type;
    this.lexeme = lexeme;
    this.literal = literal;
    this.line = line;
    }
    public override string ToString() 
    {
        string str = $"{type} {lexeme} {literal}";
        return str;
    }
}