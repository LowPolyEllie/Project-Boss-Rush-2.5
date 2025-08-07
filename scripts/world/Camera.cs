using Godot;
using System;

namespace BossRush2;

/// <summary>
/// The main <c>Camera2D</c> for the game, this is configured to be able to track and follow any node
/// </summary>
/// <remarks>
/// <br> This node must be directly under <c>World</c> with the same name as the class name </br>
/// <br> One of the few hardcoded nodes, for the purpose of static access </br>
/// </remarks>
[GlobalClass]
public partial class Camera : Camera2D
{
	/// <summary>
	/// The main entity that the camera will follow
	/// </sumary>
	public Target target { get; set; } = new();

	/// <summary>
	/// Use instead of Enabled. Never disable directly, or it WILL null activeCamera
	/// </summary>
	
	[Export]
	public bool Active
	{
		get
		{
			return Enabled;
		}
		set
		{
			Enabled = value;
			if (value)
			{
				World.activeWorld.activeCamera = this;
			}
			else
			{ 
				World.activeWorld.activeCamera = null;
			}
		}
	}
	/// <summary>
	/// Position offset from the target entity
	/// </summary>
	[Export]
	public Vector2 PositionOffset { get; set; }

	public override void _Process(double delta)
	{
		Position = target.GetTargetPosition() + PositionOffset;
	}
	public override void _Ready()
	{
		LimitLeft = -(int)World.worldSize.X;
		LimitRight = (int)World.worldSize.X;
		LimitTop = -(int)World.worldSize.Y;
		LimitBottom = (int)World.worldSize.Y;
	}
}
