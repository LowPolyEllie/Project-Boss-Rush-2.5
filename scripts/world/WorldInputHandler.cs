using Godot;

public delegate void WorldInputEventHandler(InputEvent inputEvent);
public delegate void WorldProcessEventHandler(double delta);
[GlobalClass]
public partial class WorldInputHandler : Node
{
	public static event WorldInputEventHandler worldInputEvent;
	public static event WorldProcessEventHandler worldProcessEvent;
	public override void _Input(InputEvent inputEvent)
	{
		worldInputEvent(inputEvent);
	}
	public override void _Process(double delta)
	{
		worldProcessEvent(delta);
	}
}
