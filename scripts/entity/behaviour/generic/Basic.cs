using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BossRush2;

[GlobalClass]
public partial class Basic : Entity, ITargetable
{
	[Export]
	public Targeter targeter { get; set; } = new();

	public override List<string> inputs
	{ get; set; } = ["Up", "Down", "Left", "Right", "Fire", "Fire2", "TiltLeft", "TiltRight"];
	// public override List<string> variantInputs { get; set; } = ["Target"];

	public void MovementControls()
	{
		//Create a directional vector
		Vector2 controlVector = new();
		if (inputMachine.GetInputEnabled("Up"))
		{
			controlVector.Y += 1;
		}
		if (inputMachine.GetInputEnabled("Down"))
		{
			controlVector.Y -= 1;
		}
		if (inputMachine.GetInputEnabled("Left"))
		{
			controlVector.X -= 1;
		}
		if (inputMachine.GetInputEnabled("Right"))
		{
			controlVector.X += 1;
		}
		if (inputMachine.GetInputEnabled("TiltLeft"))
		{
			controller.basis.axisAngle -= 5;
		}
		if (inputMachine.GetInputEnabled("TiltRight"))
		{
			controller.basis.axisAngle += 5;
		}
		//Normalise and apply if not zero
		if (controlVector != Vector2.Zero)
		{
			acceleration += controller.basis.RotateControlVector(controlVector) * GetAcceleration();
		}
	}
	public override void _Ready()
	{
		Init();
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
		}
		);
		stateMachine.Init();
		*/
	}
	public override void _Process(double delta)
	{
		float deltaF = (float)delta;
		Rotation = targeter.GetTargetDirection(Rotation);
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementControls();
		UpdateVelocity((float)delta);
	}
}
