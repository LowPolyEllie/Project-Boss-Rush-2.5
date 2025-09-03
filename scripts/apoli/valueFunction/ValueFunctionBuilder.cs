using System;
using System.Collections.Generic;

namespace Apoli.ValueFunctions;

public class ValueFunctionBuilder : ApoliObjectBuilder
{
    public override Dictionary<Enum, Type> apoliIdMatch { get; set; } = new()
    {

    };
    public virtual ValueFunction Build()
    {
        return (ValueFunction)_Build();
    }
    public ValueFunctionBuilder SetParam(string Key, Types.Type Value)
    {
        _SetParam(Key, Value);
        return this;
    }
    public ValueFunctionBuilder SetType(ValueFunctionId _type)
    {
        _SetType(_type);
        return this;
    }
}
public class ValueFunctionBuilder<T> : ValueFunctionBuilder
{
    public override ValueFunction Build()
    {
		if (Convert.ToInt32(type) == 0) throw new Exception("ApoliObjectBuilder: No ApoliObject type specified");

        ValueFunction<T> newObject = new ValueFunction<T>();
		newObject.parameters.AddFrom(_parameters);
		return newObject;
    }
}