using Apoli.Actions;
using BossRush2;

namespace Apoli.Powers;

public class TickPower : Power
{
    public virtual void Tick(double delta) { }
}
public class PhysicsTickPower : TickPower
{
    public int physicsTickInterval = 1;
    public override void Tick(double delta)
    {
        foreach (Action action in actions)
        {
            action.DoAction();
        }
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