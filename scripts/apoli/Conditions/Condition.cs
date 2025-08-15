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
    public virtual ParameterCollection parameters {get;set; }
	public virtual bool CheckCondition() { return false; }
}
public class ConditionBuilder
{
	private ConditionId type;
	public virtual ParameterCollection _parameters { get; set; }
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
		newCondition.parameters = _parameters;
		return newCondition;
	}
	public ConditionBuilder SetParam(string Key, Types.Type Value)
	{
		if (!_parameters.HasParam(Key))
		{
			throw new KeyNotFoundException("No keys matching \"" + Key + "\" found. use PowerBuilder.SetType() before setting values");
		}
		if (_parameters.GetType(Key) != Value.type)
		{
			throw new TypeLoadException("Wrong Apoli type: Expected " + _parameters.GetType(Key) + ", got " + Value.type);
		}
		_parameters.SetParam(Key,Value);
		_parameters = Parameter.conditionParameters[type];
		return this;
	}
	public ConditionBuilder SetType(ConditionId _type)
	{
		type = _type;
		return this;
	}
}