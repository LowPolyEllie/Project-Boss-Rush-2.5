using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Apoli.States;

public class StateLayer : ICollection<State>
{
    private State _currentState;
    private string initialState;
    public string currentState
    {
        get
        {
            return _currentState.name;
        }
        set
        {
            State newState = states.Find(val => val.name == value);
            if (newState == null) { return; }
            if (_currentState != null)
            {
                _currentState.OnLeave();
            }
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
    public float name;
    public StateMachine stateMachine;
    public List<State> states = new();
    public StateLayer(string _initialState)
    {
        initialState = _initialState;
    }
    public State GetState(string name)
    {
        return states.Find(state => state.name == name);
    }
    public void SetState(string name)
    {
        currentState = name;
    }
    public bool HasState(string name)
    {
        return states.Find(state => state.name == name) is not null;
    }
    public void Add(State state)
    {
        state.stateLayer = this;
        states.Add(state);
    }
    public bool Remove(State state)
    {
        return states.Remove(state);
    }
    public void Init()
    {
		foreach (State state in states)
		{
			state.Init();
		}

        currentState = initialState;
    }
    //The rest are just filler, can safely ignore vvv
    public int Count
    {
        get
        {
            return states.Count;
        }
    }
    public void Clear()
    {
        states.Clear();
    }
    public bool Contains(State state)
    {
        return states.Contains(state);
    }
    public void CopyTo(State[] state, int pos)
    {
        states.CopyTo(state, pos);
    }
    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }
    public IEnumerator<State> GetEnumerator()
    {
        return new StateLayerEnum(this);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return new StateLayerEnum(this);
    }
    public State this[string index]
    {
        get { return GetState(index); }
        set{ throw new System.Exception("Don't set this bruh, use an"); }
    }
    public State this[int index]
    {
        get { return states[index]; }
        set{ states[index] = value; }
    }
}
public class StateLayerEnum : IEnumerator<State>
{
    
    private StateLayer _collection;
    private int curIndex;
    private State curState;

    public StateLayerEnum(StateLayer collection)
    {
        _collection = collection;
        curIndex = -1;
        curState = default(State);
    }

    public bool MoveNext()
    {
        //Avoids going beyond the end of the collection.
        if (++curIndex >= _collection.Count)
        {
            return false;
        }
        else
        {
            // Set current State to next item in collection.
            curState = _collection[curIndex];
        }
        return true;
    }

    public void Reset() { curIndex = -1; }

    void IDisposable.Dispose() { }

    public State Current
    {
        get { return curState; }
    }

    object IEnumerator.Current
    {
        get { return Current; }
    }
}