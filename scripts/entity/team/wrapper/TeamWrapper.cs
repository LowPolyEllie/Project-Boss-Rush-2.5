using Godot;
using Godot.Collections;

namespace BossRush2;

[GlobalClass]
public partial class TeamWrapper : Node
{
    [Export]
	public int collisionLayer;
    [Export]
	public Array<TeamWrapper> collisionMask;
}