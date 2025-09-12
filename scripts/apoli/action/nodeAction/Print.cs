using Godot;

namespace Apoli.Actions;
public class Print<Subject>: NodeAction<Subject> where Subject : Node {
	public override ActionId type { get; set; } = ActionId.Print;
	public new static ParameterCollection parameterSet = new(
		new ParameterInit<string>("Message","")
	);
	public override void DoAction(Subject subject)
	{
		if (!parameters.HasParam("Message"))
		{
			return;
		}
		GD.Print((subject?.Name??"undefined")+ " : " + GetValue<string>("Message",subject));
	}
}
