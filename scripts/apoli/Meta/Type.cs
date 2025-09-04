using System.Collections;
using Apoli.ValueFunctions;
using Godot;

namespace Apoli.Types;

public interface IValue
{
    public object GetUndefinedValue();
}
public interface IValue<T>:IValue
{
    public T GetValue();
}
public class Type<T> : IValue<T>
{
    public string id;
    public System.Type type { get; } = typeof(T);
    public T value { get; set; }

    public object GetUndefinedValue() { return GetValue(); }
    public T GetValue()
    {
        return value;
    }
    public static Type<T> FromValue(T value)
    {
        return new Type<T>(value);
    }
    public Type(T _value)
    {
        value = _value;
    }

    public bool isGeneric
    {
        get
        {
            return typeof(T).IsGenericType;
        }
    }
    public bool isApoliObject
    {
        get
        {
            return typeof(T).IsAssignableTo(typeof(ApoliObject));
        }
    }
    public bool isList
    {
        get
        {
            return typeof(T).IsAssignableTo(typeof(ICollection));
        }
    }
}