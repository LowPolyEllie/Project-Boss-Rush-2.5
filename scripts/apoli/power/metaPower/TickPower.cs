using Apoli.Actions;
using BossRush2;
using Apoli.Types;
using Godot;
using Apoli.States;

namespace Apoli.Powers;

public class TickPower : Power
{
	public virtual void Tick(double delta) { }
	public new static ParameterCollection parameterSet = new(
		new ParameterInit<int>("Interval",1),
		new ParameterInit<Action>("Action")
	);
}
public class PhysicsTickPower : TickPower
{
    public override PowerId type { get; set; } = PowerId.ActionOnPhysicsTick;
	public override void Tick(double delta)
	{
		((Action)parameters.GetValue("Action")).DoAction(state.stateLayer.stateMachine.subject);
	}
	public override void OnStateEnter()
	{
		WorldInputHandler.WorldPhysicsProcessEvent += Tick;
	}
	public override void OnStateLeave()
	{ 
		WorldInputHandler.WorldPhysicsProcessEvent -= Tick;
	}
}
