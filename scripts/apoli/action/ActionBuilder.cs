using Apoli.Types;
using Godot;

namespace Apoli.Actions;

public class ActionBuilder<ActionType> : ApoliObjectBuilder<ActionType> where ActionType : Action
{
	public ActionBuilder<ActionType> SetParam<T>(string Key, IValue<T> Value)
	{
		_SetParam<T>(Key, Value);
		return this;
	}
	public ActionBuilder<ActionType> SetParam<T>(string Key, T Value)
	{
		_SetParam(Key, Value);
		return this;
	}
	public new ActionType Build()
	{
		return (ActionType)_Build();
	}
}

public static class ActionBuilderFactoryHelper
{
    public static ActionBuilder<T> NewBuilder<T>() where T:Action
    {
        return new();
    }
}