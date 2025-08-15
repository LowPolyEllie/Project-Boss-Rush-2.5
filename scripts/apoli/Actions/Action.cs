using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Powers;
using Apoli.Types;
using Godot;

namespace Apoli.Actions;
public enum ActionId {
	AllOf,
	Print
}
public class Action {
	public virtual ActionId type { get; set; }
    public virtual ParameterCollection parameters {get;set; }
	public virtual void DoAction() { }
}
public class ActionBuilder
{
	private ActionId type;
    public virtual ParameterCollection _parameters {get;set; }
	public Action Build()
	{
		Action newAction;
		switch (type)
		{
			case ActionId.Print:
				newAction = new Print();
				break;
			default:
				throw new Exception("ActionBuilder: No class specified");
		}
		newAction.parameters = _parameters;
		return newAction;
	}
    public ActionBuilder SetParam(string Key, Types.Type Value)
    {
        if (!_parameters.HasParam(Key))
        {
            throw new KeyNotFoundException("No keys matching \"" + Key + "\" found. use PowerBuilder.SetType() before setting values");
        }
        if (_parameters.GetType(Key) != Value.type)
        {
            throw new TypeLoadException("Wrong Apoli type: Expected "+_parameters.GetType(Key)+", got "+Value.type);
        }
        _parameters.SetParam(Key,Value);
        _parameters = Parameter.actionParameters[type];
        return this;
    }
	public ActionBuilder SetType(ActionId _type)
	{
		type = _type;
		return this;
	}
}
public class Print: Action {
	public override ActionId type { get; set; } = ActionId.Print;
	public override void DoAction() {
		if (!parameters.HasParam("Message")) {
			return;
		}
		GD.Print((string) parameters.GetValue("Message"));
	}
}
