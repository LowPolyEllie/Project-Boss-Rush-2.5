using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Node must have the name "Player" to be detected
/// </summary>
public partial class Player : Entity
{
	/// <summary>
	/// Moves the player based on set inputs
	/// </summary>
	public void MovementControls()
	{
		//Create a directional vector
		Vector2 inputDir = new(
			Input.GetActionStrength("moveRight") - Input.GetActionStrength("moveLeft"),
			Input.GetActionStrength("moveDown") - Input.GetActionStrength("moveUp")
		);

		//Normalise and apply if not zero
		if (inputDir != Vector2.Zero)
		{
			AccRate += inputDir.Normalized() * GetAcceleration();
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
		float deltaF = (float)delta;
		MovementControls();
		UpdateVelocity(deltaF);
	}
}
