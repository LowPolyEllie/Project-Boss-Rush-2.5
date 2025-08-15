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
    public static Dictionary<PowerId, Dictionary<string, Parameter>> powerParameters = new()
    {
        {PowerId.ActionOnCallback,new(){
            {"ActionOnStateEnter",new Parameter(TypeId.Action,null)},
            {"ActionOnStateLeave",new Parameter(TypeId.Action,null)}
        }},
        {PowerId.ActionOnInput,new(){
            {"Action",new Parameter(TypeId.Action,null)},
            {"Input",new Parameter(TypeId.String,new Types.String("Fire"))},
            {"Press",new Parameter(TypeId.Bool,new Bool(true))}
        }},
        {PowerId.Variable,new(){
            {"Type",new Parameter(TypeId.TypeIdType,null)},
            {"Value",new Parameter(TypeId.Bool,new Bool(true))}
        }}
    };
    public static Dictionary<ActionId, Dictionary<string, Parameter>> actionParameters = new()
    {
        {ActionId.AllOf,new(){
            {"Actions",new Parameter(TypeId.ActionCollection,new())},
        }},
        {ActionId.Print,new(){
            {"Message",new Parameter(TypeId.String,null)},
        }}
    };
    public static Dictionary<ConditionId, Dictionary<string, Parameter>> conditionParameters = new()
    {
        {ConditionId.Controller,new(){
        }}
    };
    public Parameter(TypeId _type, Types.Type _value)
    {
        value = _value;
        type = _type;
    }
    /*public object Clone()
    {
        return new Parameter(type,value);
    }*/
}