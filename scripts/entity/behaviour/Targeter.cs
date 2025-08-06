using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;

namespace BossRush2;

/// <summary>
/// A Node2D with a built in targeting system
/// </summary>
[GlobalClass]
public partial class Targeter : Resource
{
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
		/// Target the mouse pointer
		/// </summary>
		OWNER_TARGET,

		/// <summary>
		/// Target the nearest entity in one of the opposing teams
		/// </summary>
		NEAREST
	}

	[Export]
	public TargetMode MyTargetMode = TargetMode.NONE;

	/// <summary>
	/// The teams that this node will target
	/// </summary>
	[Export]
	public Array<string> Targets = [];

	public Target CurrentTarget;

	/// <summary>
	/// Finds the closest Entity to this node from a list
	/// </summary>
	public Entity FindClosestEntity(List<Entity> entityArray, Entity source)
	{
		float closest = -1f;
		Entity closestEntity = null;

		foreach (var thisEntity in entityArray)
		{
			float dist = source.GlobalPosition.DistanceSquaredTo(thisEntity.GlobalPosition);
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
		CurrentTarget = MyTargetMode switch
		{
			TargetMode.NONE => null,
			TargetMode.OWNER => new(entity.Owner),
			TargetMode.OWNER_TARGET => entity.Owner.inputMachine.VariantInputRegistry.Contains("Target")?new((Vector2)entity.Owner.inputMachine.GetVariantInput("Target")):null,
			TargetMode.NEAREST => new(FindClosestEntity(entity.GetAllTeamMembers(Targets),entity)),
			_ => throw new FileNotFoundException("Error, YourBrain.exe is not found")
		};
	}
}
public class Target
{
	Node2D TargetEntity;
	Vector2 TargetPosition;
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