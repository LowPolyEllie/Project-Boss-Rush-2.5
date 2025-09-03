using System;
using System.Collections.Generic;
using Godot;

namespace Apoli;

public class ApoliObjectBuilder<Type>
{
	protected ParameterCollection _parameters = new();
	public ApoliObject _Build()
	{
		ApoliObject newObject;
		newObject = (ApoliObject)Activator.CreateInstance(typeof(Type));
		newObject.parameters.AddFrom(_parameters);
		return newObject;
	}
	public ApoliObjectBuilder<Type> _SetParam(string Key, Types.Type Value)
	{
		if (!_parameters.HasParam(Key))
		{
			throw new TypeUnsetException("No keys matching \"" + Key + "\" found. use ApoliObjectBuilder.SetType() before setting values");
		}
		if (!_parameters.GetType(Key).IsAssignableFrom(Value.type))
		{
			throw new InvalidParameterException("Wrong Apoli type: Expected " + _parameters.GetType(Key) + ", got " + Value.type);
		}
		_parameters.SetValue(Key, Value);
		return this;
	}
	public ApoliObjectBuilder(){
		_parameters = ApoliObject.GetParameterSet(typeof(Type));
	}
	/*public ApoliObjectBuilder _SetType(Enum _type)
	{
		type = _type;
		internalType = apoliIdMatch[type];
		_parameters = ApoliObject.GetParameterSet(internalType);
		return this;
	}*/
}
