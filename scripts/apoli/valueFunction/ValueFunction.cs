using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Powers;
using Apoli.Types;
using BossRush2;
using Godot;

namespace Apoli.ValueFunctions;

public enum ValueFunctionId
{
	
}
public class ValueFunction : ApoliObject
{
	public Power power;
	public virtual ValueFunctionId type { get; set; }
	public virtual object ReturnValue(Node subject) { return null; }
	public new static ParameterCollection parameterSet = new();
}
public class ValueFunction<T> : ValueFunction
{
	public override object ReturnValue(Node subject)
	{
		return _ReturnValue(subject);
	}
	public virtual T _ReturnValue(Node subject)
	{
		return default;
	}
}