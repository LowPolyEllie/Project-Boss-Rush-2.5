using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// Implementation of <c>EntitySpawner</c> for most of the in game projectiles
/// </summary>
public partial class ProjSpawner : EntitySpawner
{
	/// <summary>
	/// Loads a packedscene and applies it to a source, automatically handling transformation, velocity and stats
	/// </summary>
	public Entity Shoot(PackedScene packed, Node2D source, Stats sourceStats, int zindex)
	{
		var toShoot = (Entity)packed.Instantiate();
		Spawn(toShoot, zindex);

		toShoot.ApplyTransform(source);
		if (sourceStats.DoApply)
		{
			toShoot.MyStats.MergeStats(sourceStats);
			toShoot.Velocity *= sourceStats.Speed;
		}

		return toShoot;
	}
}
