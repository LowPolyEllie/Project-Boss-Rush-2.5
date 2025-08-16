using BossRush2;
using Godot;

public delegate void WorldInputEventHandler(Godot.InputEvent inputEvent);
public delegate void WorldProcessEventHandler(double delta);
public delegate void WorldPhysicsProcessEventHandler(double delta);
[GlobalClass]
public partial class WorldInputHandler : Node
{
	public static event WorldInputEventHandler WorldInputEvent;
	public static event WorldProcessEventHandler WorldProcessEvent;
	public static event WorldPhysicsProcessEventHandler WorldPhysicsProcessEvent;
	public override void _Input(Godot.InputEvent inputEvent)
	{
		WorldInputEvent?.Invoke(inputEvent);
	}
	public override void _Process(double delta)
	{
		WorldProcessEvent?.Invoke(delta);
	}
	public override void _PhysicsProcess(double delta)
	{
		WorldPhysicsProcessEvent?.Invoke(delta);
	}
}
