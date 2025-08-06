using Godot;
using System;

namespace BossRush2;

/// <summary>
/// An entity that tracks and chases targets
/// </summary>
[GlobalClass]
public partial class Drone : Basic
{
	public Targeter targeter = new();
	[Export]
	public TargetMode targetMode = TargetMode.NONE;

	/// <summary>
	/// Interpolation point for how fast the drone turns
	/// </summary>
	[Export]
	public float accuracy = 0.9f;

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		targeter.ResetTarget(this);

		Vector2 targetPos = targeter.currentTarget.GetTargetPosition();
		float targetRot = GlobalPosition.AngleToPoint(targetPos);

		Rotation = Mathf.LerpAngle(Rotation, targetRot, 1 - Mathf.Pow(1 - accuracy, deltaF));

		acceleration += Vector2.FromAngle(Rotation) * GetAcceleration();

		UpdateVelocity(deltaF);
	}
	public override void _Process(double delta)
	{
		targeter.targetMode =
		owner.inputMachine.TryGetInputEnabled("Fire") ?
		TargetMode.OWNER_TARGET : TargetMode.OWNER;
		inputMachine.SetInputEnabled("Fire",owner.inputMachine.TryGetInputEnabled("Fire"));
	}
}
