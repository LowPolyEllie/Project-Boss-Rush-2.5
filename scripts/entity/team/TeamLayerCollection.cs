using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BossRush2;

public class TeamLayerCollection : List<TeamLayer>
{
	public List<TeamLayer> teamLayers;
	public bool hasTeam(string name)
	{
		if (teamLayers.FindIndex(layer => layer.hasTeam(name)) != -1)
		{
			return true;
		}
		return false;
	}
	public Team getTeam(string name)
	{
		foreach (TeamLayer layer in teamLayers)
		{
			if (layer.hasTeam(name))
			{
				return layer.getTeam(name);
			}
		}
		return null;
	}
	public bool hasLayer(string name)
	{
		if (teamLayers.FindIndex(layer => layer.name == name) != -1)
		{
			return true;
		}
		return false;
	}
	public TeamLayer getLayer(string name)
	{
		foreach (TeamLayer layer in teamLayers)
		{
			if (layer.name == name)
			{
				return layer;
			}
		}
		return null;
	}
	public List<Entity> getEntitiesInLayers(List<string> teams)
	{
		List<Entity> ret = [];
		foreach (string team in teams)
		{
			if (hasTeam(team))
			{
				ret.AddRange(getTeam(team).members);
			}
		}
		return ret;
	}
	//Singular team constructor
	public TeamLayerCollection(string team)
	{
		teamLayers = [
			new(){
				name = "Side",
				teams = [
					new(){ 
						name = "Polygon"
					}
				]
			}
		];
	}
}
