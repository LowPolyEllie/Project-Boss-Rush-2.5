using Apoli.Actions;
using Apoli.Powers;
using Apoli.Types;
using Apoli.ValueFunctions;
using Godot;

namespace Apoli;

public class ApoliObjectBuilder {
	public virtual ApoliObjectBuilder SetParam(){ return null; }
	public virtual ApoliObject Build(){ return null; }
}
public class ApoliObjectBuilder<ObjectType> : ApoliObjectBuilder
{
	protected ParameterCollection _parameters = new();
	public ApoliObject _Build()
	{
		ApoliObject newObject;
		newObject = (ApoliObject)System.Activator.CreateInstance(typeof(ObjectType));
		newObject.parameters.AddFrom(_parameters);
		return newObject;
	}
	public ApoliObjectBuilder<ObjectType> _SetParam<T>(string Key, IValue<T> Value)
	{
		GD.Print(Key);
		GD.Print(typeof(T));
		if (!_parameters.HasParam(Key))
		{
			throw new TypeUnsetException("No keys matching \"" + Key + "\" found. use ApoliObjectBuilder.SetType() before setting values");
		}
		if (!_parameters.GetType(Key).IsAssignableFrom(typeof(T)))
		{
			throw new InvalidParameterException("Wrong Apoli type: Expected " + _parameters.GetType(Key).ToString() + ", got " + typeof(T).ToString());
		}
		_parameters.SetValue(Key, Value);
		return this;
	}
	public ApoliObjectBuilder<ObjectType> _SetParam<T>(string Key, T Value)
	{
		return _SetParam<T>(Key, Type<T>.FromValue(Value));
	}
	public ApoliObjectBuilder()
	{
		_parameters = ApoliObject.GetParameterSet(typeof(ObjectType));
	}
}

public static class ApoliObjectBuilderFactory
{
	public static ApoliObjectBuilder FromType(System.Type type)
	{
		ApoliObjectBuilder builder = null;
		if (type.IsAssignableTo(typeof(Power)))
		{
			builder = (ApoliObjectBuilder)typeof(PowerBuilderFactoryHelper).GetMethod("NewBuilder").MakeGenericMethod([type]).Invoke(null, []);
		}
		if (type.IsAssignableTo(typeof(Action)))
		{
			builder = (ApoliObjectBuilder)typeof(ActionBuilderFactoryHelper).GetMethod("NewBuilder").MakeGenericMethod([type]).Invoke(null, []);
		}
		if (type.IsAssignableTo(typeof(ValueFunction)))
		{
			builder = (ApoliObjectBuilder)typeof(ValueFunctionBuilderFactoryHelper).GetMethod("NewBuilder").MakeGenericMethod([type]).Invoke(null, []);
		}
		return builder;
	}
}