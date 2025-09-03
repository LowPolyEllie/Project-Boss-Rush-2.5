using Godot;
using Apoli.Types;

namespace Apoli.Actions;
/// <summary>
/// Action class for changing the target
/// </summary>
public class TargetAction: EntityAction {
	public new static ParameterCollection parameterSet = new(
		new ParameterInit<Action>("Action")
	);
}
