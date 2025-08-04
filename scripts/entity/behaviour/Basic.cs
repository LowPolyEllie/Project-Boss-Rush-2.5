using Godot;
using System;
using Apoli;
using Apoli.Powers;
using Apoli.Types;
using Apoli.Actions;
using System.Security.Cryptography.X509Certificates;
namespace BossRush2;

[GlobalClass]
public partial class Basic : Entity
{
	public void MovementControls()
	{
		//Create a directional vector
		Vector2 controlVector = new();
		if (inputMachine.InputEnabled("Up"))
		{
			controlVector.Y -= 1;
		}
		if (inputMachine.InputEnabled("Down"))
		{
			controlVector.Y += 1;
		}
		if (inputMachine.InputEnabled("Left"))
		{
			controlVector.X -= 1;
		}
		if (inputMachine.InputEnabled("Right"))
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
		Vector2 mousePos = GetGlobalMousePosition();
		Rotation = Position.AngleToPoint(mousePos);
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementControls();
		UpdateVelocity((float)delta);
	}
}
