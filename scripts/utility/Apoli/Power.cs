using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Types;
using Godot;

namespace Apoli.Powers;

public enum PowerId
{
    action_on_callback,
    action_on_key_press
}
public abstract class Power
{
    public State state;
    public abstract PowerId type{ get; set; }
    public Dictionary<string, Types.Type> parameters;
}
public class PowerBuilder
{
    private PowerId type;
    private Dictionary<string, Types.Type> _parameters;
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
    public PowerBuilder SetParam(string Key, Types.Type Value)
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
    public override PowerId type { get { return PowerId.action_on_callback; } set { } }
}
public class ActionOnKeyPress : Power
{
    public override PowerId type { get { return PowerId.action_on_key_press; } set { } }
}
public class Demo
{
    protected StateMachine _StateMachine = new()
    {
        states = [
            new StateLayer(
                [
                    new State("Idle",[
                        new PowerBuilder()
                        .SetType(PowerId.action_on_callback)
                        .SetParam("ActionOnEnterState",new Action(
                            new Actions.ActionBuilder()
                            .SetType(Actions.ActionId.print)
                            .SetParam("Message",new String("Meow"))
                            .Build())
                        )
                        .Build()
                    ]),
                    new State("firing")
                ],
                initialState : "Idle"
            )
        ]
    };
}