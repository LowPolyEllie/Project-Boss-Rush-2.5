using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BossRush2;

/// <summary>
/// Base class for all game entities in Boss Rush 2.
/// </summary>
/// <remarks>
/// <br>Provides shared logic for movement, stat management, collision response, and health tracking.</br>
/// <br>Designed to simplify entity creation with default friction, stat scaling, health bar integration, and more.</br>
/// </remarks>
[GlobalClass]
public partial class Entity : CharacterBody2D, ICollidable
{
	/// <summary>
	/// The input machine of the entity. Controls everything. Override inputs and variantInputs to register mandatory inputs
	/// </summary>
	public InputMachine inputMachine;
	public virtual Array<string> inputs { get; set; } = [];
	public virtual Array<string> variantInputs { get; set; } = [];
	/// <summary>
	/// The entity's owner. Meant for controllable projectiles and potentially damage reflection
	/// </summary>
	[Export]
	public new Entity Owner;
	/// <summary>
	/// The acceleration applied during a frame, <c>delta</c> is already accounted for, so ignore it
	/// </summary>
	protected Vector2 AccRate;

	/// <summary>
	/// <c>AccRate</c>, but for <c>RotVelocity</c> instead of <c>Velocity</c>
	/// </summary>
	protected float RotAccRate;

	[Export]
	public Stats MyStats { get; set; } = new();

	/// <summary>
	/// Rotational Velocity, uses radians because its rarely utilised
	/// </summary>
	[Export]
	public float RotVelocity = 0f;

	/// <summary>
	/// Amplifies or reduces the impact from World.Friction
	/// </summary>
	[Export]
	public float FrictionPower = 1f;

	/// <summary>
	/// Amplifies or reduces the impact from World.Friction
	/// </summary>
	/// <remarks>
	/// <br> Theres no rotational friction by default </br>
	/// </remarks>
	[Export]
	public float RotFrictionPower = 0f;

	/// <summary>
	/// I regret using godot's CharacterBody2D class
	/// </summary>
	[Export]
	Vector2 _Velocity = new();

	/// <summary>
	/// Returns the Acceleration, assuming the Speed stat as terminal velocity, and friction as 0.02f
	/// </summary>
	/// <returns></returns>
	public float GetAcceleration()
	{
		return MyStats.Speed * (1 - World.DefaultFriction);
	}

	/// <summary>
	/// Alters transformation and initial velocity based on transformation data
	/// </summary>
	public void ApplyTransform(Node2D source)
	{
		GlobalPosition += source.GlobalPosition;
		GlobalRotation += source.GlobalRotation;
		GlobalScale *= source.GlobalScale;

		Velocity = Velocity.Rotated(source.GlobalRotation);
	}

	/// <summary>
	/// Automatic velocity and friction updating
	/// </summary>
	public void UpdateVelocity(float deltaF)
	{
		if (FrictionPower > 0f)
		{
			Velocity = ExtraMath.PredictVelocity(
			Velocity, AccRate, Mathf.Pow(World.Friction, FrictionPower), deltaF
			);
		}
		else
		{
			Velocity += AccRate * deltaF;
		}

		if (RotFrictionPower > 0f)
		{
			RotVelocity = ExtraMath.PredictVelocity(
			RotVelocity, RotAccRate, Mathf.Pow(World.Friction, RotFrictionPower), deltaF
			);
		}
		else
		{
			RotVelocity += RotAccRate * deltaF;
		}

		MoveAndSlide();
		Rotation += RotVelocity * deltaF;
		AccRate = Vector2.Zero;
	}

	/// <summary>
	/// This represents current health, refer to <c>MyStats.Health</c> instead if you wish to change that
	/// </summary>
	[Export]
	public float CurrentHealth;

	/// <summary>
	/// A method, dedicated to <c>StatBar</c> instances. 
	/// </summary>
	/// <remarks>
	/// <br> Connecting new stats must be done with code due to engine limitations </br>
	/// </remarks>
	public (double min, double max, double value) HealthRange() => (0, MyStats.Health, CurrentHealth);

	[Export]
	protected Control _HealthBarRef;
	public StatBar HealthBarRef;

	// Team and group data

	[Export(PropertyHint.Enum, "playerTeam,bossTeam,polygonTeam")]
	public string Team { get; set; }

	[Export]
	public Array<string> SubTeams { get; set; } = [];

	public void OnCollisionWith(float deltaF, ICollidable target, bool applyDamage)
	{
		if (target is Entity targetEntity)
		{
			//Here we go again with the stupid american spelling
			Vector2 dirVect = (Position - targetEntity.Position).Normalized();
			float forceStrength = 100f + targetEntity.MyStats.Knockback * MyStats.KnockbackMultiplier * targetEntity.Velocity.Length();
			AccRate += dirVect * forceStrength * (1 - World.DefaultFriction);
		}
		if (applyDamage)
		{
			CurrentHealth -= target.MyStats.Damage * deltaF;
		}
	}

	public event Action DisableCollision;
	protected void RegisterInputs()
	{
		if (inputMachine is null)
		{
			inputMachine = new();
		}
		foreach (string id in inputs)
		{
			inputMachine.TryRegisterInput(id);
		}
		foreach (string id in variantInputs)
		{
			inputMachine.TryRegisterVariantInput(id);
		}
	}

	public override void _EnterTree()
	{
		if (_Velocity != Vector2.Zero && Velocity != Vector2.Zero)
		{
			throw new InvalidOperationException("Please add an object to a scene before modifying its velocity");
		}
		Velocity = _Velocity;
	}

	public override void _Ready()
	{
		CollisionLayer = 0;
		CollisionMask = 1;

		CurrentHealth = MyStats.Health;
		if (_HealthBarRef is not null)
		{
			HealthBarRef = (StatBar)_HealthBarRef;
			HealthBarRef.TargetRef = HealthRange;
		}

		TreeExiting += () => DisableCollision?.Invoke();

		if (MyStats.LifeTime > 0)
		{
			Timer deletionTimer = new()
			{
				OneShot = true,
				Autostart = true,
				WaitTime = MyStats.LifeTime
			};
			AddChild(deletionTimer);
			deletionTimer.Timeout += QueueFree;
		}

		AddToGroup(Team);
		foreach (string thisSubTeam in SubTeams)
		{
			string combinedName = Team + "_" + thisSubTeam;
			AddToGroup(combinedName);
		}

		RegisterInputs();
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		UpdateVelocity(deltaF);
	}

}
