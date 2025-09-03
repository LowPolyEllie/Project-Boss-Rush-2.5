using BossRush2;
using Godot;

namespace Apoli.Actions;
/// <summary>
/// Diverts subsequent actions to the owner. Entity only
/// </summary>
public class TargetOwner : TargetAction
{
    public override ActionId type { get; set; } = ActionId.TargetOwner;
    public override void DoAction(Entity subject)
    {
        parameters.GetValue<EntityAction>("EntityAction", subject).DoAction((Entity)subject.Owner);
    }
}
