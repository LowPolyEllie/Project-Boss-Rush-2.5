
using Godot;

public delegate void WorldInputEventHandler(InputEvent inputEvent);
[GlobalClass]
public partial class WorldInputHandler : Node
{
	public static event WorldInputEventHandler worldInputEvent;
	public override void _Input(InputEvent inputEvent)
	{
		worldInputEvent(inputEvent);
	}
}
