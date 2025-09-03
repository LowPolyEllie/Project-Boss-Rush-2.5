using Apoli.Powers;
using Godot;

namespace Apoli.ValueFunctions;

public enum ValueFunctionId
{
	
}
public class ValueFunction : ApoliObject
{
	public Power power;
	public virtual ValueFunctionId type { get; set; }
	public virtual object ReturnValue(Node subject) { return null; }
	public new static ParameterCollection parameterSet = new();
}
public class ValueFunction<ReturnType,Subject> : ValueFunction
{
	public override object ReturnValue(Node subject)
	{
		return _ReturnValue(subject);
	}
	public virtual ReturnType _ReturnValue(Node subject)
	{
		return default;
	}
}