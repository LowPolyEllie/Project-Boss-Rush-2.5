using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// A node that automatically faces a target
/// </summary>
[GlobalClass]
public partial class Turret : Entity
{
	/// <summary>
	/// The parent's input type that would cause this cannon to shoot
	/// </summary>
	[Export]
	public string shootTrigger = "Fire";

	public override List<string> inputs { get; set; } = ["Fire"];

	[Export]
	public TargetMode targetMode;
	[Export]
	public Array<string> targets;

	public Targeter targeter = new();

	[Export]
	public bool controllable;

	/// <summary>
	/// Overidden rotation value
	/// </summary>
	public float rotation;

	/// <summary>
	/// Interpolation point for how fast the turret turns
	/// </summary>
	[Export]
	public float accuracy = 0.9f;

	public override void _Ready()
	{
		Init();
		owner ??= this.SearchForParent<Entity>();
		targeter.targetMode = targetMode;
		targeter.targets = targets;
		rotation = GlobalRotation;
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		targeter.ResetTarget(this);

		Vector2 targetPos = targeter.currentTarget.GetTargetPosition();
		float targetRot = GlobalPosition.AngleToPoint(targetPos);

		rotation = Mathf.LerpAngle(rotation, targetRot, 1 - Mathf.Pow(1 - accuracy, deltaF));
		GlobalRotation = rotation;
		inputMachine.SetInputEnabled("Fire", owner.inputMachine.TryGetInputEnabled(shootTrigger));
	}

	public override void _Process(double delta)
	{
		GlobalRotation = rotation;
	}
}
