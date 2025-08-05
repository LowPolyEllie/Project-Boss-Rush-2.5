using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Loads a <c>PackedScene</c> to shoot as a projectile
/// </summary>
[GlobalClass]
public partial class Cannon : Node2D
{
	[Export]
	public Stats MyStats;

	/// <summary>
	/// The time it takes after the bullet is fired for the cannon to shoot again
	/// </summary>
	[Export]
	public float FireRate = 0.5f;

	/// <summary>
	/// The maximum number of projectiles this cannon can shoot, set to -1f for no limit
	/// </summary>
	[Export]
	public float ProjCount = -1f;
	protected float projTracker = 0f;

	/// <summary>
	/// InitDelay is ignored if true
	/// </summary>
	[Export]
	public bool HaveDelay = false;

	/// <summary>
	/// The time is takes after input is pressed before bullet is fired
	/// </summary>
	[Export]
	public float InitDelay;

	/// <summary>
	/// Because godot doesn't allow referencing custom types
	/// </summary>
	[Export]
	Node2D _Source;

	public Entity Source;

	/// <summary>
	/// The scene to instantiate once the bullet fires
	/// </summary>
	[Export]
	public PackedScene ToShoot;

	/// <summary>
	/// Automatically fire without any input needed
	/// </summary>
	[Export]
	public bool AutoFire;

	/// <summary>
	/// Whether or not the cooldown or delay timer is active
	/// </summary>
	public bool OnCooldown;
	/// <summary>
	/// Whether or not the delay timer is active
	/// </summary>
	public bool OnDelay;
	public bool InputFiring;

	/// <summary>
	/// Timers are created at runtime for abstraction purposes
	/// </summary>
	protected Timer ShootTimer, DelayTimer;

	public override void _Ready()
	{
		if (Source is null)
		{
			if (_Source is Entity entity)
			{
				Source = entity;
			}
			else
			{
				throw new InvalidCastException("Must reference Entity type source only");
			}
		}

		ShootTimer = new Timer()
		{
			OneShot = true,
			WaitTime = FireRate,
		};
		if (HaveDelay)
		{
			DelayTimer = new Timer()
			{
				OneShot = true,
				WaitTime = InitDelay
			};
			DelayTimer.Timeout += OnShoot;
			AddChild(DelayTimer);
		}

		ShootTimer.Timeout += OnCooldownEnd;
		AddChild(ShootTimer);
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		if (IsShooting())
		{
			if (!OnCooldown)
			{
				if (HaveDelay)
				{
					OnCooldown = true;
					OnDelay = true;
					DelayTimer.Start();
				}
				else
				{
					OnShoot();
				}
			}
		}
		else
		{
			if (OnDelay)
			{
				OnCooldown = false;
				OnDelay = false;
				DelayTimer.Stop();
			}
		}
	}
	public override void _Process(double delta)
	{
		InputFiring = Source.inputMachine.TryGetInputEnabled("Fire");
	}
	/// <summary>
	/// Whether or not the inputs are triggering the cannon to shoot
	/// </summary>
	public bool IsShooting()
	{
		return
			(AutoFire ||
			InputFiring) &&
			(ProjCount < 0 || projTracker < ProjCount);
	}

	/// <summary>
	/// Forces the cannon to shoot, regardless of timer or cooldown
	/// </summary>
	public void OnShoot()
	{
		Entity proj = World.ProjSpawnerMain.Shoot(ToShoot, this, MyStats, Source.ZIndex - 1, Owner:Source);
		projTracker += 1;
		proj.TreeExited += () => projTracker -= 1;

		OnDelay = false;
		OnCooldown = true;
		ShootTimer.Start();
	}

	protected void OnCooldownEnd()
	{
		OnCooldown = false;
		if (IsShooting())
		{
			OnShoot();
		}
	}
}
