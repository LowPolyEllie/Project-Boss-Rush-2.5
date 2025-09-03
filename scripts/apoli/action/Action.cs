using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Conditions;
using Apoli.Powers;
using Apoli.Types;
using BossRush2;
using Godot;

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
	public virtual void DoAction(Node subject) { }
    public new static ParameterCollection parameterSet = new(
        new ParameterInit<Condition>("Condition")
    );
}