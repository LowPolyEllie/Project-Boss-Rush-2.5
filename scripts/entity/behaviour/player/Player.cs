using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// The class for the main player
/// </summary>
/// <remarks>
/// The properties here are persistent, they're not tank specific
/// </remarks>
[GlobalClass]
public partial class Player : Basic
{
	public override List<string> inputs
	{ get; set; } = [
		"Up", "Down", "Left", "Right", "Fire", "Fire2", //From Basic
		"Upgrade", "Downgrade"
	];

	/// <summary>
	/// The current loadout, being used by the Player
	/// </summary>
	[Export]
	public Array<TankLoader> currentLoadout;
	/// <summary>
	/// Note the zero indexing
	/// </summary>
	[Export]
	public int currentTier = 0;

	/// <summary>
	/// Moves a tank up a tier
	/// </summary>
	public void UpgradeTank()
	{
		currentTier++;
		if (currentTier > currentLoadout.Count - 1)
		{
			currentTier = currentLoadout.Count - 1;
			throw new InvalidOperationException("UpgradeTank() used when tank is at max tier");
		}
		else
		{
			currentLoadout[currentTier].LoadTank(this);
		}
	}

	/// <summary>
	/// Moves a tank down a tier
	/// </summary>
	public void DowngradeTank()
	{
		currentTier--;
		if (currentTier < 0)
		{
			currentTier = 0;
			throw new InvalidOperationException("DowngradeTank() used when tank is at tier 0");
		}
		else
		{
			currentLoadout[currentTier].LoadTank(this);
		}
	}

	/// <summary>
	/// Sets the tank to tier 0
	/// </summary>
	public void ResetTier()
	{
		currentTier = 0;
		currentLoadout[currentTier].LoadTank(this);
	}

	/// <summary>
	/// Sets the tank to a given tier
	/// </summary>
	public void SetTier(int tier)
	{
		currentTier = tier;
		currentLoadout[currentTier].LoadTank(this);
	}
}
