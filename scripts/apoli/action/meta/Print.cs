using Godot;
using Apoli.Types;

namespace Apoli.Actions;
public class Print: Action {
	public override ActionId type { get; set; } = ActionId.Print;
	public new static ParameterCollection parameterSet = new(
		new ParameterCollectionInitParam("Message",TypeId.String,"")
	);
	public override void DoAction(Node subject)
	{
		if (!parameters.HasParam("Message"))
		{
			return;
		}
		GD.Print((subject?.Name??"undefined")+ " : " + (string)parameters.GetValue("Message"));
	}
	public override void InitParam()
	{
		base.InitParam();
		parameters.AddFrom(parameterSet);
	}
}
