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
public partial class Targeter : Node2D
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
		PLAYER,

		/// <summary>
		/// Target the mouse pointer
		/// </summary>
		MOUSE,

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

	public Node2D CurrentTarget;

	/// <summary>
	/// Finds the closest Entity to this node from a list
	/// </summary>
	public Entity FindClosestEntity(List<Entity> entityArray)
	{
		float closest = -1f;
		Entity closestEntity = null;

		foreach (var thisEntity in entityArray)
		{
			float dist = GlobalPosition.DistanceSquaredTo(thisEntity.GlobalPosition);
			if (dist < closest || closest < 0f)
			{
				closest = dist;
				closestEntity = thisEntity;
			}
		}

		return closestEntity;
	}

	public void ResetTarget()
	{
		CurrentTarget = MyTargetMode switch
		{
			TargetMode.NONE => null,
			TargetMode.PLAYER => World.PlayerMain,
			TargetMode.MOUSE => World.MouseTrackerMain,
			TargetMode.NEAREST => FindClosestEntity(this.GetAllTeamMembers(Targets)),
			_ => throw new FileNotFoundException("Error, YourBrain.exe is not found"),
		};
	}
}
