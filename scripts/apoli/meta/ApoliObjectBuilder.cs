using Apoli.Types;
using Godot;

namespace Apoli;

public class ApoliObjectBuilder<ObjectType>
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
		return _SetParam<T>(Key,Type<T>.FromValue(Value));
	}
	public ApoliObjectBuilder()
	{
		_parameters = ApoliObject.GetParameterSet(typeof(ObjectType));
	}
}
