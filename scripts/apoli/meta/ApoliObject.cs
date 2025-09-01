using System;
using System.Reflection;
using Godot;

namespace Apoli;

public class ApoliObject
{
	public ParameterCollection parameters { get; set; } = [];
	public static ParameterCollection parameterSet = [];
	public static ParameterCollection GetParameterSet(Type type)
	{
		ParameterCollection paramSet = new();

		while (!type.IsEquivalentTo(typeof(ApoliObject)))
		{
			ParameterCollection baseSet = (ParameterCollection)type.GetField("parameterSet", BindingFlags.Public | BindingFlags.Static)?.GetValue(null);
			if (baseSet is not null)
			{
				paramSet.AddFrom(baseSet);
			}
			type = type.BaseType;
		}
		return paramSet;
	}
	public virtual void Init()
	{
		foreach (Parameter parameter in parameters)
		{
			if (parameter.value.value is ApoliObject obj)
			{
				obj.Init();
			}
		}
	}
}
