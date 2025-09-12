using BossRush2;
using Godot;

/// <summary>
/// A node with so little <c>DETERMINATION</c>, that it follows the mouse
/// </summary>
public partial class MouseTracker : Node2D, IBrObject
{
    public override void _Process(double delta) => GlobalPosition = GetGlobalMousePosition();
}