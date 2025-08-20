using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Types;
using Apoli.Actions;
using Godot;
using System;
using Apoli.States;

namespace Apoli.Powers;

public enum PowerId
{
	Default,
	ActionOnCallback,
	ActionOnInput,
	Variable,
	ActionOnPhysicsTick
}
public class Power : ApoliObject
{
	public State state;
	public virtual PowerId type { get; set; }
	public virtual void Init()
	{
		state.StateEnterEvent += OnStateEnter;
		state.StateLeaveEvent += OnStateLeave;
	}
	public virtual void OnStateEnter()
	{

	}
	public virtual void OnStateLeave()
	{

	}
	public new static ParameterCollection parameterSet = new(
		new ParameterCollectionInitParam("Condition", TypeId.Condition, "")
	);
	public override void InitParam()
	{
		base.InitParam();
		parameters.AddFrom(parameterSet);
	}
}
public class ActionOnCallback : Power
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
