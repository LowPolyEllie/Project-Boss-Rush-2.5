using Godot;
using System;
using BrAnimator;

namespace BossRush2;

/// <summary>
/// Loads a <c>PackedScene</c> to shoot as a projectile
/// </summary>
[GlobalClass]
public partial class Cannon : EntitySegment
{
	/// <summary>
	/// The parent's input type that would cause this cannon to shoot
	/// </summary>
	[Export]
	public string shootTrigger = "Fire";

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
	/// The maximum number of projectiles this cannon can shoot, set to -1 for no limit
	/// </summary>
	[Export]
	public int maxProjectiles = -1;
	protected int currentProjectiles = 0;

	/// <summary>
	/// The time is takes after input is pressed before bullet is fired
	/// </summary>
	/// <remarks>
	/// <br> I feel like this should be a stat -Fox </br>
	/// <br> Its unique to specifically <c>Cannon</c>, and bosses will need more than one delay anyways -Ellie </br>
	/// </remarks>
	[Export]
	public float delay;

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
	public bool isShooting => 
		(autoFire ||
		inputFiring || inputFireEventBuffer) &&
		(maxProjectiles == -1 || currentProjectiles < maxProjectiles);
	
	public bool inputFiring;
	protected bool inputFireEventBuffer = false;
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
		FindOwner();
		owner ??= this.SearchForParent<Entity>();
		if (owner.inputMachine.HasInput(shootTrigger))
		{
			owner.inputMachine.GetInputEvent(shootTrigger).Listen(shootBufferEvent);
		}

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

	public void shootBufferEvent()
	{
		inputFireEventBuffer = true;
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
		inputFireEventBuffer = false;
	}
	public override void _Process(double delta)
	{
		inputFiring = owner.inputMachine.TryGetInputEnabled(shootTrigger);
		animator.StepAnimation(delta);
	}

	/// <summary>
	/// Forces the cannon to shoot, regardless of timer or cooldown
	/// </summary>
	public void OnShoot()
	{
		animator.StartAnimation();

		Entity proj = World.activeWorld.activeProjectileSpawner.Shoot(toShoot, this, stats, owner.TrueZIndex() - 1, owner:owner);
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
