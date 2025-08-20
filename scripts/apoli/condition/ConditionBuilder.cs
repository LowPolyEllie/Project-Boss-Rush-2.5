using System;
using System.Collections.Generic;

namespace Apoli.Conditions;

public class ConditionBuilder:ApoliObjectBuilder
{
    public override Dictionary<Enum, Type> apoliIdMatch { get; set; } = new()
    { 
        {ConditionId.Controller,typeof(Controller)}
    };
    public Condition Build()
    {
        return (Condition)_Build();
    }
	public ConditionBuilder SetParam(string Key, Types.Type Value)
	{
        _SetParam(Key, Value);
		return this;
	}
	public ConditionBuilder SetType(ConditionId _type)
	{
        _SetType(_type);
		return this;
	}
}