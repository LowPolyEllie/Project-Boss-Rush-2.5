using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// Extension method container for team related utility
/// </summary>
public static class TeamFunctions
{
	/// <summary>
	/// Gets all team members in select teams and subteams
	/// </summary>
	/// <remarks>
	/// <br> This may lag a shit ton, but I'll just hope it doesn't </br>
	/// </remarks>
	public static List<Entity> GetAllTeamMembers(this Node subjectEntity, Array<string> teams)
	{
		SceneTree targetTree = subjectEntity.GetTree();
		List<Entity> toReturn = [];
		foreach (string thisTeam in teams)
		{
			toReturn.AddRange(targetTree.GetNodesInGroup(thisTeam).FilterNodeByType<Entity>());
		}
		return toReturn;
	}
}
