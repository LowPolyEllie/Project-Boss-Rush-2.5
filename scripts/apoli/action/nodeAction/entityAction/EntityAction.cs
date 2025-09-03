using BossRush2;
using Godot;

namespace Apoli.Actions;

public class EntityAction : NodeAction<Entity>
{
    public override void _DoNodeAction(Entity subject)
    {
        _DoEntityAction(subject);
    }
    public virtual void _DoEntityAction(Entity subject)
    {
        
    }
}
