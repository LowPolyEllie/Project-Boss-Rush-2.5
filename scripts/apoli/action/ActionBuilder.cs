using System;
using System.Collections.Generic;

namespace Apoli.Actions;

public class ActionBuilder<ActionType> : ApoliObjectBuilder<ActionType> where ActionType:Action
{
	public Action Build()
	{
		return (Action)_Build();
	}
	public ActionBuilder<ActionType> SetParam(string Key, Types.Type Value)
	{
		_SetParam(Key, Value);
		return this;
	}
}
