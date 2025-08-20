using Apoli.Types;
using Apoli.Actions;
using Apoli.Powers;
using Godot;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using Apoli.Conditions;

namespace Apoli;

public class Parameter //: ICloneable
{
	public Types.Type value { get; set; }
	public TypeId type { get; set; }
	public static Dictionary<PowerId, ParameterCollection> powerParameters = new()
	{
		{PowerId.ActionOnCallback,new(
			new("ActionOnStateEnter",TypeId.Action),
			new("ActionOnStateLeave",TypeId.Action)
		)},
		{PowerId.ActionOnInput,new(
			new("Action",TypeId.Action),
			new("Input",TypeId.String,new Types.String("Fire")),
			new("Press",TypeId.Bool,true)
		)},
		{PowerId.Variable,new(
			new("Type",TypeId.TypeIdType),
			new("Value",TypeId.Bool,true)
		)},
		{PowerId.ActionOnPhysicsTick,new(
			new("Action",TypeId.Action),
			new("Interval",TypeId.Bool,true)
		)}
	};
	public static Dictionary<ActionId, ParameterCollection> actionParameters = new()
	{
		{ActionId.AllOf,new(
			new ParameterCollectionInitParam("Actions",TypeId.ActionCollection)
		)},
		{ActionId.Print,new(
			new ParameterCollectionInitParam("Message",TypeId.String)
		)}
	};
	public static Dictionary<ConditionId, ParameterCollection> conditionParameters = new()
	{
		{ConditionId.Controller,new(
			new ParameterCollectionInitParam("PlayerController",TypeId.Bool)
		)}
	};
	public Parameter(TypeId _type, Types.Type _value)
	{
		value = _value;
		type = _type;
	}
	public Parameter(TypeId _type, object _value)
	{
		value = Types.Type.FromValue(_value);
		type = _type;
	}
	public Parameter(TypeId _type)
	{
		type = _type;
	}
	public Parameter(bool _bool)
	{
		type = TypeId.Bool;
		value = new Bool(_bool);
	}
	public Parameter(string _string)
	{
		type = TypeId.String;
		value = new Types.String(_string);
	}
	/*public object Clone()
	{
		return new Parameter(type,value);
	}*/
	public override string ToString()
	{
		return value.ToString()+"("+type.ToString()+")";
	}

}
