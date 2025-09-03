using Apoli.States;

namespace Apoli.Powers;

public enum PowerId
{
	Default,
	ActionOnCallback,
	ActionOnInput,
	Variable,
	ActionOnPhysicsTick,
	StateChangeOnDelay
}
public class Power : ApoliObject
{
	public State state;
	public virtual PowerId type { get; set; }
	public override void Init()
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
		new ParameterInit<bool>("EntityCondition")
	);
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
