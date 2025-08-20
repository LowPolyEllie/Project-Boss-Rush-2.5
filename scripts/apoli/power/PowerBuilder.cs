using System;
using System.Collections.Generic;

namespace Apoli.Powers;

public class PowerBuilder:ApoliObjectBuilder
{
    public override Dictionary<Enum, Type> apoliIdMatch { get; set; } = new()
    { 
        {PowerId.ActionOnCallback,typeof(ActionOnCallback)},
        {PowerId.ActionOnInput,typeof(ActionOnInput)},
        {PowerId.ActionOnPhysicsTick,typeof(PhysicsTickPower)},
        {PowerId.Variable,typeof(Variable)}
    };
    public Power Build()
    {
        return (Power)_Build();
    }
	public PowerBuilder SetParam(string Key, Types.Type Value)
	{
        _SetParam(Key, Value);
		return this;
	}
	public PowerBuilder SetType(PowerId _type)
	{
        _SetType(_type);
		return this;
	}
}