using Godot;
using System;

namespace BossRush2;

/// <summary>
/// A node with so much <c>DETERMINATION</c>, that it refuses to rotate
/// </summary>
[GlobalClass]
[Tool]
public partial class StopRotating : Node2D
{
	public override void _EnterTree() => GlobalRotation = 0f;
	public override void _Ready() => GlobalRotation = 0f;
	public override void _Process(double delta) => GlobalRotation = 0f;
	public override void _PhysicsProcess(double delta) => GlobalRotation = 0f;
}
