using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Types;
using Godot;

namespace Apoli.Actions;
public enum ActionId {
	all_of,
	print
}
public class Action {
	public virtual ActionId type { get; set; }
	public Dictionary < string, Types.Type > parameters;
	public virtual void DoAction() { }
}
public class ActionBuilder
{
	private ActionId type;
	private Dictionary<string, Types.Type> _parameters = new();
	public Action Build()
	{
		Action newAction;
		switch (type)
		{
			case ActionId.print:
				newAction = new Print();
				break;
			default:
				throw new Exception("ActionBuilder: No class specified");
		}
		newAction.parameters = _parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
		return newAction;
	}
	public ActionBuilder SetParam(string Key, Types.Type Value)
	{
		_parameters.Add(Key, Value);
		return this;
	}
	public ActionBuilder SetType(ActionId _type)
	{
		type = _type;
		return this;
	}
}
public class Print: Action {
	public override ActionId type { get; set; } = ActionId.print;
	public override void DoAction() {
		if (!parameters.ContainsKey("Message")) {
			return;
		}
		if (parameters["Message"].type != TypeId.String) {
			return;
		}
		GD.Print((string) parameters["Message"].value);
	}
}
