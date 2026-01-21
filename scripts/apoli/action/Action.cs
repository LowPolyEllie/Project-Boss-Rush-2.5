using System;
using System.Collections;
using Apoli.Powers;
using Godot.Collections;

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
public class Action<Subject> : Action, IAction<Subject>
{
	public virtual void DoAction(Subject subject) { }

	public T GetValue<T>(string key, Subject subject)
	{
		return parameters.GetValue<T, Subject>(key, subject);
	}
	public void SetValue<T>(T value)
    {
        if (typeof(T).IsPrimitive){ throw new Exception(); }
    }
}