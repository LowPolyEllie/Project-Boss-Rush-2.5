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
    public override void DoAction(Node subject)
    {
        ((Action)parameters.GetValue("Action")).DoAction((Entity)subject.Owner);
    }
}
