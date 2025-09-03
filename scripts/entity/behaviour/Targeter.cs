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
	PLAYER
}

public class Targeter : IBrObject
{
	private TargetMode _targetMode = TargetMode.NONE;
	public TargetMode targetMode
	{
		get => _targetMode;
		set
		{
			_targetMode = value;
			ResetTarget();
		}
	}

	/// <summary>
	/// The teams that this node will target
	/// </summary>
	public Array<string> targets = [];

	public Entity subject;
	public Target currentTarget;

	/// <summary>
	/// Finds the closest Entity to this node from a list
	/// </summary>
	public Entity FindClosestEntity(List<Entity> entityArray)
	{
		float closest = -1f;
		Entity closestEntity = null;

		foreach (var thisEntity in entityArray)
		{
			float dist = subject.GlobalPosition.DistanceSquaredTo(thisEntity.GlobalPosition);
			if (dist < closest || closest < 0f)
			{
				closest = dist;
				closestEntity = thisEntity;
			}
		}

		return closestEntity;
	}

	/// <summary>
	/// Automatically called if TargetMode changes
	/// </summary>
	public void ResetTarget()
	{
		currentTarget = targetMode switch
		{
			TargetMode.NONE => new(),
			TargetMode.OWNER => new(subject.owner),
			TargetMode.OWNER_TARGET => subject.owner.inputMachine.variantinputRegistry.Contains("Target") ? new((Vector2)subject.owner.inputMachine.GetVariantInput("Target")) : null,
			TargetMode.PLAYER => new(World.activeWorld.activePlayerController.player),
			// Disabled temporarily
			// TargetMode.NEAREST => new(FindClosestEntity(World.activeWorld.activeTeams.GetLayer("Side").GetEntitiesInTeams([.. targets.ToArray()]), subject)),
			_ => throw new FileNotFoundException("Error, YourBrain.exe is not found")
		};
	}

	//Wrapper functions
	public Vector2 GetTargetPosition()
	{
		return currentTarget.GetTargetPosition();
	}
	public Vector2? GetTargetPositionOrNull()
	{
		return currentTarget.GetTargetPositionOrNull();
	}
	/// <summary>
	/// Returns <c>defaultRot</c> if in case of emptyTarget
	/// </summary>
	public float GetTargetDirection(float defaultRot)
	{
		return currentTarget.GetTargetDirection(subject.GlobalPosition,defaultRot);
	}
}
public class Target : IBrObject
{
	Node2D TargetEntity;
	Vector2 TargetPosition = new();
	bool emptyTarget = true;
	public Target() { }
	public Target(Node2D _TargetEntity)
	{
		TargetEntity = _TargetEntity;
		emptyTarget = false;
	}
	public Target(Vector2 _TargetPosition)
	{
		TargetPosition = _TargetPosition;
		emptyTarget = false;
	}
	public Vector2 GetTargetPosition()
	{
		if (TargetEntity is not null)
		{
			return TargetEntity.GlobalPosition;
		}
		return TargetPosition;
	}
	public Vector2? GetTargetPositionOrNull()
	{
		if (emptyTarget)
		{
			return null;
		}
		else
		{
			if (TargetEntity is not null)
			{
				return TargetEntity.GlobalPosition;
			}
			return TargetPosition;
		}

	}
	public float GetTargetDirection(Vector2 subject, float defaultRot)
	{
		if (emptyTarget)
		{
			return defaultRot;
		}
		else
		{
			Vector2 targPos = GetTargetPosition();
			return subject.AngleToPoint(targPos);
		}
	}
}