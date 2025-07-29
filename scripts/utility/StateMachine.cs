using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;

namespace BossRush2;

public class State
{
    public delegate void _OnEnter(State self);
    public delegate void _OnLeave(State self);
    public _OnEnter OnEnter;
    public _OnLeave OnLeave;
    public string Name;
    public StateLayer Layer;
}

public class StateLayer
{
    private State _CurrentState;
    public string CurrentState
    {
        get
        {
            return _CurrentState.Name;
        }
        set
        {
            State newState = stateData.Find(val => val.Name == value);
            _CurrentState.OnLeave(_CurrentState);

            _CurrentState = newState;
        }
    }
    public float Name;
    public List<State> stateData;
    public StateLayer(List<State> _stateData, string initialState)
    {
        stateData = _stateData;
        CurrentState = initialState;
    }
}

public interface IStateMachine
{

}
public class StateMachine
{
    public List<StateLayer> States;
}
public class Demo
{
    protected StateMachine _StateMachine = new()
    {
        States = [
            new StateLayer(
                [
                    new(){
                        Name = "Idle",
                        OnEnter = delegate(State self){
                            GD.Print("Idling");
                        },
                        OnLeave = delegate(State self){
                            GD.Print("Doing something else idk");
                        }
                    },
                    new(){
                        Name = "Firing",
                        OnEnter = delegate(State self){
                            GD.Print("Firing");
                        },
                        OnLeave = delegate(State self){
                            GD.Print("Firing stopped");
                            self.Layer.CurrentState = "Idle";
                        }
                    }
                ],
                initialState : "Idle"
            )
        ]
    };
}