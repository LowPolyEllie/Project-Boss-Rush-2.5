using Godot;
using BossRush2;

namespace Apoli.ValueFunctions;

public class EntityValueFunction<ReturnType> : ValueFunction<ReturnType,Entity>
{
	public override object ReturnValue(Node subject)
	{
		return ReturnEntityValue((Entity)subject);
	}
	public virtual ReturnType ReturnEntityValue(Entity subject) { return default; }
}
