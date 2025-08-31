using Apoli;
using Apoli.States;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BossRush2;

/// <summary>
/// Base class for all game entities in Boss Rush 2.
/// </summary>
/// <remarks>
/// <br>Provides shared logic for movement, stat management, collision response, and health tracking.</br>
/// <br>Designed to simplify entity creation with default friction, stat scaling, health bar integration, and more.</br>
/// </remarks>
[GlobalClass]
public partial class Entity : Node2D, IInputMachine, IStateMachine
{
	/// <summary>
	/// The input machine of the entity. Controls everything. Override inputs and variantInputs to register mandatory inputs
	/// </summary>
	public InputMachine inputMachine;
	
	public Controller controller;
	public StateMachine stateMachine{ get; set; }
	public virtual List<string> inputs { get; set; } = [];
	public virtual List<string> variantInputs { get; set; } = [];
	/// <summary>
	/// Teams determine collision masks, collision layers, groups, and targeting queries
	/// </summary>
	public System.Collections.Generic.Dictionary<string, Team> teams = [];

	/// <summary>
	/// For setting teams in game
	/// </summary>
	[Export]
	public Godot.Collections.Dictionary<string, string> _teams { get; set; } = [];

	/// <summary>
	/// Only use for debugging purposes
	/// </summary>
	[Export]
	public Godot.Collections.Dictionary<TeamLayerWrapper, TeamWrapper> _teamsDebug { get; set; } = [];

	/// <summary>
	/// The instance of <c>Stats</c>, used for a variety of reasons
	/// </summary>
	[Export]
	public Stats stats { get; set; }

	/// <summary>
	/// Triggers all hitboxes to forget it, and may also destroy its own hitbox
	/// </summary>
	/// <remarks>
	/// <br> Use mainly when entity is being destroyed </br>
	/// </remarks>
	public event Action disableCollision;

	/// <summary>
	/// Whether the entity can independently collide
	/// </summary>
	[Export]
	public bool isTopLevel = true;
	/// <summary>
	/// The entity's owner. Meant for controllable projectiles and potentially damage reflection
	/// </summary>
	[Export]
	public Entity owner;
	/// <summary>
	/// The acceleration applied during a frame, <c>delta</c> is already accounted for, so ignore it
	/// </summary>
	protected Vector2 acceleration;

	/// <summary>
	/// <c>acceleration</c>, but for <c>angularVelocity</c> instead of <c>velocity</c>
	/// </summary>
	protected float angularAcceleration;

	/// <summary>
	/// Rotational velocity, uses radians because its rarely utilised
	/// </summary>
	[Export]
	public float angularVelocity = 0f;

	/// <summary>
	/// Amplifies or reduces the impact from World.Friction
	/// </summary>
	[Export]
	public float friction = 1f;

	/// <summary>
	/// Amplifies or reduces the impact from World.Friction
	/// </summary>
	/// <remarks>
	/// <br> Theres no rotational friction by default </br>
	/// </remarks>
	[Export]
	public float angularFriction = 0f;

	/// <summary>
	/// I regret using godot's CharacterBody2D class
	/// </summary>
	[Export]
	public Vector2 velocity = new();

	/// <summary>
	/// Returns the Acceleration, assuming the Speed stat as terminal velocity, and friction as 0.02f
	/// </summary>
	/// <returns></returns>
	public float GetAcceleration()
	{
		return stats.Speed * (1 - World.DefaultFriction);
	}

	/// <summary>
	/// Alters transformation and initial velocity based on transformation data
	/// </summary>
	public void ApplyTransform(Node2D source)
	{
		GlobalPosition += source.GlobalPosition;
		GlobalRotation += source.GlobalRotation;
		GlobalScale *= source.GlobalScale;

		velocity = velocity.Rotated(source.GlobalRotation);
	}

	/// <summary>
	/// Automatic velocity and friction updating
	/// </summary>
	public void UpdateVelocity(float deltaF)
	{
		if (friction > 0f)
		{
			velocity = ExtraMath.PredictVelocity(
			velocity, acceleration, Mathf.Pow(World.Friction, friction), deltaF
			);
		}
		else
		{
			velocity += acceleration * deltaF;
		}

		if (angularFriction > 0f)
		{
			angularVelocity = ExtraMath.PredictVelocity(
			angularVelocity, angularAcceleration, Mathf.Pow(World.Friction, angularFriction), deltaF
			);
		}
		else
		{
			angularVelocity += angularAcceleration * deltaF;
		}

		Position += velocity * deltaF;
		Rotation += angularVelocity * deltaF;
		acceleration = Vector2.Zero;
	}

	/// <summary>
	/// This represents current health, refer to <c>stats.Health</c> instead if you wish to change that
	/// </summary>
	[Export]
	public float health;
	public void OnCollisionWith(float deltaF, Entity target, bool applyDamage)
	{
		if (target is Entity targetEntity)
		{
			//Here we go again with the stupid american spelling
			Vector2 dirVect = (Position - targetEntity.Position).Normalized();
			float forceStrength = 100f + targetEntity.stats.Knockback * stats.KnockbackMultiplier * targetEntity.velocity.Length();
			acceleration += dirVect * forceStrength * (1 - World.DefaultFriction);
		}
		if (applyDamage)
		{
			health -= target.stats.Damage * deltaF;
		}
	}

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
		foreach ((TeamLayerWrapper layer, TeamWrapper team) in _teamsDebug)
		{
			JoinTeam(layer.Name, World.activeWorld.activeTeams.GetTeam(team.Name));
		}
		foreach ((string layer, string team) in _teams)
		{
			JoinTeam(layer, World.activeWorld.activeTeams.GetTeam(team));
		}
	}
	public override void _Ready()
	{
		if (owner is null && !isTopLevel)
		{
			owner = this.SearchForParent<Entity>();
		}

		health = stats.Health;

		TreeExiting += () => disableCollision?.Invoke();

		if (stats.LifeTime > 0)
		{
			Timer deletionTimer = new()
			{
				OneShot = true,
				Autostart = true,
				WaitTime = stats.LifeTime
			};
			AddChild(deletionTimer);
			deletionTimer.Timeout += QueueFree;
		}

		RegisterInputs();
	}

	public void JoinTeam(string layer, Team team)
	{
		team.AddMember(this);
		teams.TryAdd(layer,team);
	}
	public void LeaveTeam(Team team)
	{
		team.RemoveMember(this);
		teams.Remove(teams.FirstOrDefault(x=>x.Value.Equals(team)).Key);
	}
	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		UpdateVelocity(deltaF);
	}

}
