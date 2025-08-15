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
    public virtual Dictionary<string, Parameter> parameters {get;set; }
}
public class PowerBuilder
{
    private PowerId type;
    private Dictionary<string, Parameter> _parameters = new();
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
        newPower.parameters = _parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
        return newPower;
    }
    public PowerBuilder SetParam(string Key, Types.Type Value)
    {
        if (!_parameters.ContainsKey(Key))
        {
            throw new KeyNotFoundException("No keys matching \"" + Key + "\" found. use PowerBuilder.SetType() before setting values");
        }
        if (_parameters[Key].type != Value.type)
        {
            throw new TypeLoadException("Wrong Apoli type: Expected "+_parameters[Key].type+", got "+Value.type);
        }
        _parameters[Key].value.value = Value.value;
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