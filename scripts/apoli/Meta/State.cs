using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Apoli.Powers;
using Godot;

namespace Apoli;

public delegate void OnStateEnterEventHandler();
public delegate void OnStateLeaveEventHandler();
public delegate void OnPhysicsTickEventHandler();
public class State
{
    public event OnStateEnterEventHandler StateEnterEvent;
    public event OnStateLeaveEventHandler StateLeaveEvent;
    public event OnPhysicsTickEventHandler PhysicsTickEvent;
    public void OnEnter()
    {
        StateEnterEvent();
    }
    public void OnLeave()
    {
        StateLeaveEvent();
    }
    public void OnTick()
    {
        PhysicsTickEvent();
    }
    public string name;
    public StateLayer layer;
    public List<Power> powers;
    public State(List<Power> _powers)
    {
        powers = _powers;
    }
    public State(string _name, List<Power> _powers)
    {
        powers = _powers;
        name = _name;
    }
    public State(string _name)
    {
        name = _name;
    }
    public void AddPower(Power power)
    {
        powers.Add(power);
        power.state = this;
    }
}
public class StateMachine
{
    public Dictionary<string, StateLayer> stateLayers;
    public void AddLayer(string name, StateLayer layer) {
        stateLayers.Add(name, layer);
    }
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