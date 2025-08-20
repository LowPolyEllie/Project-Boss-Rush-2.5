using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Apoli.Powers;
using Godot;

namespace Apoli.States;

public delegate void OnStateEnterEventHandler();
public delegate void OnStateLeaveEventHandler();
public delegate void OnPhysicsTickEventHandler();
public class State:ICollection<Power>
{
	public event OnStateEnterEventHandler StateEnterEvent;
	public event OnStateLeaveEventHandler StateLeaveEvent;
	public event OnPhysicsTickEventHandler PhysicsTickEvent;
	public void OnEnter()
	{
		StateEnterEvent?.Invoke();
	}
	public void OnLeave()
	{
		StateLeaveEvent?.Invoke();
	}
	public void OnPhysicsTick()
	{
		PhysicsTickEvent?.Invoke();
	}
	public string name;
	public StateLayer stateLayer;
	public List<Power> powers = new();
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
	public State()
	{
	}
	public void AddPower(Power power)
	{
		power.state = this;
		power.Init();
		powers.Add(power);
	}
	public void Add(Power state)
	{
		AddPower(state);
	}
	public bool Remove(Power state)
	{
		return powers.Remove(state);
	}

	//The rest are just filler, can safely ignore vvv
	public int Count
	{
		get
		{
			return powers.Count;
		}
	}
	public void Clear()
	{
		powers.Clear();
	}
	public bool Contains(Power state)
	{
		return powers.Contains(state);
	}
	public void CopyTo(Power[] state, int pos)
	{
		powers.CopyTo(state, pos);
	}
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}
	public IEnumerator<Power> GetEnumerator()
	{
		return new StateEnum(this);
	}
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new StateEnum(this);
	}
	public Power this[int index]
	{
		get { return powers[index]; }
		set{ powers[index] = value; }
	}
}
public class StateEnum : IEnumerator<Power>
{

	private State _collection;
	private int curIndex;
	private Power curPower;

	public StateEnum(State collection)
	{
		_collection = collection;
		curIndex = -1;
		curPower = default(Power);
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
			curPower = _collection[curIndex];
		}
		return true;
	}

	public void Reset() { curIndex = -1; }

	void IDisposable.Dispose() { }

	public Power Current
	{
		get { return curPower; }
	}

	object IEnumerator.Current
	{
		get { return Current; }
	}
}
