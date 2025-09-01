using Godot;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// In game non wall hitbox
/// </summary>
[GlobalClass]
public partial class Hitbox : Area2D, IBrObject, IEntitySegment
{
	/// <summary>
	/// Currently colliding entities with the hitbox
	/// </summary>
	protected List<Entity> collidingEntities = [];

	/// <summary>
	/// Can detect and act upon (without damaging) teamates with this value set to true as well
	/// </summary>
	[Export]
	public bool antiCram;

	/// <summary>
	/// Whatever Entity that uses this hitbox
	/// </summary>
	[Export]
	public Entity owner { get; set; }
	/// <summary>
	/// Always false, just there for information
	/// </summary>
	public bool isTopLevel { get; set; } = false; 

	public override void _Ready()
	{
		owner ??= this.SearchForParent<Entity>();

		//Connecting signals
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		CollisionLayer = 0;
		CollisionMask = 0;

		CollisionLayer += (uint)1 << owner.teams["Side"].collisionLayer;
		foreach (Team team in owner.teams["Side"].collisionMask)
		{
			CollisionMask += (uint)1 << team.collisionLayer;
		}

		if (antiCram)
		{
			CollisionLayer += 1 << 4;
			CollisionMask += 1 << 4;
		}
	}

	protected void OnAreaEntered(Area2D area)
	{
		if (area is Hitbox hitbox)
		{
			var entity = hitbox.owner;
			collidingEntities.Add(entity);
			entity.disableCollision += () => collidingEntities.Remove(entity);
		}
	}

	protected void OnAreaExited(Area2D area)
	{
		if (area is Hitbox hitbox)
		{
			var entity = hitbox.owner;
			collidingEntities.Remove(entity);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		foreach (Entity entity in collidingEntities)
		{
			owner.OnCollisionWith(deltaF, entity, owner.teams["Side"].Equals(entity.teams["Side"]));
		}
	}
}
