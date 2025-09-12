using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// A node that automatically faces a target
/// </summary>
[GlobalClass]
public partial class Turret : Entity, ITargetable
{
	/// <summary>
	/// The parent's input type that would cause this cannon to shoot
	/// </summary>
	[Export]
	public string shootTrigger = "Fire";

	public override List<string> inputs { get; set; } = ["Fire"];

	[Export]
	public Targeter targeter { get; set; }

	[Export]
	public bool controllable;

	/// <summary>
	/// Overidden rotation value
	/// </summary>
	public float rotation;

	public float startingRot;

	protected TargetMode initTargetMode;

	/// <summary>
	/// Interpolation point for how fast the turret turns
	/// </summary>
	[Export]
	public float accuracy = 0.9f;

	public override void _Ready()
	{
		Init();
		owner ??= this.SearchForParent<Entity>();
		rotation = GlobalRotation;
		startingRot = Rotation;
		initTargetMode = targeter.targetMode;
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		float targetRot = targeter.GetTargetDirection(startingRot + GetParent<Node2D>().GlobalRotation);

		rotation = Mathf.LerpAngle(rotation, targetRot, 1 - Mathf.Pow(1 - accuracy, deltaF));
		GlobalRotation = rotation;
		inputMachine.SetInputEnabled("Fire",!targeter.currentTarget.emptyTarget);
	}

	public override void _Process(double delta)
	{
		GlobalRotation = rotation;
		if (controllable)
		{
			targeter.targetMode =
			owner.inputMachine.TryGetInputEnabled(shootTrigger) ?
			TargetMode.OWNER_TARGET : initTargetMode;
		}
	}
}
