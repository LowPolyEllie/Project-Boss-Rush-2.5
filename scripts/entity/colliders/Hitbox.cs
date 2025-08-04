using Godot;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// In game non wall hitbox
/// </summary>
[GlobalClass]
public partial class Hitbox : Area2D
{
	/// <summary>
	/// Currently colliding entities with the hitbox
	/// </summary>
	protected List<ICollidable> CollidingEntities = [];

	/// <summary>
	/// Because godot doesn't allow referencing custom types
	/// </summary>
	[Export]
	Node2D _Source;
	[Export]
	Node2D _AbsoluteSource;

	/// <summary>
	/// Can detect and act upon (without damaging) teamates with this value set to true as well
	/// </summary>
	[Export]
	public bool AntiCram;

	/// <summary>
	/// The node that this hitbox belongs to
	/// </summary>
	public ICollidable Source;

	/// <summary>
	/// The root Entity of this hierarchy, defaults to Source if initialised as null
	/// </summary>
	public Entity AbsoluteSource;

	public override void _Ready()
	{
		//Connecting signals
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		//Setting up Source, more or less just a validation check
		if (Source is null)
		{
			if (_Source is not ICollidable collidable)
				throw new InvalidCastException("Must reference ICollidable source only");

			Source = collidable;
		}

		//Setting up AbsoluteSource 
		//If its initialisation value is null, it uses Source, which has to be Entity in that case
		if (AbsoluteSource is null)
		{
			if (_AbsoluteSource is null)
			{
				if (Source is not Entity sourceEntity)
					throw new InvalidCastException("Must reference Entity source only (auto initialisation used)");

				AbsoluteSource = sourceEntity;
			}
			else
			{
				if (_AbsoluteSource is not Entity absEntity)
					throw new InvalidCastException("Must reference Entity source only");

				AbsoluteSource = absEntity;
			}
		}

		CollisionLayer = 0;
		CollisionMask = 0;

		string team = Source.Team;
		var subTeams = Source.SubTeams;

		CollisionLayer += World.TeamCollisionLayers[team];
		CollisionMask += World.TeamCollisionMasks[team];

		if (AntiCram)
		{
			CollisionLayer += 1 << 4;
			CollisionMask += 1 << 4;
		}

		foreach (string thisSubTeam in subTeams)
		{
			string combinedName = team + "_" + thisSubTeam;
			AddToGroup(combinedName);

			if (World.TeamCollisionLayers.TryGetValue(combinedName, out uint layer))
			{
				CollisionLayer += layer;
			}
			if (World.TeamCollisionMasks.TryGetValue(combinedName, out uint mask))
			{
				CollisionMask += mask;
			}
		}
	}

	protected void OnAreaEntered(Area2D area)
	{
		if (area is Hitbox hitbox)
		{
			var entity = hitbox.Source;
			CollidingEntities.Add(entity);
			entity.DisableCollision += () => CollidingEntities.Remove(entity);
		}
	}

	protected void OnAreaExited(Area2D area)
	{
		if (area is Hitbox hitbox)
		{
			var entity = hitbox.Source;
			CollidingEntities.Remove(entity);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		foreach (var thisCollider in CollidingEntities)
		{
			Source.OnCollisionWith(deltaF, thisCollider, thisCollider.Team != Source.Team);
		}
	}
}
