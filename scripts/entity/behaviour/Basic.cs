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
public partial class Basic : Entity
{
	public override List<string> inputs
	{ get; set; } = ["Up", "Down", "Left", "Right", "Fire"];
	public override List<string> variantInputs { get; set; } = ["Target"];

	public void MovementControls()
	{
		//Create a directional vector
		Vector2 controlVector = new();
		if (inputMachine.GetInputEnabled("Up"))
		{
			controlVector.Y -= 1;
		}
		if (inputMachine.GetInputEnabled("Down"))
		{
			controlVector.Y += 1;
		}
		if (inputMachine.GetInputEnabled("Left"))
		{
			controlVector.X -= 1;
		}
		if (inputMachine.GetInputEnabled("Right"))
		{
			controlVector.X += 1;
		}
		//Normalise and apply if not zero
		if (controlVector != Vector2.Zero)
		{
			acceleration += controlVector.Normalized() * GetAcceleration();
		}
	}
	public override void _Ready()
	{
		base._Ready();
		/*
		stateMachine = new(this);
		stateMachine.AddLayer("Base", new StateLayer("Idle")
		{ 
			new State("Idle"){
				new PowerBuilder()
					.SetType(PowerId.ActionOnPhysicsTick)
					.SetParam("Action", new ActionType(
						new ActionBuilder()
							.SetType(ActionId.Print)
							.SetParam("Message",
								Type.FromValue("MEOW")
							)
						.Build()))
				.Build()
			}
		}.Init()
		);
		*/
	}
	public override void _Process(double delta)
	{
		float deltaF = (float)delta;

		if (inputMachine.GetVariantInput("Target").VariantType == Variant.Type.Vector2)
		{
			Vector2 mousePos = (Vector2)inputMachine.GetVariantInput("Target");
			Rotation = Position.AngleToPoint(mousePos);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementControls();
		UpdateVelocity((float)delta);
	}
}
