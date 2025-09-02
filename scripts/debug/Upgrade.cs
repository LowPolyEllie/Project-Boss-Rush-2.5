using Godot;
using System;

namespace BossRush2.Debug;

public partial class Upgrade : Button
{
	public override void _Ready()
	{
		World.activeWorld.InitSetup += Init;
	}

	public void Init()
	{
		Pressed += World.activeWorld.activePlayerUpgrader.UpgradeTank;
	}
}
