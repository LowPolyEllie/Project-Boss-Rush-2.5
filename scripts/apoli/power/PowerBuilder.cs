using System;
using System.Collections.Generic;
using Apoli.Types;

namespace Apoli.Powers;

public class PowerBuilder<PowerType>:ApoliObjectBuilder<PowerType> where PowerType:Power
{
    public PowerBuilder<PowerType> SetParam<T>(string Key, IValue<T> Value)
    {
        _SetParam<T>(Key, Value);
        return this;
    }
    public PowerBuilder<PowerType> SetParam<T>(string Key, T Value)
    {
        _SetParam(Key, Value);
        return this;
    }
    public Power Build()
    {
        return (Power)_Build();
    }
}