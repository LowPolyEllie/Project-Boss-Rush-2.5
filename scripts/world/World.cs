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
	public TeamLayerCollection activeTeamLayers;

	[Export]
	public PolygonSpawner activePolygonSpawner { get; set; }
	[Export]
	public ProjectileSpawner activeProjectileSpawner { get; set; }

	//Cached border shapes, ignore this
	static float boundaryWidth = 500f;
	static CollisionShape2D leftBoundary, rightBoundary, topBoundary, bottomBoundary;
	static RectangleShape2D horizontalHitbox, verticalHitbox;

	/// <summary>
	/// Amount of Velocity in ratio remaining after a second
	/// </summary>
	public static float Friction { get; set; } = 0.01f;

	/// <summary>
	/// Friction assigned upon setup
	/// </summary>
	public static float DefaultFriction { get; set; }

	public static Vector2 worldSize = new(1000f, 1000f);
	/// <summary>
	/// <br> The boundaries for Player movement and Camera scrolling </br>
	/// <br> No, this won't show in the inspector because Godot is being a bitch </br>
	/// </summary>
	public static Vector2 WorldSize
	{
		get => worldSize;
		set
		{
			worldSize = value;
			//Boundary Hitboxes
			horizontalHitbox.Size = new Vector2(
				2 * (worldSize.Y + boundaryWidth), boundaryWidth
			);

			verticalHitbox.Size = new Vector2(
				boundaryWidth, 2 * (worldSize.X + boundaryWidth)
			);

			//Boundary Positioning
			leftBoundary.Position = new Vector2(
				-(worldSize.X + boundaryWidth / 2),
				0f
			);
			rightBoundary.Position = new Vector2(
				worldSize.X + boundaryWidth / 2,
				0f
			);
			topBoundary.Position = new Vector2(
				0f,
				-(worldSize.Y + boundaryWidth / 2)
			);
			bottomBoundary.Position = new Vector2(
				0f,
				worldSize.Y + boundaryWidth / 2
			);
		}
	}

	/// <summary>
	/// The team collision layers, one which every member will be in
	/// </summary>
	public static readonly Dictionary<string, uint> TeamCollisionLayers = new()
	{
		["playerTeam"] = 1 << 1,
		["bossTeam"] = 1 << 2,
		["polygonTeam"] = 1 << 3
	};

	/// <summary>
	/// The team collision masks, one which every member will react to
	/// </summary>
	public static readonly Dictionary<string, uint> TeamCollisionMasks = new()
	{
		["playerTeam"] = (1 << 2) + (1 << 3),
		["bossTeam"] = (1 << 1) + (1 << 3),
		["polygonTeam"] = (1 << 1) + (1 << 2) + (1 << 3),
	};

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

		//Caching data
		var WorldBoundaries = GetNode<StaticBody2D>("WorldBoundaries");
		leftBoundary = WorldBoundaries.GetNode<CollisionShape2D>("LeftHitbox");
		rightBoundary = WorldBoundaries.GetNode<CollisionShape2D>("RightHitbox");
		topBoundary = WorldBoundaries.GetNode<CollisionShape2D>("TopHitbox");
		bottomBoundary = WorldBoundaries.GetNode<CollisionShape2D>("BottomHitbox");
		horizontalHitbox = (RectangleShape2D)topBoundary.Shape;
		verticalHitbox = (RectangleShape2D)leftBoundary.Shape;

		//Initialising stuff
		DefaultFriction = Friction;
		GD.Randomize(); //Seriously? Randomize with a Z???
	}

	//Secondary initialisation, called after every other node's _Ready() has been called
	public override void _Ready()
	{
		EmitSignal(SignalName.InitSetup);
	}
}
