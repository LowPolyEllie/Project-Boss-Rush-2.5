using System.Reflection.Metadata;
using Apoli.Types;

namespace Apoli;

public class Parameter
{
	public virtual Type value { get; set; }
	public TypeId type { get; set; }
}
public class Parameter<T> : Parameter //: ICloneable
{
	public Type<T> _value;
    public override Type value
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
	public Parameter(TypeId _type, Type<T> __value)
	{
		value = __value;
		type = _type;
	}
	public Parameter(TypeId _type)
	{
		type = _type;
	}
	public Parameter(TypeId _type, Type __value)
	{
		type = _type;
		value = __value;
	}
	public Parameter() { }
	public static Parameter<T> FromValue(T __value)
	{
		return new()
		{
			type = Type.typeIdMatch.GetFirstKey(typeof(T)),
			value = new Type<T>(__value)
		};
	}
	/*public object Clone()
	{
		return new Parameter(type,value);
	}*/
	public override string ToString()
	{
		return value.ToString() + "(" + type.ToString() + ")";
	}

}
