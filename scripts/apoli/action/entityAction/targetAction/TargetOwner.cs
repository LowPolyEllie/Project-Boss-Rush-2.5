using Godot;
using Apoli.Types;
using BossRush2;

namespace Apoli.Actions;
/// <summary>
/// Diverts subsequent actions to the owner. Entity only
/// </summary>
public class TargetOwner : TargetAction
{
    public override ActionId type { get; set; } = ActionId.TargetOwner;
    public override void DoEntityAction(Entity subject)
    {
        parameters.GetValue<Action>("Action",subject).DoAction(subject.Owner);
    }
}
