using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Powers;
using Apoli.Types;
using BossRush2;
using Godot;

namespace Apoli.Conditions;

public enum ConditionId
{
	Controller
}
public class Condition {
	public virtual ConditionId type { get; set; }
    public virtual Dictionary<string, Parameter> parameters {get;set; }
	public virtual bool CheckCondition() { return false; }
}
public class ConditionBuilder
{
	private ConditionId type;
	public virtual Dictionary<string, Parameter> _parameters { get; set; }
	public Condition Build()
	{
		Condition newCondition;
		switch (type)
		{
			case ConditionId.Controller:
				newCondition = new Controller();
				break;
			default:
				throw new Exception("ConditionBuilder: No class specified");
		}
		newCondition.parameters = _parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
		return newCondition;
	}
	public ConditionBuilder SetParam(string Key, Types.Type Value)
	{
		if (!_parameters.ContainsKey(Key))
		{
			throw new KeyNotFoundException("No keys matching \"" + Key + "\" found. use PowerBuilder.SetType() before setting values");
		}
		if (_parameters[Key].type != Value.type)
		{
			throw new TypeLoadException("Wrong Apoli type: Expected " + _parameters[Key].type + ", got " + Value.type);
		}
		_parameters[Key].value.value = Value.value;
		_parameters = Parameter.conditionParameters[type];
		return this;
	}
	public ConditionBuilder SetType(ConditionId _type)
	{
		type = _type;
		return this;
	}
}