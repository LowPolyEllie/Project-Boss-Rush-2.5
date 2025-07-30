using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Drones controlled by the player
/// </summary>
[GlobalClass]
public partial class PlayerMinion : Minion
{
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("firePrimary"))
		{
			MyTargeter.MyTargetMode = Targeter.TargetMode.MOUSE;
			IgnoreFollowLimit = false;
			UseMinionAccuracy = true;
		}
		else
		{
			MyTargeter.MyTargetMode = Targeter.TargetMode.PLAYER;
			IgnoreFollowLimit = true;
			UseMinionAccuracy = false;
		}
		
	}
}
