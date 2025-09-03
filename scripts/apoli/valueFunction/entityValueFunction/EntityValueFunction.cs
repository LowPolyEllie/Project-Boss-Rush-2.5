using Godot;
using Apoli.Types;
using BossRush2;

namespace Apoli.ValueFunctions;

public class EntityValueFunction<T> : ValueFunction<T>
{
	public override object ReturnValue(Node subject)
	{
		return ReturnEntityValue((Entity)subject);
	}
	public virtual T ReturnEntityValue(Entity subject) { return default; }
}
