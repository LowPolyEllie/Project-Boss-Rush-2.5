using Godot;

namespace BossRush2;

/// <summary>
/// An alternate Drone with a follow limit
/// </summary>
[GlobalClass]
public partial class Minion : Drone
{
	/// <summary>
	/// A seperate set of <c>Accuracy</c> specifically for <c>Minion</c> nodes
	/// </summary>
	[Export]
	public float minionAccuracy = 0.95f;

	/// <summary>
	/// Whether or not to use <c>MinionAccuracy</c> instead of <c>Accuracy</c>
	/// </summary>
	[Export]
	public bool useMinionAccuracy = true;

	/// <summary>
	/// How close the minion can get to its target
	/// </summary>
	[Export]
	public float followLimit = 100f;

	/// <summary>
	/// If true, the FollowLimit is ignored
	/// </summary>
	[Export]
	public bool ignoreFollowLimit = false;

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		Vector2? targetPos = targeter.GetTargetPositionOrNull();
		if (targetPos is Vector2 trueTargetPos)
		{
			if (ignoreFollowLimit || Position.DistanceSquaredTo(trueTargetPos) > followLimit * followLimit)
			{
				acceleration += Vector2.FromAngle(Rotation) * GetAcceleration();
			}
			else
			{
				acceleration -= Vector2.FromAngle(Rotation) * GetAcceleration();
				acceleration += Vector2.FromAngle(Rotation - Mathf.Pi / 2) * GetAcceleration();
			}
		}

		float targetRot = targeter.GetTargetDirection(Rotation);

		float newAccuracy = useMinionAccuracy ? minionAccuracy : accuracy;
		
		Rotation = Mathf.LerpAngle(Rotation, targetRot, 1 - Mathf.Pow(1 - newAccuracy, deltaF));


		UpdateVelocity(deltaF);
	}
	public override void _Process(double delta)
	{
		if (controllable)
		{
			if (owner.inputMachine.TryGetInputEnabled("Fire"))
			{
				targeter.targetMode = TargetMode.OWNER_TARGET;
				ignoreFollowLimit = false;
				useMinionAccuracy = true;
			}
			else
			{
				targeter.targetMode = TargetMode.OWNER;
				ignoreFollowLimit = true;
				useMinionAccuracy = false;
			}
			inputMachine.SetInputEnabled("Fire",owner.inputMachine.TryGetInputEnabled("Fire"));
		}
	}
}
