using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Lexer
{
    public string source;
    //Reserved words
    public static Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
    {
        {"Number", TokenType.Number},
        {"String", TokenType.String},
        {"Bool", TokenType.Bool},
        {"false", TokenType.False},
        {"true", TokenType.True},
        {"card", TokenType.Card},
        {"effect", TokenType.effect},
        //Effect:
        {"Name", TokenType.Name},
        {"Params", TokenType.Params},
        {"Action", TokenType.Action},
        //Loops:
        {"while", TokenType.While},
        {"for", TokenType.For},
        {"in", TokenType.In},
        //Context Properties:
        {"TriggerPlayer", TokenType.TriggerPlayer},
        {"HandOfPlayer", TokenType.HandOfPlayer},
        {"FieldOfPlayer", TokenType.FieldOfPlayer},
        {"GraveyardOfPlayer", TokenType.GraveyardOfPlayer},
        {"DeckOfPlayer", TokenType.DeckOfPlayer},
        {"Hand", TokenType.Hand},
        {"Field", TokenType.Field},
        {"Graveyard", TokenType.Graveyard},
        {"Deck", TokenType.Deck},
        //Methods:
        {"Push", TokenType.Push},
        {"SendBottom", TokenType.SendBottom},
        {"Pop", TokenType.Pop},
        {"Remove", TokenType.Remove},
        {"Shuffle", TokenType.Shuffle},
        {"Find", TokenType.Find},
        //Card:
        {"Type", TokenType.Type},
        {"Faction", TokenType.Faction},
        {"Power", TokenType.Power},
        {"Range", TokenType.Range},
        {"OnActivation", TokenType.OnActivation},
        //OnActivation:
        {"Effect", TokenType.Effect},
        {"Selector", TokenType.Selector},
        {"Source", TokenType.Source},
        {"Single", TokenType.Single},
        {"Predicate", TokenType.Predicate},
        {"PostAction", TokenType.PostAction},

    };
    public List<Token> tokens = new List<Token>();
    int start = 0;
    int current = 0;
    int line = 1;
    int column = 1;

    //Contructor del Lexer
    public Lexer(string source)
    {
        this.source = source;
    }

    //Lista de los Tokens
    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null, line, column));
        return tokens;
    }

    //Para saber cuando consumimos todos los caracteres
    public bool IsAtEnd()
    {
        return current >= source.Length;
    }

    //Reconociendo Lexemas
    void ScanToken()
    {
        char character = advance();
        switch (character)
        {
            case '(': AddToken(TokenType.LeftParen); break;
            case ')': AddToken(TokenType.RightParen); break;
            case '{': AddToken(TokenType.LeftBrace); break;
            case '}': AddToken(TokenType.RightBrace); break;
            case '[': AddToken(TokenType.LeftBracket); break;
            case ']': AddToken(TokenType.RightBracket); break;
            case ',': AddToken(TokenType.Comma); break;
            case '.': AddToken(TokenType.Dot); break;
            case ':': AddToken(TokenType.Colon); break;
            case ';': AddToken(TokenType.Semicolon); break;
            case '%': AddToken(TokenType.Mod); break;
            case '^': AddToken(TokenType.Pow); break;
            case '!':
                AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : Match('>') ? TokenType.Arrow : TokenType.Equal);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;
            case '+':
                AddToken(Match('=') ? TokenType.PlusEqual : Match('+') ? TokenType.PlusPlus : TokenType.Plus);
                break;
            case '-':
                AddToken(Match('=') ? TokenType.MinusEqual : Match('-') ? TokenType.MinusMinus : TokenType.Minus);
                break;
            case '*':
                AddToken(Match('=') ? TokenType.MulEqual : TokenType.Mul);
                break;
            case '@':
                AddToken(Match('=') ? TokenType.AtSymbolequal : Match('@') ? TokenType.AtSymbolAtSymbol : TokenType.AtSymbol);
                break;
            case '&':
                if (Match('&')) AddToken(TokenType.And);
                else DSL.Report(line, column, "", "Unexpected character: " + character);
                break;
            case '|':
                if (Match('|')) AddToken(TokenType.Or);
                else DSL.Report(line, column, "", "Unexpected character: " + character);
                break;
            case '/':
                if (Match('/'))
                {
                    // A comment goes until the end of the line.
                    while (peek() != '\n' && !IsAtEnd()) advance();
                }
                else
                {
                    AddToken(TokenType.Slash);
                }
                break;
            case ' ':
            case '\t':
            case '\r':
                //Ignore whitespace
                break;
            case '\n':
                line++;
                column = 1;
                break;
            case '"': String(); break;

            default:
                if (IsDigit(character)) Number();
                else if (IsAlpha(character)) Identifier();
                else DSL.Report(line, column, "", "Unexpected character: " + character);
                break;
        }

    }
    char advance()
    {
        current++;
        column++;
        return source[current - 1];
    }
    void AddToken(TokenType type)
    {
        AddToken(type, null);
    }
    void AddToken(TokenType type, object literal)
    {
        string text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal, line, column - text.Length));
    }
    bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;
        current++;
        return true;
    }
    //va recorreindo pero sin consumir los caracteres
    char peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }
    void String()
    {
        //Recorriendo hasta que se acabe el string con "
        while (peek() != '"' && !IsAtEnd() && peek() != '\n')
        {
            advance();
        }

        //Nunca alcanzo " por lo tanto no se termino el string y si \n aumenta la linea y reinicia la linea
        if (IsAtEnd() || peek() == '\n')
        {
            DSL.Report(line, column, "", "Unterminated string.");
            if (peek() == '\n')
            {
                line++;
                column = 1;
                advance();
            }
            return;
        }

        // alcanzo "
        advance();
        object value = source.Substring(start + 1, current - start - 2);
        AddToken(TokenType.StringLiteral, value);
    }

    //para saber si es un numero
    bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    void Number()
    {
        //recorreindo el numero 
        while (IsDigit(peek())) advance();
        AddToken(TokenType.NumberLiteral, int.Parse(source.Substring(start, current - start)));
    }

    bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }

    void Identifier()
    {
        while (IsAlphaNumeric(peek())) advance();
        string text = source.Substring(start, current - start);
        //Comprobar      
        if (keywords.ContainsKey(text)) AddToken(keywords[text]);
        else AddToken(TokenType.Identifier);
    }

    bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }


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
    Mul, // multiplicacion *
    MulEqual, // multiplicacion y asigancion *=
    SlashEqual, // division y asignacion /=
    Equal, //Signo de igual =
    EqualEqual, //Comparación de igualdad ==
    Arrow, //Flecha => 
    Greater, //Mayor que >
    GreaterEqual, //Mayor o igual que >=
    Less, //Menor que <
    LessEqual, //Menor o igual que <=
    AtSymbolAtSymbol, //Arroba doble @@
    AtSymbolequal, //concatenar @=
    Or, //Operador lógico OR ||
    And, //Operador lógico AND &&

    //3. Literals
    Identifier, //Identificadores, que son nombres de variables, funciones, etc
    StringLiteral, //Literales de cadena, e.g., "example"
    NumberLiteral, //Literales numéricos, e.g., 42
    True, // Palabra clave para detectar true
    False,// lo mismo para false

    //4. Reserved Words
    Card, //Palabra clave para definir una carta
    effect, //Efecto de la carta

    //Effect:
    Name, //Nombre de la carta o efecto
    Params, //Parámetros del efecto
    Action, //Acción que realiza el efecto
    
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
    Board, // Tablero

    //Methods:
    Push, //Método para añadir elementos al tope de la lista
    SendBottom, //Método para enviar un elemento al fondo de la lista
    Pop, //Método para quitar un elemnto que esta al tope de la lista y devolverla
    Remove, //Método para eliminar un elemento
    Shuffle, //Método para mezclar
    Find, //Método para encontrar un elemento

    //Card:
    Owner, // Jugador que posee la carta
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
    //Types
    Number, String, Bool,
    EOF //Indica el final del archivo
}

public class Token
{
    public TokenType type;
    public string lexeme;
    public object literal;
    public int line;
    public int column;
    public Token(TokenType type, string lexeme, object literal, int line, int column)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
        this.column = column;
    }
    public override string ToString()
    {
        string str = $"[Line: {line}, Column: {column}] {type} {lexeme}";
        return str;
    }
}


