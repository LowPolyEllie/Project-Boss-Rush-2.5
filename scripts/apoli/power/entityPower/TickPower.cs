using Apoli.Actions;
using BossRush2;

namespace Apoli.Powers;

public class ActionOnTick : Power
{
	public virtual void Tick(double delta) { }
	public new static ParameterCollection parameterSet = new(
		new ParameterInit<int>("Interval", 1),
		new ParameterInit<IAction<Entity>>("EntityAction")
	);
}
public class ActionOnPhysicsTick : ActionOnTick
{
    public override PowerId type { get; set; } = PowerId.ActionOnPhysicsTick;
	public override void Tick(double delta)
	{
		parameters.GetValue<IAction<Entity>>("EntityAction").DoAction((Entity)state.stateLayer.stateMachine.subject);
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
