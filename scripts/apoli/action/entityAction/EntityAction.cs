using Godot;
using Apoli.Types;
using BossRush2;

namespace Apoli.Actions;

public class EntityAction : Action
{
	public override void DoAction(Node subject)
	{
		DoEntityAction((Entity)subject);
	}
	public virtual void DoEntityAction(Entity subject) { }
}
