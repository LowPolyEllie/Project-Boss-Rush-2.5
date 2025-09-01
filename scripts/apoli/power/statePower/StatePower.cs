

using Apoli.Actions;
using BossRush2;
using Apoli.Types;
using Godot;
using Apoli.States;

namespace Apoli.Powers;

public class StatePower : Power
{
	public new static ParameterCollection parameterSet = new(
		new ParameterCollectionInitParam("State",TypeId.String,"")
	);
}
public class StateChangeOnDelay : StatePower
{
    public override PowerId type { get; set; } = PowerId.StateChangeOnDelay;
    private int timer = 0;
    private string targetState;
	public new static ParameterCollection parameterSet = new(
        new ParameterCollectionInitParam("Delay", TypeId.Int, "")
    );
    public void Tick(double delta)
    {
        timer -= 1;
        if (timer == 0)
        {
            state.stateLayer.SetState(targetState);
        }
	}
    public override void Init()
    {
        base.Init();
        
        targetState = parameters.GetValue<string>("State");
        if (!state.stateLayer.HasState(targetState))
        {
            throw new InvalidParameterException("No state in layer called " + targetState);
        }
    }
    public override void OnStateEnter()
    {
        timer = parameters.GetValue<int>("Delay");
        if (timer <= 0)
        {
            throw new InvalidParameterException("Delay can't be <= 0, is " + timer);
        }

		WorldInputHandler.WorldPhysicsProcessEvent += Tick;
	}
	public override void OnStateLeave()
	{ 
		WorldInputHandler.WorldPhysicsProcessEvent -= Tick;
	}
}
