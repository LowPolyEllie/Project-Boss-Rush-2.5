using BossRush2;
using Godot;

namespace Apoli.Actions;

public class NodeAction<Subject> : Action<Node> where Subject : Node
{
    public override void _DoAction(Node subject)
    {
        _DoNodeAction((Subject)subject);
    }
    public virtual void _DoNodeAction(Subject subject) { }
}
