using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Drones controlled by the player
/// </summary>
[GlobalClass]
public partial class PlayerDrone : Drone
{
	public override void _Process(double delta)
	{
		MyTargeter.MyTargetMode =
			Input.IsActionPressed("firePrimary") ?
			Targeter.TargetMode.MOUSE : Targeter.TargetMode.ACTIVE_PLAYER;
	}
}
