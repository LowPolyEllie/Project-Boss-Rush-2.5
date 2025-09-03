using System;
using System.Collections.Generic;

namespace Apoli.Actions;

public class ActionBuilder : ApoliObjectBuilder
{
	public override Dictionary<Enum, Type> apoliIdMatch { get; set; } = new()
	{
		{ActionId.Print,typeof(Print)}
	};
	public Action Build()
	{
		return (Action)_Build();
	}
	public ActionBuilder SetParam(string Key, Types.Type Value)
	{
		_SetParam(Key, Value);
		return this;
	}
	public ActionBuilder SetType(ActionId _type)
	{
		_SetType(_type);
		return this;
	}
}
