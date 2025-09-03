using Godot;

namespace Apoli.Actions;
public class Print: NodeAction<Node> {
	public override ActionId type { get; set; } = ActionId.Print;
	public new static ParameterCollection parameterSet = new(
		new ParameterInit<string>("Message","")
	);
	public override void DoAction(Node subject)
	{
		if (!parameters.HasParam("Message"))
		{
			return;
		}
		GD.Print((subject?.Name??"undefined")+ " : " + (string)parameters.GetValue("Message"));
	}
}
