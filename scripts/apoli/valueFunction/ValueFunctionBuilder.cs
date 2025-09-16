using System;
using System.Collections.Generic;
using Apoli.Types;

namespace Apoli.ValueFunctions;

public class ValueFunctionBuilder<FunctionType> : ApoliObjectBuilder<FunctionType> where FunctionType : ValueFunction
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
    public new FunctionType Build()
    {
        return (FunctionType)_Build();
    }
}

public static class ValueFunctionBuilderFactoryHelper
{
    public static ValueFunctionBuilder<T> NewBuilder<T>() where T:ValueFunction
    {
        return new();
    }
}