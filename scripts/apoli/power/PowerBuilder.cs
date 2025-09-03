using System;
using System.Collections.Generic;

namespace Apoli.Powers;

public class PowerBuilder<PowerType>:ApoliObjectBuilder<PowerType> where PowerType:Power
{
    public Power Build()
    {
        return (Power)_Build();
    }
	public PowerBuilder<PowerType> SetParam(string Key, Types.Type Value)
	{
        _SetParam(Key, Value);
		return this;
	}
}