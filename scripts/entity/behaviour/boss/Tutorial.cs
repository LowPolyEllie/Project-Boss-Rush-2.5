using Godot;
using Godot.Collections;
using Apoli;
using Apoli.Powers;
using Apoli.Types;
using Apoli.Actions;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using Apoli.States;
namespace BossRush2;

[GlobalClass]
public partial class Tutorial : Basic
{
	public override void _Ready()
	{
		Init();
		stateMachine = new(this);
		stateMachine.AddLayer("Base", new StateLayer("Idle")
		{
			new State("Idle"){
				new PowerBuilder()
					.SetType(PowerId.ActionOnPhysicsTick)
					.SetParam("Action", new Type<Action>(
						new ActionBuilder()
							.SetType(ActionId.Print)
							.SetParam("Message",
								Type.FromValue("CMONNNN")
							)
						.Build()))
				.Build()
			}
		}
		);
		stateMachine.Init();
	}
}
