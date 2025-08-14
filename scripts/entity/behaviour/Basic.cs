using Godot;
using Godot.Collections;
using Apoli;
using Apoli.Powers;
using Apoli.Types;
using Apoli.Actions;
using Microsoft.VisualBasic;
using System.Collections.Generic;
namespace BossRush2;

[GlobalClass]
public partial class Basic : Entity
{
	[Export]
	public Array<TankLoader> currentLoadout = [];
	int currentTier = 0;

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

	public override void _Process(double delta)
	{
		float deltaF = (float)delta;

		if (inputMachine.GetVariantInput("Target").VariantType == Variant.Type.Vector2)
		{
			Vector2 mousePos = (Vector2)inputMachine.GetVariantInput("Target");
			Rotation = Position.AngleToPoint(mousePos);
		}

		//Placeholder, for until I add a proper wave system
		if (Input.IsActionJustPressed("debug_1"))
		{
			currentTier++;
			if (currentTier == currentLoadout.Count)
			{
				currentTier = 0;
			}
			currentLoadout[currentTier].LoadTank(this);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementControls();
		UpdateVelocity((float)delta);
	}
}
