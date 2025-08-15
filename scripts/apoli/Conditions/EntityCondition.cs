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
/// <summary>
/// Conditions that take in an entity as it's subject
/// </summary>
public class EntityCondition : Condition
{
    public Entity subject;
}
/// <summary>
/// Whether the entity has a controller
/// </summary>
public class Controller : EntityCondition
{
    public override ConditionId type { get; set; } = ConditionId.Controller;
    public override bool CheckCondition()
    {
        if ((bool)parameters.GetValue("PlayerController"))
        {
            return subject.controller is PlayerController;
        }
        else
        { 
            return subject.controller is not null;
        }
    }
}
