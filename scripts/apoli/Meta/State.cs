using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Godot;

namespace Apoli;


public class State
{
    public void OnEnter()
    {
        foreach (Powers.Power power in powers.FindAll(e => { return e.type == Powers.PowerId.ActionOnCallback && e.parameters.ContainsKey("ActionOnEnterState"); }))
        {
            Actions.Action actionOnEnterState = (Actions.Action)power.parameters["ActionOnEnterState"].value.value;
            actionOnEnterState.DoAction();
        }
    }
    public void OnLeave()
    {
        foreach (Powers.Power power in powers.FindAll(e => { return e.type == Powers.PowerId.ActionOnCallback && e.parameters.ContainsKey("ActionOnLeaveState"); }))
        {
            Actions.Action actionOnLeaveState = (Actions.Action)power.parameters["ActionOnLeaveState"].value.value;
            actionOnLeaveState.DoAction();
        }
    }
    public string name;
    public StateLayer layer;
    public List<Powers.Power> powers;
    public State(List<Powers.Power> _powers)
    {
        powers = _powers;   
    }
    public State(string _name, List<Powers.Power> _powers)
    {
        powers = _powers;   
        name = _name;   
    }
    public State(string _name)
    {
        name = _name;   
    }
}

public interface IStateMachine
{

}
public class StateMachine
{
    public List<StateLayer> stateLayers;
}
public class StateLayer
{
    private State _currentState;
    public string currentState
    {
        get
        {
            return _currentState.name;
        }
        set
        {
            State newState = states.Find(val => val.name == value);
            if (newState == null){return;}
            if (_currentState != null)
            {
                _currentState.OnLeave();
            }
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
    public float name;
    public List<State> states;
    public StateLayer(List<State> _states, string initialState)
    {
        states = _states;
        currentState = initialState;
    }
}