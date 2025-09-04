using Apoli.Types;

namespace Apoli;

public class Parameter
{
	public virtual object value { get; set; }
	public virtual System.Type type { get;}

}
public class Parameter<T> : Parameter
{
	public Type<T> _value;
	public override System.Type type { get; } = typeof(T);
    public new IValue<T> value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = (Type<T>)value;
		}
	}
	public Parameter() { }
	public static Parameter<T> FromValue(T __value)
	{
		return new()
		{
			value = new Type<T>(__value)
		};
	}
	public override string ToString()
	{
		return value.ToString() + "(" + typeof(T).Name + ")";
	}

}
