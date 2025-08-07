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
	public Stats stats;

	/// <summary>
	/// The time it takes after the bullet is fired for the cannon to shoot again
	/// 
	/// this should DEFINITELY be a stat, seperate CannonStat in the future?
	/// </summary>
	[Export]
	public float fireRate = 0.5f;

	/// <summary>
	/// The maximum number of projectiles this cannon can shoot, set to -1f for no limit
	/// </summary>
	[Export]
	public float maxProjectiles = -1f;
	protected float currentProjectiles = 0f;

	/// <summary>
	/// The time is takes after input is pressed before bullet is fired
	///  
	/// I feel like this should be a stat -Fox
	/// </summary>
	[Export]
	public float delay;
	
	[Export]
	public Entity owner;

	/// <summary>
	/// The scene to instantiate once the bullet fires
	/// </summary>
	[Export]
	public PackedScene toShoot;

	/// <summary>
	/// Automatically fire without any input needed
	/// </summary>
	[Export]
	public bool autoFire;

	/// <summary>
	/// Whether or not the cooldown or delay timer is active
	/// </summary>
	public bool onCooldown = false;
	/// <summary>
	/// Whether or not the delay timer is active
	/// </summary>
	public bool onDelay = false;
	public bool isShooting{ get
		{
			return
				(autoFire ||
				inputFiring) &&
				(maxProjectiles == -1 || currentProjectiles < maxProjectiles);
			
	 } set { } }
	public bool inputFiring;
	[Export]
	public Node2D body;
	[Export]
	public LinearAnimator animator;

	/// <summary>
	/// Timers are created at runtime for abstraction purposes
	/// </summary>
	protected Timer shootTimer, delayTimer;

	public override void _Ready()
	{
		shootTimer = new Timer()
		{
			OneShot = true,
			WaitTime = fireRate,
		};
		if (delay > 0)
		{
			delayTimer = new Timer()
			{
				OneShot = true,
				WaitTime = delay
			};
			delayTimer.Timeout += OnShoot;
			AddChild(delayTimer);
		}

		shootTimer.Timeout += OnCooldownEnd;
		AddChild(shootTimer);
		animator = (LinearAnimator)animator.Duplicate();
		animator.subject = body;
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		if (isShooting)
		{
			if (!onCooldown)
			{
				if (delay > 0)
				{
					onCooldown = true;
					onDelay = true;
					delayTimer.Start();
				}
				else
				{
					OnShoot();
				}
			}
		}
		else
		{
			if (onDelay)
			{
				onCooldown = false;
				onDelay = false;
				delayTimer.Stop();
			}
		}
	}
	public override void _Process(double delta)
	{
		inputFiring = owner.inputMachine.TryGetInputEnabled("Fire");
		animator.StepAnimation(delta);
	}

	/// <summary>
	/// Forces the cannon to shoot, regardless of timer or cooldown
	/// </summary>
	public void OnShoot()
	{
		animator.StartAnimation();

		Entity proj = World.activeWorld.activeProjectileSpawner.Shoot(toShoot, this, stats, owner.ZIndex - 1, owner:owner);
		currentProjectiles += 1;
		proj.TreeExited += () => currentProjectiles -= 1;

		onDelay = false;
		onCooldown = true;
		shootTimer.Start();
	}

	protected void OnCooldownEnd()
	{
		onCooldown = false;
		if (isShooting)
		{
			OnShoot();
		}
	}
}
