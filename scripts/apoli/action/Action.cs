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
	public new static ParameterCollection parameterSet = new(
		new ParameterInit<bool>("Condition")
	);
}
public interface IAction<in Subject>
{
	public void DoAction(Subject subject);
}
public class Action<Subject> : Action,IAction<Subject>
{
	public virtual void DoAction(Subject subject) {}
}