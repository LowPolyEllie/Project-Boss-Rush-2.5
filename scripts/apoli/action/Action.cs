using Apoli.Powers;

namespace Apoli.Actions;

public enum ActionId
{
	AllOf,
	TargetOwner,
	Print,
	BeginInput,
	EndInput,
	FireInput
}
public class Action : ApoliObject
{
	public Power power;
	public virtual ActionId type { get; set; }
	public virtual void DoAction(object subject) { }
	public new static ParameterCollection parameterSet = new(
		new ParameterInit<bool>("Condition")
	);
}
public class Action<Subject> : Action
{
	public override void DoAction(object subject) { _DoAction((Subject)subject); }
	public virtual void _DoAction(Subject subject) { }
}