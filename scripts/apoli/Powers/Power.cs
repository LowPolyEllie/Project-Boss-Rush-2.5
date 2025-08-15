using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Types;
using Apoli.Actions;
using Godot;
using System;

namespace Apoli.Powers;

public enum PowerId
{
    ActionOnCallback,
    ActionOnInput,
    Variable
}
public class Power
{
    public State state;
    public virtual PowerId type { get; set; }
    public virtual ParameterCollection parameters {get;set; }
}
public class PowerBuilder
{
    private PowerId type;
    private ParameterCollection _parameters = new();
    public Power Build()
    {
        Power newPower;
        switch (type)
        {
            case PowerId.ActionOnCallback:
                newPower = new ActionOnCallback();
                break;
            case PowerId.ActionOnInput:
                newPower = new ActionOnInput();
                break;
            default:
                throw new Exception("PowerBuilder: No class specified");
        }
        newPower.parameters = _parameters;
        return newPower;
    }
    public PowerBuilder SetParam(string Key, Types.Type Value)
    {
        if (!_parameters.HasParam(Key))
        {
            throw new KeyNotFoundException("No keys matching \"" + Key + "\" found. use PowerBuilder.SetType() before setting values");
        }
        if (_parameters.GetType(Key) != Value.type)
        {
            throw new TypeLoadException("Wrong Apoli type: Expected "+_parameters.GetType(Key)+", got "+Value.type);
        }
        _parameters.SetParam(Key,Value);
        return this;
    }
    public PowerBuilder SetType(PowerId _type)
    {
        type = _type;
        _parameters = Parameter.powerParameters[type];
        return this;
    }
}public class ActionOnCallback : Power
{
    public override PowerId type { get; set; } = PowerId.ActionOnCallback;
}
public class ActionOnInput : Power
{
    public override PowerId type { get; set; } = PowerId.ActionOnInput;
}
public class Variable : Power
{
    public override PowerId type { get; set; } = PowerId.Variable;
}