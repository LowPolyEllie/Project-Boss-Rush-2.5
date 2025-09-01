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
public partial class Entity : EntitySegment, IInputMachine, IStateMachine
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
	/// This represents current health, refer to <c>stats.Health</c> instead for max health
	/// </summary>
	[Export]
	public float health;

	/// <summary>
	/// If true, pass the damage it takes over to its owner
	/// </summary>
	/// <remarks>
	/// <br> An Entity can still be top level while doing this </br>
	[Export]
	public bool carryDamage = false;

	/// <summary>
	/// Called when this Entity collides with another Entity
	/// </summary>
	/// <remarks>
	/// <br> Collisions will occur more than once if an Entity has multiple hitboxes </br>
	/// <br> This is by design to make tanks with piercing abilities stand out more </br>
	/// <br> If an Entity is not top level, the force applied is passed over to its owner </br>
	/// </remarks>
	public void OnCollisionWith(float deltaF, Entity target, bool applyDamage)
	{
		if (isTopLevel)
		{
			//Here we go again with the stupid american spelling
			Vector2 dirVect = (Position - target.Position).Normalized();
			float forceStrength = 100f + target.stats.Knockback * stats.KnockbackMultiplier * target.velocity.Length();
			acceleration += dirVect * forceStrength * (1 - World.DefaultFriction);
		}
		else
		{
			owner.OnCollisionWith(deltaF, target, false);
		}
		
		if (applyDamage)
		{
			float damageTaken = target.stats.Damage;
			if (carryDamage)
			{
				owner.health -= damageTaken;
			}
			else
			{
				health -= damageTaken;
			}
		}
	}

	/// <summary>
	/// Registers the input types
	/// </summary>
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

	/// <summary>
	/// General formatting convention for <c>Entity</c> objects: _EnterTree() is typically used for joining Teams
	/// </summary>
	public override void _EnterTree()
	{
		if (teams.Count == 0 && _teams.Count == 0 && _teamsDebug.Count == 0)
		{
			_teams = owner._teams;
			_teamsDebug = owner._teamsDebug;
		}
		foreach ((TeamLayerWrapper layer, TeamWrapper team) in _teamsDebug)
		{
			JoinTeam(layer.Name, World.activeWorld.activeTeams.GetTeam(team.Name));
		}
		foreach ((string layer, string team) in _teams)
		{
			JoinTeam(layer, World.activeWorld.activeTeams.GetTeam(team));
		}
	}

	/// <summary>
	/// Please avoid doing any Team related setup in _Ready(), use that for 
	/// </summary>
	public override void _Ready()
	{
		FindOwner();

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
