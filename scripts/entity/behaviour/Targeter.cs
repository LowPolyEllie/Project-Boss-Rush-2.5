using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BossRush2;

/// <summary>
/// To be utilised by other functions here
/// </summary>
public enum TargetMode
{
	/// <summary>
	/// Placeholder for when targeting isn't used
	/// </summary>
	NONE,

	/// <summary>
	/// Target the player
	/// </summary>
	OWNER,

	/// <summary>
	/// Target what the owner of this node is targeting
	/// </summary>
	OWNER_TARGET,

	/// <summary>
	/// Target the nearest entity in one of the opposing teams
	/// </summary>
	NEAREST
}

public class Targeter : IBrObject
{

	public TargetMode targetMode = TargetMode.NONE;

	/// <summary>
	/// The teams that this node will target
	/// </summary>
	public Array<string> targets = [];

	public Target currentTarget;

	/// <summary>
	/// Finds the closest Entity to this node from a list
	/// </summary>
	public Entity FindClosestEntity(List<Entity> entityArray, Entity entity)
	{
		float closest = -1f;
		Entity closestEntity = null;

		foreach (var thisEntity in entityArray)
		{
			float dist = entity.GlobalPosition.DistanceSquaredTo(thisEntity.GlobalPosition);
			if (dist < closest || closest < 0f)
			{
				closest = dist;
				closestEntity = thisEntity;
			}
		}

		return closestEntity;
	}

	public void ResetTarget(Entity entity)
	{
		currentTarget = targetMode switch
		{
			TargetMode.NONE => new(),
			TargetMode.OWNER => new(entity.owner),
			TargetMode.OWNER_TARGET => entity.owner.inputMachine.variantinputRegistry.Contains("Target") ? new((Vector2)entity.owner.inputMachine.GetVariantInput("Target")) : new(),
			TargetMode.NEAREST => new(FindClosestEntity(World.activeWorld.activeTeams.GetEntitiesInLayers([.. targets.ToArray()]), entity)),
			_ => throw new FileNotFoundException("Error, YourBrain.exe is not found")
		};
	}
}
public class Target
{
	Node2D TargetEntity;
	Vector2 TargetPosition = new();
	public Target()
	{
		
	 }
	public Target(Node2D _TargetEntity)
	{
		TargetEntity = _TargetEntity;
	}
	public Target(Vector2 _TargetPosition)
	{
		TargetPosition = _TargetPosition;
	}
	public Vector2 GetTargetPosition()
	{
		if (TargetEntity is not null)
		{
			return TargetEntity.GlobalPosition;
		}
		return TargetPosition;
	}
}