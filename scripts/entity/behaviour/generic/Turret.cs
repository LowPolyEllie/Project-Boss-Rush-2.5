using Godot;
using Godot.Collections;
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

	public float startingRot;

	/// <summary>
	/// Interpolation point for how fast the turret turns
	/// </summary>
	[Export]
	public float accuracy = 0.9f;

	public override void _Ready()
	{
		Init();
		owner ??= this.SearchForParent<Entity>();
		targeter.subject = this;
		targeter.targets = targets;
		targeter.targetMode = targetMode;
		rotation = GlobalRotation;
		startingRot = Rotation;
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		float targetRot = targeter.GetTargetDirection(startingRot + GetParent<Node2D>().GlobalRotation);

		rotation = Mathf.LerpAngle(rotation, targetRot, 1 - Mathf.Pow(1 - accuracy, deltaF));
		GlobalRotation = rotation;
		inputMachine.SetInputEnabled("Fire", owner.inputMachine.TryGetInputEnabled(shootTrigger));
	}

	public override void _Process(double delta)
	{
		GlobalRotation = rotation;
		if (controllable)
		{
			targeter.targetMode =
			owner.inputMachine.TryGetInputEnabled(shootTrigger) ?
			TargetMode.OWNER_TARGET : TargetMode.NONE;
			inputMachine.SetInputEnabled("Fire",owner.inputMachine.TryGetInputEnabled(shootTrigger));
		}
	}
}
