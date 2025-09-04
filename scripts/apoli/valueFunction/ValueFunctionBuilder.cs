using System;
using System.Collections.Generic;
using Apoli.Types;

namespace Apoli.ValueFunctions;

public class ValueFunctionBuilder<FunctionType> : ApoliObjectBuilder<FunctionType> where FunctionType:ValueFunction
{
    public ValueFunctionBuilder<FunctionType> SetParam<T>(string Key, IValue<T> Value)
    {
        _SetParam<T>(Key, Value);
        return this;
    }
    public ValueFunctionBuilder<FunctionType> SetParam<T>(string Key, T Value)
    {
        _SetParam(Key, Value);
        return this;
    }
    public FunctionType Build()
    {
        return (FunctionType)_Build();
    }
}