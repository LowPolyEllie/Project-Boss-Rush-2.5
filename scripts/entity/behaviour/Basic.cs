using Godot;
using Godot.Collections;
using Apoli;
using Apoli.Powers;
using Apoli.Types;
using Apoli.Actions;
using Microsoft.VisualBasic;
namespace BossRush2;

[GlobalClass]
public partial class Basic : Entity
{
	public override Array<string> inputs { get; set; } = ["Up","Down","Left","Right","Fire"];
	public override Array<string> variantInputs { get; set; } = ["Target"];
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
			AccRate += controlVector.Normalized() * GetAcceleration();
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
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementControls();
		UpdateVelocity((float)delta);
	}
}
