using BossRush2;

namespace Apoli.ValueFunctions;

public class GetName : EntityValueFunction<string>
{
	public override string ReturnEntityValue(Entity subject) { return subject.Name; }
}
