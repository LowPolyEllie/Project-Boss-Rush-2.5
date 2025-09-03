using System;
using System.Collections.Generic;
using System.Reflection;
using Apoli.Conditions;
using Godot;

namespace Apoli;

public class ApoliObjectBuilder
{
	public virtual Dictionary<Enum, Type> apoliIdMatch { get; set; } = [];
	protected Enum type;
	protected Type internalType;
	protected ParameterCollection _parameters = new();
	public ApoliObject _Build()
	{
		if (Convert.ToInt32(type) == 0) throw new Exception("ApoliObjectBuilder: No ApoliObject type specified");

		ApoliObject newObject;
		newObject = (ApoliObject)Activator.CreateInstance(internalType);
		newObject.parameters.AddFrom(_parameters);
		return newObject;
	}
	public ApoliObjectBuilder _SetParam(string Key, Types.Type Value)
	{
		GD.Print(Key);
		if (!_parameters.HasParam(Key))
		{
			throw new KeyNotFoundException("No keys matching \"" + Key + "\" found. use ApoliObjectBuilder.SetType() before setting values");
		}
		if (_parameters.GetType(Key) != Value.type)
		{
			throw new TypeLoadException("Wrong Apoli type: Expected " + _parameters.GetType(Key) + ", got " + Value.type);
		}
		_parameters.SetValue(Key, Value);
		return this;
	}
	public ApoliObjectBuilder _SetType(Enum _type)
	{
		type = _type;
		internalType = apoliIdMatch[type];
		_parameters = ApoliObject.GetParameterSet(internalType);
		return this;
	}
}
