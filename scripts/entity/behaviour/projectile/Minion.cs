using Godot;
using System;

namespace BossRush2;

/// <summary>
/// An alternate Drone with a follow limit
/// </summary>
public partial class Minion : Drone
{
	/// <summary>
	/// A seperate set of <c>Accuracy</c> specifically for <c>Minion</c> nodes
	/// </summary>
	[Export]
	public float MinionAccuracy = 0.95f;

	/// <summary>
	/// Whether or not to use <c>MinionAccuracy</c> instead of <c>Accuracy</c>
	/// </summary>
	[Export]
	public bool UseMinionAccuracy = true;

	/// <summary>
	/// How close the minion can get to its target
	/// </summary>
	[Export]
	public float FollowLimit = 100f;

	/// <summary>
	/// If true, the FollowLimit is ignored
	/// </summary>
	[Export]
	public bool IgnoreFollowLimit = false;

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		MyTargeter.ResetTarget();

		Vector2 targetPos = MyTargeter.CurrentTarget.GlobalPosition;
		float targetRot = GlobalPosition.AngleToPoint(targetPos);

		float accuracy = UseMinionAccuracy ? MinionAccuracy : Accuracy;

		Rotation = Mathf.LerpAngle(Rotation, targetRot, 1 - Mathf.Pow(1 - accuracy, deltaF));

		if (IgnoreFollowLimit || Position.DistanceSquaredTo(targetPos) > FollowLimit * FollowLimit)
		{
			AccRate += Vector2.FromAngle(Rotation) * GetAcceleration();
		}
		else
		{
			AccRate -= Vector2.FromAngle(Rotation) * GetAcceleration();
			AccRate += Vector2.FromAngle(Rotation - Mathf.Pi / 2) * GetAcceleration();
		}

		UpdateVelocity(deltaF);
	}
}
