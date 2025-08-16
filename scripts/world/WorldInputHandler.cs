using Godot;

public delegate void WorldInputEventHandler(InputEvent inputEvent);
public delegate void WorldProcessEventHandler(double delta);
public delegate void WorldPhysicsProcessEventHandler(double delta);
[GlobalClass]
public partial class WorldInputHandler : Node
{
	public static event WorldInputEventHandler WorldInputEvent;
	public static event WorldProcessEventHandler WorldProcessEvent;
	public static event WorldPhysicsProcessEventHandler WorldPhysicsProcessEvent;
	public override void _Input(InputEvent inputEvent)
	{
		WorldInputEvent(inputEvent);
	}
	public override void _Process(double delta)
	{
		WorldProcessEvent(delta);
	}
	public override void _PhysicsProcess(double delta)
	{
		WorldPhysicsProcessEvent(delta);
	}
}
