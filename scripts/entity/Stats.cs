using Godot;
using Godot.Collections;
using System;

namespace BossRush2;

/// <summary>
/// Stats, containing permanant configurable data
/// </summary>
/// <remarks>
/// <br> Please refrain from directly changing these </br>
/// <br> A lot of these stats refer to <c>Entity</c> classes </br>
/// </remarks>
[GlobalClass]
public partial class Stats : Resource
{
	/// <summary>
	/// Amount of health points this <c>Entity</c> can deal
	/// </summary>
	[Export]
	public float Damage;

	/// <summary>
	/// This represents max health, refer to <c>CurrentHealth</c> instead if you wish to change that
	/// </summary>
	[Export]
	public float Health;

	/// <summary>
	/// This represents the terminal velocity of an entity
	/// </summary>
	/// <remarks>
	/// <br> To apply, call <c>GetAcceleration()</c> and add to <c>AccRate</c> </br>
	/// <br> (All of this is assuming that you are using an <c>Entity</c> class for this) </br>
	/// </remarks>
	[Export]
	public float Speed;

	/// <summary>
	/// Amount of <c>CurrentHealth</c> regenerated per second
	/// </summary>
	/// <remarks>
	/// WARNING: Regen is not implemented yet, remove this after it has
	/// </remarks>
	[Export]
	public float HealthRegen = 0f;

	/// <summary>
	/// The time after taking damage, before health starts to regenerate again
	/// </summary>
	/// <remarks>
	/// WARNING: Regen is not implemented yet, remove this after it has
	/// </remarks>
	[Export]
	public float RegenDelay = 0f;

	/// <summary>
	/// How much force the enemy dealing knockback will deal, dependant on velocity
	/// </summary>
	/// <remarks>
	/// <br> This maps to <c>AccRate</c> in collisions </br>
	/// </remarks>
	[Export]
	public float Knockback = 1f;

	/// <summary>
	/// Multiplier value for the knockback taken
	/// </summary>
	[Export]
	public float KnockbackMultiplier = 1f;

	/// <summary>
	/// <c>Knockback</c>, but independant of velocity
	/// </summary>
	/// <remarks>
	/// <br> This maps to <c>AccRate</c> in collisions </br>
	/// </remarks>
	[Export]
	public float ReboundForce = 100f;

	/// <summary>
	/// <br> How long after an entity spawns, that it will die </br>
	/// </summary>
	[Export]
	public float LifeTime = -1f;

	/// <summary>
	/// Whether or not these stats should be applied to projectiles shot by it too
	/// </summary>
	[Export]
	public bool DoApply = false;

	/// <summary>
	/// More niche stats are put into this dictionary
	/// </summary>
	[Export]
	public Dictionary<string, float> Extra = [];

	/// <summary>
	/// Merges this set of stats with another
	/// </summary>
	public void MergeStats(Stats source)
	{
		Damage *= source.Damage;
		Health *= source.Health;
		Speed *= source.Speed;
		HealthRegen *= source.HealthRegen;
		RegenDelay *= source.RegenDelay;
		Knockback *= source.Knockback;
		KnockbackMultiplier *= source.KnockbackMultiplier;
		ReboundForce *= source.ReboundForce;
		LifeTime *= source.LifeTime;

		foreach (var thisKey in Extra.Keys)
		{
			Extra[thisKey] *= source.Extra[thisKey];
		}
	}

	public Stats()
	{
		Damage = 1f;
		Health = 1f;
		Speed = 1f;
		HealthRegen = 0f;
		RegenDelay = 0f;
		Knockback = 1f;
		KnockbackMultiplier = 1f;
		ReboundForce = 100f;
		LifeTime = -1f;
		DoApply = false;
		Extra = [];
	}
}
