using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

[GlobalClass]
public partial class TeamWrapper : Node{
    [Export]
	public int collisionLayer;
    [Export]
	public Array<TeamWrapper> collisionMask;
}