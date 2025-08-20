using Godot;
using Apoli.Types;

namespace Apoli.Actions;
/// <summary>
/// Action class for changing the target
/// </summary>
public class TargetAction: Action {
	public override ActionId type { get; set; } = ActionId.Print;
	public new static ParameterCollection parameterSet = new(
		new ParameterCollectionInitParam("Action",TypeId.Action)
	);
}
