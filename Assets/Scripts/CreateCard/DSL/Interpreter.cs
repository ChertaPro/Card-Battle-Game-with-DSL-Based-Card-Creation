using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class Interpreter : IVisitor
{
    public Environment globals = new Environment();
    public Environment environment = new Environment();
    private Gamecontext gamecontext;
    
    private Dictionary<string, (Dictionary<string, TokenType>, Method)> effectFun;
    private object parentTargets;
    private Lists actualSource;
    private Lists TARGETS;
    public Interpreter()
    {
        environment = globals;
        parentTargets = null;
    }

    public Interpreter(Gamecontext _gamecontext)
    {
        gamecontext = _gamecontext;
        environment = globals;
        parentTargets = null;
    }
    public Dictionary<Card, OnActivation> CreateCards(List<GameEntity> gameEntities)
    {
        Dictionary<Card, OnActivation> cards = new Dictionary<Card, OnActivation>();
        foreach (var entity in gameEntities)
        {
            if(entity is CardClass)
            {
                Card card = (Card)Execute(entity);
                if (card != null)
            {
                cards[card] = ((CardClass)entity).onActivation;
            }
            }
            if(entity is EffectClass)
            {
                Card card = (Card)execute(entity);
            }
        }
        return cards;
    }
    private object Execute(GameEntity entity) => entity.Accept(this);

    private object Execute(Method method) => method.Accept(this);

    private object Execute(Prop prop) => prop.Accept(this);

    private object Execute(IStatement stmt) => stmt.Accept(this);

    public object VisitCardClass(CardClass cardClass)
    {
        object type = Execute(cardClass.type);
        object name = Execute(cardClass.name);
        object faction = Execute(cardClass.faction);
        object power = Execute(cardClass.power);
        object range = Execute(cardClass.range);
        if ((string)type == "Aumento" || (string)type == "Clima")
        {
            range = null;
        }
        OnActivation onActivation = cardClass.onActivation;
        OnActBody actbody = onActivation.onActBodies[0];
        Effect effect = actbody.effect;
        string effectname = "";
        if(effect.exprName != null)
        {
            effectname = (string)Evaluate(effect.exprName);
            return new Card(CardDatabase.COCDeck.Count+1,0,(string)name,(string)type,(string)faction,(int?)power,(List<string>)range,(string)effectname,Resources.Load<Sprite>("Designer (1)"),true,true);
        }
        else
        {
            effectname = (string)Execute(effect.name);
            Dictionary<string, object> _params = new Dictionary<string, object>();
            foreach(var _param in effect.paramsv)
            {
                string paramName;
                object paramValue;
                (paramName, paramValue) = ((string, object))Execute(_param);
                _params[paramName] = paramValue;
            }
            return new Card(CardDatabase.COCDeck.Count+1,0,(string)name,(string)type,(string)faction,(int?)power,(List<string>)range,(string)effectname,(int)_params["Amount"],Resources.Load<Sprite>("Designer (1)"),true,true);
        }
        
    }

    public object VisitOnActivationMethod(OnActivation onActivation)
    {
        foreach(var onAct in onActivation.onActBodies)
        {
            Execute(onAct);
        }
        return null;
    }

    public object VisitOnActBodyMethod(OnActBody onActBody)
    {
        object effect = Execute(onActBody.effect);
        Token effectToken;
        string nameEffect;
        Dictionary<string, object> paramsEffect = new Dictionary<string, object>();
        if(effect is ValueTuple<Token, string>)
        {
            (effectToken, nameEffect) = ((Token, string))effect;
        }
        else
        {
            (effectToken, nameEffect, paramsEffect) = ((Token, string, Dictionary<string, object>))effect;
        }
        object selector, postAction;
        if(onActBody.selector != null)
        {
            selector = Execute(onActBody.selector);
        }
        else
        {
            selector = parentTargets;
        }
        parentTargets = selector;
        if(onActBody.postAction != null)
        {
            postAction = Execute(onActBody.postAction);
        }
        return null;
    }
        
    public object VisitEffectMethod(Effect effect)
    {
        Dictionary<string, object> _params = new Dictionary<string, object>();
        if(effect.exprName != null)
        {
            return (effect.effect, (string)Evaluate(effect.exprName));
        }
        object name = Execute(effect.name);
        foreach(var _param in effect.paramsv)
        {
            string paramName;
            object paramValue;
            (paramName, paramValue) = ((string, object))Execute(_param);
            _params[paramName] = paramValue;
        }
        return (effect.effect, (string)name,  _params);
    }    

    public object VisitSelectorMethod(Selector selector)
    {
        object source = Execute(selector.source);
        if (source == null) return null;
        object single = Execute(selector.single);
        if (single == null) return null;
        actualSource = (Lists)source;
        object predicate = Execute(selector.predicate);
        actualSource = null;
        if (predicate == null) return null;
        if (((Lists)predicate).Cards.Count == 0)
        {
            return (Lists)predicate;
        }
        List<Card> list = ((Lists)predicate).Cards;
        if ((bool)single)
        {
            list = new List<Card> { ((Lists)predicate).Cards[0] };
        }
        return new Lists(list);
    }

    public object VisitNameProp(Name name)
    {
        object value = Evaluate(name.value);
        if (!(value is string))
        {
            DSL.error(name.name, "'Name' must be a string.");
            return null;
        }
        return value;
    }

    public object VisitTypeProp(Type type)
    {
        object value = Evaluate(type.value);
        if (!(value is string))
        {
            DSL.error(type.type, "'Type' must be a string.");
            return null;
        }
        return value;
    }

    public object VisitFactionProp(Faction faction)
    {
        object value = Evaluate(faction.value);
        if (!(value is string))
        {
            DSL.error(faction.faction, "'Faction' must be a string.");
            return null;
        }
        return value;
    }

    public object VisitPowerProp(Power power)
    {
        object value = Evaluate(power.value);
        if (!(value is int))
        {
            DSL.error(power.power, "'Power' must be an integer.");
            return null;
        }
        return value;
    }

    public object VisitRangeProp(Range range)
    {
        List<string> rangeStr = new List<string>();
        Dictionary<string, string> Pairs = new Dictionary<string, string>();
        Pairs["Melee"] = "Melee";
        Pairs["Ranged"] = "Ranged";
        Pairs["Siege"] = "Siege";
        foreach (var cardrange in range.list)
        {
            object value = Evaluate(cardrange);
            if (!(value is string))
            {
                DSL.error(range.range, "'Range' must be a set of strings.");
                return null;
            }
            if (!Pairs.ContainsKey((string)value))
            {
                DSL.error(range.range, "'Range' set can only have strings 'Melee', 'Ranged' or 'Siege'.");
                return null;
            }
            rangeStr.Add(Pairs[(string)value]);
            break;
        }
        return rangeStr;
    }

    public object VisitSourceProp(Source source)
    {
        object value = Evaluate(source.value);
        if (value == null)
        {
            DSL.error(source.source, "'Source' parameter can't be null.");
            return null;
        }
        if (!(value is string))
        {
            DSL.error(source.source, "'Source' parameter must contain a 'string'.");
            return null;
        }
        if ((string)value == "board") return gamecontext.Board;
        if ((string)value == "hand") return gamecontext.Hand;
        if ((string)value == "otherHand") return gamecontext.OtherHand;
        if ((string)value == "field") return gamecontext.Field;
        if ((string)value == "otherField") return gamecontext.OtherField;
        if ((string)value == "deck") return gamecontext.Deck;
        if ((string)value == "otherDeck") return gamecontext.OtherDeck;
        if ((string)value == "parent")
        {
            if (parentTargets == null)
            {
                DSL.error(source.source, "Unknown 'parent' targets");
                return null;
            }
            return parentTargets;
        }
        DSL.error(source.source, "The value must be 'board', 'hand', 'otherHand', 'field', 'otherField', 'deck', 'otherDeck' or 'parent'");
        return null;
    }

    public object VisitSingleProp(Single single)
    {
        object value = Evaluate(single.value);
        if (value == null)
        {
            DSL.error(single.single, "'Single' parameter can't be null.");
            return null;
        }
        return IsTruthy(value);
    }

    public object VisitPredicateProp(Predicate predicate)
    {
        List<Card> cards = new List<Card>();
        foreach (var card in actualSource.Cards)
        {
            Environment previous = environment;
            environment.Define(predicate.card.lexeme, card);
            bool ok = IsTruthy(Evaluate(predicate.condition));
            if (ok)
            {
                cards.Add(card);
            }
            environment = previous;
        }
        return new Lists(cards);
    }
    public object VisitParamValueProp(ParamValue paramValue)
    {
        return (paramValue.name.lexeme, Evaluate(paramValue.value));
    }
    public object VisitEffectClass(EffectClass effectClass)
    {
        object name = Execute(effectClass.name);
        object _params = new Dictionary<string, TokenType>();
        if (effectClass.parameters != null)
        {
            _params = Execute(effectClass.parameters);
        }
        effectFun[(string)name] = ((Dictionary<string, TokenType>, Method))(_params, effectClass.action);
        return null;
    }

    public object VisitParamsMethod(Params parameters)
    {
        Dictionary<string, TokenType> paramsDict = new Dictionary<string, TokenType>();
        foreach (var param in parameters.paramslist)
        {
            string name;
            TokenType value;
            (name, value) = ((string, TokenType))Execute(param);
            paramsDict[name] = value;
        }
        return paramsDict;
    }

    public object VisitParamDeclarationProp(ParamDeclaration prop)
    {
        return (prop.name.lexeme, prop.value.type);
    }

    public object VisitActionMethod(Action action)
    {
        environment.Define(action.targets.lexeme, TARGETS);
        environment.Define(action.context.lexeme, null);
        foreach(var stmt in action.statements)
        {
            Execute(stmt);
        }
        return null;
    }

    public object VisitLiteral(Literal expr)
    {
        return expr.value;
    }

    public object VisitGrouping(Grouping expr)
    {
        return Evaluate(expr.expression);
    }

    public object VisitUnary(Unary expr)
    {
        object right = Evaluate(expr.right);
        switch (expr.unaryoperator.type)
        {
            case TokenType.Bang:
                return !IsTruthy(right);
            case TokenType.Minus:
            CheckNumberOperand(expr.unaryoperator,right);
                return -(int)right;
        }
        return null;
    }

    void CheckNumberOperand(Token unaryoperator, object operand)
    {
        if (operand is int) return;
        throw new RuntimeError(unaryoperator,"Operand must be a number.");
    }
    private object execute(GameEntity effect)
    {
        return null;
    }
    public object VisitBinary(Binary expr)
    {
        object left = Evaluate(expr.left);   
        object right = Evaluate(expr.right);
        if (left == null || right == null)
        {
            DSL.error(expr.binaryoperator, "Operands can't be null.");
            return null;
        }
        
        switch (expr.binaryoperator.type)
        {
            //Comparison
            case TokenType.And:
                return IsTruthy(left) && IsTruthy(right);
            case TokenType.Or:
                return IsTruthy(left) || IsTruthy(right);
            case TokenType.Greater:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left > (int)right;

            case TokenType.GreaterEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left >= (int)right;

            case TokenType.Less:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left < (int)right;

            case TokenType.LessEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left <= (int)right;

            case TokenType.EqualEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return IsEqual(left, right);

            case TokenType.BangEqual:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return !IsEqual(left, right);

            //Operations
            case TokenType.Minus:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left - (int)right;

            case TokenType.Plus:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left + (int)right;

            case TokenType.Slash:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left / (int)right;

            case TokenType.Mul:
                CheckNumberOperands(expr.binaryoperator, left, right);
                return (int)left * (int)right;

            case TokenType.Pow:
            CheckNumberOperands(expr.binaryoperator, left, right);
                return Math.Pow((int)left, (int)right); 

            case TokenType.AtSymbol: return left.ToString() + right.ToString();
            case TokenType.AtSymbolAtSymbol: return left.ToString() + " " + right.ToString();
        }

        return null;
    }

    public object VisitCallExpr(Call expr)
    {
        object callee = Evaluate(expr.callee);
        List<object> arguments = new List<object>();    

        foreach (IExpression argument in expr.arguments)
        {
            arguments.Add(Evaluate(argument));
        }
        if (!(callee is ICallable))
        {
            throw new RuntimeError(expr.paren, "Can only call functions.");
        }
        ICallable function = (ICallable)callee;
        if (arguments.Count != function.Arity())
        {
            throw new RuntimeError(expr.paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
        }
        return function.call(this,arguments); 
    }

    public object VisitAccessExpr(Access expr) => null;
    public object VisitSetExpr(Set expr) => null;
    public object VisitPostoperation(Postoperation expr) => null;
    public object VisitPreoperation(Preoperation expr) => null;
    

    void CheckNumberOperands(Token binaryoperator, object left, object right)
    {
        if (left is int && right is int) return;
        throw new RuntimeError(binaryoperator, "Operands must be numbers.");

    }

    public object VisitVariableExpr(Variable expr)
    {
        return environment.Get(expr.name);
    }

    
    private bool IsTruthy(object obj)
    {
        if (obj == null) return false;
        if (obj is bool) return (bool)obj;
        if (obj is int) return (int)obj != 0;
        if (obj is string) return (string)obj != "";
        return true;
    }
    
    private bool IsEqual(object left, object right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        return left.Equals(right);
    }
    private object Evaluate(IExpression expr)
    {
        return expr.Accept(this);
    }


    public void executeBlock(List<IStatement> statements, Environment environment)
    {
        Environment previous = this.environment;
        this.environment = environment;
        foreach (IStatement stmt in statements)
        {
            Execute(stmt);
        }
        this.environment = previous;
    }

    public object VisitBlockStmt(Block stmt)
    {
        executeBlock(stmt.statements, new Environment(environment));
        return null;
    }
    public object VisitExpressionStmt(Expression stmt)
    {
        Evaluate(stmt.expression);
        return null;
    }

    public object VisitVarStmt(Var stmt)
    {
        object value = null;
        if (stmt.initializer == null)
        {
            environment.Define(stmt.name.lexeme, value);
            return value;
        }
        TokenType type = OperConverter(stmt.type.type);
        Token token = stmt.type;
        token.type = type;
        if (type == TokenType.Equal) value = Evaluate(stmt.initializer);
        else if (type != stmt.type.type) value = null;
        else value = Evaluate(new Binary(new Variable(stmt.name), token, stmt.initializer));
        environment.Define(stmt.name.lexeme, value);
        return value;
    }

    public object VisitWhileStmt(While stmt)
    {
        while(IsTruthy(Evaluate(stmt.condition)))
        {
            Execute(stmt.body);
        }
        return null;
    }

    public object VisitForStmt(For stmt)
    {
        object list = Evaluate(stmt.list);
        if (!(list is Lists))
        {
            DSL.error(stmt.iter, "Expected list for this iterator.");
            return null;
        }
        foreach (var item in ((Lists)list).Cards)
        {
            Environment previous = environment;
            environment.Define(stmt.iter.lexeme, item);
            Execute(stmt.body);
            environment = previous;
        }
        return null;
    }

    private TokenType OperConverter(TokenType tokenType)
    {
        switch (tokenType)
        {
            case TokenType.AtSymbolequal:
                return TokenType.AtSymbol;
            case TokenType.MulEqual:
                return TokenType.Mul;
            case TokenType.PlusEqual:
                return TokenType.Plus;
            case TokenType.MinusEqual:
                return TokenType.Minus;
            case TokenType.SlashEqual:
                return TokenType.Slash;
            case TokenType.Equal:
                return TokenType.Equal;
        }
        return tokenType;
    }
}
