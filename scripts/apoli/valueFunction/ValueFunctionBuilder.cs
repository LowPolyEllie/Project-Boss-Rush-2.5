using System;
using System.Collections.Generic;

namespace Apoli.ValueFunctions;

public class ValueFunctionBuilder<FunctionType> : ApoliObjectBuilder<FunctionType>
{
    public ValueFunctionBuilder<FunctionType> SetParam(string Key, Types.Type Value)
    {
        _SetParam(Key, Value);
        return this;
    }
    public ValueFunction Build()
    {
        return (ValueFunction)_Build();
    }
}