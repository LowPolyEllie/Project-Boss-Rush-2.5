using Godot;
using System;
using System.Collections.Generic;

namespace BossRush2;

public class TeamLayer
{
	public List<Team> teams;
	public string name;
	public Team getTeam(string @name)
	{
		if (hasTeam(@name))
		{
			return teams.Find(team => team.name == @name);
		}
		return null;
	}
	public bool hasTeam(string @name)
	{
		if (teams.FindIndex(team => team.name == @name) != -1)
		{
			return true;
		}
		return false;
	}
	public bool hasTeam(Team @team)
	{
		if (teams.Contains(@team))
		{
			return true;
		}
		return false;
	}
	public bool hasSameTeam(TeamLayer layer)
	{
		foreach (Team team in teams)
		{
			if (layer.hasTeam(team))
			{
				return true;
			}
		}
		return false;
	}
}
