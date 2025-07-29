using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Self explanatory, use when you need the global mouse position wrapped into a <c>Node2D</c>
/// </summary>
/// <remarks>
/// Access through <c>World.MouseTrackerMain</c>
/// </remarks>
public partial class MouseTracker : Node2D
{
	public override void _Process(double delta)
	{
		GlobalPosition = GetGlobalMousePosition();
	}
}
