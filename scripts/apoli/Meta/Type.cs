using System.Collections.Generic;
using System.Linq;
using Apoli.Actions;
using Apoli.Conditions;
using Apoli.Powers;
using Apoli.ValueFunctions;
using BossRush2;
using Godot;

namespace Apoli.Types;

public enum TypeId
{
    Default,
    String,
    Float,
    Int,
    Bool,
    Variant,
    TypeId,

    Power,
    PowerCollection,
    Action,
    ActionCollection,
    Condition,
    ConditionCollection,

    ValueFunction
}
public class Type
{
    public static Dictionary<TypeId, System.Type> typeIdMatch = new()
    {
        {TypeId.String,typeof(string)},
        {TypeId.Float,typeof(float)},
        {TypeId.Int,typeof(int)},
        {TypeId.Bool,typeof(bool)},
        {TypeId.Variant,typeof(Variant)},
        {TypeId.TypeId,typeof(TypeId)},

        {TypeId.Power,typeof(Power)},
        {TypeId.Action,typeof(Action)},
        {TypeId.Condition,typeof(Condition)},

        {TypeId.ValueFunction,typeof(ValueFunction)}
    };
    protected static TypeId[] genericTypes = [
        TypeId.Int,
        TypeId.String,
        TypeId.Float,
        TypeId.Bool
    ];
    protected static TypeId[] apoliObjectTypes = [
        TypeId.Power,
        TypeId.Action,
        TypeId.Condition,
        TypeId.ValueFunction
    ];
    protected static TypeId[] collectionTypes = [
        TypeId.PowerCollection,
        TypeId.ActionCollection,
        TypeId.ConditionCollection
    ];
    public virtual TypeId type { get; set; }
    public virtual object value { get; set; }
    public virtual bool isGeneric { get; }
    public virtual bool isApoliObject { get; }
    public virtual bool isCollection { get; }
    
    public static Type FromValue<T>(T value)
    {
        return new Type<T>(value);
    }

    public ValueFunction valueFunction;
    public object GetValue(Node subject)
    {
        if (valueFunction is null)
        {
            return value;
        }
        return valueFunction.ReturnValue(subject);
    }
}
public class Type<T> : Type
{
    public string id;
    public virtual T _value { get; set; }
    public override object value { get => _value; set => _value = (T)value; }
    public Type(T _value)
    {
        value = _value;
        type = typeIdMatch.GetFirstKey(typeof(T));
    }
    public override bool isGeneric
    {
        get
        {
            return genericTypes.Contains(type);
        }
    }
    public override bool isApoliObject
    {
        get
        {
            return apoliObjectTypes.Contains(type);
        }
    }
    public override bool isCollection
    {
        get
        {
            return collectionTypes.Contains(type);
        }
    }
}