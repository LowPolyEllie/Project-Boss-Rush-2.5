using Godot;
using Godot.Collections;

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
	[Export]
	public Array<string> targets = [];

	/// <summary>
	/// If this is on, the drone will be controlled according to owner inputs
	/// </summary>
	[Export]
	public bool controllable = false;

	/// <summary>
	/// If controllable, this input type from the owner would cause this drone to move forward
	/// </summary>
	[Export]
	public string controlTrigger = "Fire";

	/// <summary>
	/// Interpolation point for how fast the drone turns
	/// </summary>
	[Export]
	public float accuracy = 0.9f;

	public override void _Ready()
	{
		targeter.subject = this;
		targeter.targets = targets;
		targeter.targetMode = targetMode;
	}
	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		float targetRot = targeter.GetTargetDirection(Rotation);

		Rotation = Mathf.LerpAngle(Rotation, targetRot, 1 - Mathf.Pow(1 - accuracy, deltaF));

		acceleration += Vector2.FromAngle(Rotation) * GetAcceleration();

		UpdateVelocity(deltaF);
	}
	public override void _Process(double delta)
	{
		if (controllable)
		{
			targeter.targetMode =
			owner.inputMachine.TryGetInputEnabled(controlTrigger) ?
			TargetMode.OWNER_TARGET : TargetMode.OWNER;
			inputMachine.SetInputEnabled("Fire",owner.inputMachine.TryGetInputEnabled(controlTrigger));
		}
	}
}
