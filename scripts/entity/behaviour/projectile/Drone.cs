using Godot;
using System;

namespace BossRush2;

/// <summary>
/// An entity that tracks and chases targets
/// </summary>
public partial class Drone : Entity
{
	/// <summary>
	/// The targeting node, self explanatory
	/// </summary>
	[Export]
	public Targeter MyTargeter;

	/// <summary>
	/// Interpolation point for how fast the drone turns
	/// </summary>
	[Export]
	public float Accuracy = 0.9f;

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		MyTargeter.ResetTarget();

		Vector2 targetPos = MyTargeter.CurrentTarget.GlobalPosition;
		float targetRot = GlobalPosition.AngleToPoint(targetPos);

		Rotation = Mathf.LerpAngle(Rotation, targetRot, 1 - Mathf.Pow(1 - Accuracy, deltaF));

		AccRate += Vector2.FromAngle(Rotation) * GetAcceleration();

		UpdateVelocity(deltaF);
	}
}
