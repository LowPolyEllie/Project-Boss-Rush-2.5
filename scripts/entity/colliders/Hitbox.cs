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
	protected List<Entity> collidingEntities = [];

	/// <summary>
	/// Can detect and act upon (without damaging) teamates with this value set to true as well
	/// </summary>
	[Export]
	public bool antiCram;

	/// <summary>
	/// The node that this hitbox belongs to
	/// </summary>
	[Export]
	public Entity owner;
	public override void _Ready()
	{
		//Connecting signals
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		CollisionLayer = 0;
		CollisionMask = 0;

		/*CollisionLayer += World.TeamCollisionLayers[team];
		CollisionMask += World.TeamCollisionMasks[team];

		if (antiCram)
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
		}*/
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
			owner.OnCollisionWith(deltaF, entity, owner.teams.getLayer("Side").hasSameTeam(entity.teams.getLayer("Side")));
		}
	}
}
