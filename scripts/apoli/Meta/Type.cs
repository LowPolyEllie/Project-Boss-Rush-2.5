using System.Collections;
using Apoli.ValueFunctions;
using Godot;

namespace Apoli.Types;

public class Type
{
    public virtual object value { get; set; }
    public virtual bool isGeneric { get; }
    public virtual bool isApoliObject { get; }
    public virtual bool isList { get; }
    public virtual System.Type type { get; }
    
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
    public override System.Type type { get; } = typeof(T);
    public Type(T _value)
    {
        value = _value;
    }
    public override bool isGeneric
    {
        get
        {
            return typeof(T).IsGenericType;
        }
    }
    public override bool isApoliObject
    {
        get
        {
            return typeof(T).IsAssignableTo(typeof(ApoliObject));
        }
    }
    public override bool isList
    {
        get
        {
            return typeof(T).IsAssignableTo(typeof(ICollection));
        }
    }
}