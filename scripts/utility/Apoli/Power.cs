using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Types;
using Apoli.Actions;
using Godot;

namespace Apoli.Powers;

public enum PowerId
{
    action_on_callback,
    action_on_key_press
}
public class Power
{
    public State state;
    public virtual PowerId type{ get; set; }
    public Dictionary<string, Type> parameters;
}
public class PowerBuilder
{
    private PowerId type;
    private Dictionary<string, Type> _parameters = new();
    public Power Build()
    {
        Power newPower;
        switch (type)
        {
            case PowerId.action_on_callback:
                newPower = new ActionOnCallback();
                break;
            default:
                throw new System.Exception("PowerBuilder: No class specified");
        }
        newPower.parameters = _parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
        return newPower;
    }
    public PowerBuilder SetParam(string Key, Type Value)
    {
        _parameters.Add(Key, Value);
        return this;
    }
    public PowerBuilder SetType(PowerId _type)
    {
        type = _type;
        return this;
    }
}public class ActionOnCallback : Power
{
    public override PowerId type { get; set; } = PowerId.action_on_callback;
}
public class ActionOnKeyPress : Power
{
    public override PowerId type { get; set; } = PowerId.action_on_key_press;
}