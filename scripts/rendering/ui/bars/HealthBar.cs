using Godot;
using System;

namespace BossRush2;

/// <summary>
/// An extended <c>TextureProgressBar</c>, with more functionality fit for my game
/// </summary>
/// <remarks>
/// <br> Utilises automatic referencing by passing a lambda to <c>TargetRef</c> </br>
/// <br> Interpolates changes in the value, with a controllable multiplier </br>
/// <br> Can dynamically change textures from the <c>TexturePool</c>, based on value </br>
/// <br> (The value is ratio based, meaning it doesn't care about min or max) </br>
/// </remarks>
[GlobalClass]
public partial class HealthBar : StatBar
{
    public override void _Ready()
    {
        min = 0;
        max = subject.stats.Health;
        TrueValue = max;
        base._Ready();
    }
    public override void _Process(double delta)
    {
        targetValue = subject.health;
        base._Process(delta);
    }
}