using Godot;
using Godot.Collections;
using System;

namespace BossRush2;

/// <summary>
/// A node that automatically faces a target
/// </summary>
[GlobalClass]
public partial class Turret : Node2D
{
	[Export]
	public Entity owner;
	[Export]
	public TargetMode targetMode;
	[Export]
	public Array<string> targets;

	public Targeter targeter = new();

	[Export]
	public bool controllable;

	/// <summary>
	/// Interpolation point for how fast the turret turns
	/// </summary>
	[Export]
	public float accuracy = 0.9f;

	public override void _Ready()
	{
		owner ??= this.SearchForParent<Entity>();
		targeter.targetMode = targetMode;
		targeter.targets = targets;
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		targeter.ResetTarget(owner);

		Vector2 targetPos = targeter.currentTarget.GetTargetPosition();
		float targetRot = GlobalPosition.AngleToPoint(targetPos);

		GlobalRotation += Mathf.LerpAngle(Rotation, targetRot, 1 - Mathf.Pow(1 - accuracy, deltaF));
	}

	public override void _Process(double delta) {
		if (controllable)
		{
			targeter.targetMode =
			owner.inputMachine.TryGetInputEnabled("Fire") ?
			TargetMode.OWNER_TARGET : TargetMode.OWNER;
		}
	}
}