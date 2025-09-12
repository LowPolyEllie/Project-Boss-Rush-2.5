using Godot;
using BossRush2;

namespace Apoli.ValueFunctions;

public class EntityValueFunction<ReturnType> : ValueFunction<ReturnType,Entity>
{
	public override ReturnType GetValue(Entity subject)
	{
		return ReturnEntityValue(subject);
	}
	public virtual ReturnType ReturnEntityValue(Entity subject) { return default; }
}
