using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// Implementation of <c>EntitySpawner</c> for most of the in game projectiles
/// </summary>
[GlobalClass]
public partial class ProjectileSpawner : EntitySpawner
{
	/// <summary>
	/// Loads a packedscene and applies it to a source, automatically handling transformation, velocity and stats
	/// </summary>
	public Entity Shoot(PackedScene packed, Node2D source, Stats sourceStats, int zindex, Entity owner)
	{
		var toShoot = (Entity)packed.Instantiate();
		toShoot.owner = owner;
		Spawn(toShoot, zindex);

		toShoot.ApplyTransform(source);
		if (sourceStats.DoApply)
		{
			toShoot.stats.MergeStats(sourceStats);
			toShoot.velocity *= sourceStats.Speed;
		}

		return toShoot;
	}
}
