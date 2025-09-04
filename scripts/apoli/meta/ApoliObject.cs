using System;
using System.Reflection;
using Apoli.Types;

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
			if (((IValue)parameter.value).GetUndefinedValue().GetType().IsAssignableTo(typeof(ApoliObject)))
			{
				((ApoliObject)((IValue)parameter.value).GetUndefinedValue())?.Init();
			}
		}
	}
}
