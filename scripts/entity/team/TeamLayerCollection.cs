using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BossRush2;

public class TeamLayerCollection
{
	public List<TeamLayer> teamLayers = [];
	public bool HasTeam(string name)
	{
		if (teamLayers.FindIndex(layer => layer.HasTeam(name)) != -1)
		{
			return true;
		}
		return false;
	}
	public Team GetTeam(string name)
	{
		foreach (TeamLayer layer in teamLayers)
		{
			if (layer.HasTeam(name))
			{
				return layer.GetTeam(name);
			}
		}
		return null;
	}
	public bool HasLayer(string name)
	{
		if (teamLayers.FindIndex(layer => layer.name == name) != -1)
		{
			return true;
		}
		return false;
	}
	public TeamLayer GetLayer(string name)
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
	//Singular team constructor
	public TeamLayerCollection()
	{
		
	}
	public TeamLayerCollection(string layer, Team team)
	{
		teamLayers = [
			new(){
				name = layer,
				teams = [
					team
				]
			}
		];
	}
	public void AddLayer(TeamLayer layer)
	{
		teamLayers.Add(layer);
	}
	public TeamLayer FindAddLayer(string layer)
	{
		if (GetLayer(layer) is null)
		{
			AddLayer(new()
			{ 
				name = layer
			});
		}
		return GetLayer(layer);
	}
}
