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
		new ParameterCollectionInitParam("Interval",TypeId.Int,""),
		new ParameterCollectionInitParam("Action",TypeId.Action,"")
	);
	public override void InitParam()
	{
		base.InitParam();
		parameters.AddFrom(parameterSet);
	}
}
public class PhysicsTickPower : TickPower
{
	public override void Tick(double delta)
	{
		((Action)parameters.GetValue("Action")).DoAction(state.stateLayer.stateMachine.subject);
	}
	public override void OnStateEnter()
	{
		WorldInputHandler.WorldPhysicsProcessEvent += Tick;
		GD.Print("meow");
	}
	public override void OnStateLeave()
	{ 
		WorldInputHandler.WorldPhysicsProcessEvent -= Tick;
	}
}
