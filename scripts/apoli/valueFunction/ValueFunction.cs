using Apoli.Powers;
using Apoli.Types;
using Godot;

namespace Apoli.ValueFunctions;

public enum ValueFunctionId
{
	
}
public class ValueFunction : ApoliObject
{
	public Power power;
	public virtual ValueFunctionId type { get; set; }
	public new static ParameterCollection parameterSet = new();
}
public interface IValueFunction<T,in Subject>:IValue<T>
{
    public T GetValue(Subject subject);
}
public class ValueFunction<ReturnType, Subject> : ValueFunction, IValueFunction<ReturnType, Subject>
{
	public object GetUndefinedValue() { return null; }
	public virtual ReturnType GetValue()
	{
		return default;
	}
	public virtual ReturnType GetValue(Subject subject)
	{
		return GetValue();
	}
	
	public T GetValue<T>(string key, Subject subject){
		return parameters.GetValue<T, Subject>(key, subject);
	}
}