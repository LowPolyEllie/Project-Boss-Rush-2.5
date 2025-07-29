using Godot;
using System;

namespace BossRush2;

/// <summary>
/// A single set of bar textures
/// </summary>
[GlobalClass]
public partial class BarTexture : Resource
{
    [Export]
    public Texture2D Under;
    [Export]
    public Texture2D Over;
    [Export]
    public Texture2D Progress;

    /// <summary>
    /// <br> The point where this group is meant to show </br>
    /// <br> Yes it can exceed 1f, as long as proper offsetting is implemented </br>
    /// </summary>
    [Export]
    public double Threshold = 1.0;

    public BarTexture()
    {
        Threshold = 1.0;
    }
}
