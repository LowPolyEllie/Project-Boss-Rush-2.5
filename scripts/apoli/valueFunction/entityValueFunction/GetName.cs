using Godot;
using Apoli.Types;
using BossRush2;
using System;

namespace Apoli.ValueFunctions;

public class GetName : EntityValueFunction<string>
{
	public override string ReturnEntityValue(Entity subject) { return subject.Name; }
}
