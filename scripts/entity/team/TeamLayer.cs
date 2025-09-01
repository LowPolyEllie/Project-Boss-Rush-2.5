using Godot;
using System;
using System.Collections.Generic;

namespace BossRush2;

public class TeamLayer
{
	public List<Team> teams = [];
	public string name;
	public Team GetTeam(string @name)
	{
		if (HasTeam(@name))
		{
			return teams.Find(team => team.name == @name);
		}
		return null;
	}
	public bool HasTeam(string @name)
	{
		if (teams.FindIndex(team => team.name == @name) != -1)
		{
			return true;
		}
		return false;
	}
	public bool HasTeam(Team @team)
	{
		if (teams.Contains(@team))
		{
			return true;
		}
		return false;
	}
	public void AddTeam(Team team)
	{
		teams.Add(team);
	}
	public void RemoveTeam(Team team)
	{
		teams.Remove(team);
	}
	public List<Entity> GetEntitiesInTeams(List<string> teams)
	{
		List<Entity> ret = [];
		foreach (string team in teams)
		{
			if (HasTeam(team))
			{
				ret.AddRange(GetTeam(team).members);
			}
		}
		return ret;
	}
}
