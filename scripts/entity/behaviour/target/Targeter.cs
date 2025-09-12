using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.IO;

namespace BossRush2;

// Meow, Resource and Node scripts need an isolated .cs file for some reason
// I moved the rest to ExtraTargeter.cs
[GlobalClass]
public partial class Targeter : EntitySegment, IBrObject
{
	[Export]
	private TargetMode _targetMode = TargetMode.NONE;
	public TargetMode targetMode
	{
		get => _targetMode;
		set
		{
			if (value != _targetMode)
			{
				_targetMode = value;
				ResetTarget();
			}

		}
	}

	[Export]
	/// <summary>
	/// The teams that this node will target
	/// </summary>
	public Array<TeamTarget> targets = [];
	public Target currentTarget = new();

	/// <summary>
	/// Within this range, the turret will lock in on this Target
	/// </summary>
	[Export]
	public float lockDist = 750f;

	/// <summary>
	/// Automatically called if TargetMode changes
	/// </summary>
	public void ResetTarget()
	{
		switch (targetMode)
		{
			case TargetMode.NONE:
				currentTarget = new();
				break;
			case TargetMode.OWNER:
				currentTarget = new(owner.owner);
				break;
			case TargetMode.OWNER_TARGET:
				currentTarget =
					owner.owner is ITargetable targetable ?
					targetable.targeter.currentTarget : new();
				break;
			case TargetMode.NEAREST:
				float closestDist = -1f;
				Entity closestEntity = null;
				var activeTeams = World.activeWorld.activeTeams;
				foreach (var thisTeamGroup in targets)
				{
					foreach (var thisTeam in thisTeamGroup.teamData)
					{
						foreach (
							var thisEntity in
							activeTeams.GetLayer(thisTeam.Key).GetTeam(thisTeam.Value).members
						)
						{
							float distSquared =
								thisEntity.GlobalPosition.DistanceSquaredTo(GlobalPosition)
								* thisTeamGroup.distanceMultiplier;
							if (
								closestDist < 0 ||
								distSquared < closestDist
							)
							{
								closestDist = distSquared;
								closestEntity = thisEntity;
							}
						}
					}
				}
				if (closestDist < 0)
				{
					currentTarget = new();
				}
				else
				{
					currentTarget = new(closestEntity);
				}

				break;
			case TargetMode.MOUSE:
				currentTarget = new(World.activeWorld.activeMouseTracker);
				break;
		}
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
		return currentTarget.GetTargetDirection(GlobalPosition, defaultRot);
	}

	public override void _Ready()
	{
		FindOwner();
	}

	public override void _Process(double delta)
	{
		if (
			(currentTarget.emptyTarget && targetMode != TargetMode.NONE) ||
			(
				targetMode == TargetMode.NEAREST ||
				GlobalPosition.DistanceSquaredTo(currentTarget.GetTargetPosition()) > lockDist * lockDist
			)
		)
		{
			ResetTarget();
		}
    }
}
