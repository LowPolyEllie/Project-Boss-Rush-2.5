using Godot;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// The top most node in the main scene 
/// </summary>
/// <remarks>
/// <para> Most of its data is static for easy access </para>
/// <para> Signals are queued due to static limitations, and sent during _Process() </para>
/// <para> Data must be configured here for now, I will make making a proper json later </para>
/// </remarks>
[GlobalClass]
public partial class World : Node
{
	//Built in nodes

	public static World activeWorld { get; set; }
	private Camera _activeCamera;
	public Camera activeCamera
	{
		get
		{
			return _activeCamera;
		}
		set
		{
			_activeCamera.Enabled = false;

			value.Enabled = true;
			value.LimitLeft = -(int)worldSize.X;
			value.LimitRight = (int)worldSize.X;
			value.LimitTop = -(int)worldSize.Y;
			value.LimitBottom = (int)worldSize.Y;
			
			_activeCamera = value;
		}
	}
	public PlayerController activePlayerController { get; set; }

	[Export]
	public PolygonSpawner activePolygonSpawner { get; set; }
	[Export]
	public ProjectileSpawner activeProjectileSpawner { get; set; }
	[Export]
	public Node teamRegistry { get; set; }
	public TeamLayerCollection activeTeams = new();
	public TeamLayer allTeams = new();

	/// <summary>
	/// Amount of velocity in ratio remaining after a second
	/// </summary>
	public static float Friction { get; set; } = 0.01f;

	/// <summary>
	/// Friction assigned upon setup
	/// </summary>
	public static float DefaultFriction { get; set; }

	/// <summary>
	/// Size that determines limit for grid rendering and camera scroll
	/// </summary>
	/// <remarks>
	/// Boundaries for player/entity movement should be manually done per level
	/// </remarks>
	public static Vector2 worldSize { get; set; } = new(1000f, 1000f);

	//Signal related stuff here

	/// <summary>
	/// Called specifically after World is initialised
	/// </summary>
	[Signal]
	public delegate void InitSetupEventHandler();

	//Main setup, called in _EnterTree() instead of _Ready() because it needs to be available as early as possible
	public override void _EnterTree()
	{
		activeWorld = this;

		//Initialising stuff
		DefaultFriction = Friction;
		GD.Randomize(); //Seriously? Randomize with a Z???

		//What a mess
		List<TeamWrapper> allWrappers = [];
		foreach (TeamLayerWrapper layer in teamRegistry.GetChildren())
		{
			TeamLayer _layer = new();
			_layer.name = layer.Name;
			foreach (TeamWrapper team in layer.GetChildren())
			{
				Team _team = new();
				_team.name = team.Name;
				_team.collisionLayer = team.collisionLayer;
				_layer.AddTeam(_team);
				allTeams.AddTeam(_team);
				if (!allWrappers.Contains(team))
				{
					allWrappers.Add(team);
				}
			}
			activeTeams.AddLayer(_layer);
		}
		foreach (TeamWrapper team in allWrappers)
		{

			foreach (TeamWrapper maskTeam in team.collisionMask)
			{
				allTeams.GetTeam(team.Name).collisionMask.Add(allTeams.GetTeam(maskTeam.Name));
			}
		}
	}

	//Secondary initialisation, called after every other node's _Ready() has been called
	public override void _Ready()
	{
		EmitSignal(SignalName.InitSetup);
	}
}
