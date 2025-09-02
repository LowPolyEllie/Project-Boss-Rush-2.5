using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Apoli.Powers;
using Apoli.Types;
using BossRush2;
using Godot;

namespace Apoli.Conditions;

public enum ConditionId
{
	Controller
}
public class Condition : ApoliObject
{
	public virtual ConditionId type { get; set; }
	public virtual bool CheckCondition() { return false; }
}
