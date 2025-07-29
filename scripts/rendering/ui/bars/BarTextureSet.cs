using Godot;
using Godot.Collections;
using System;

namespace BossRush2;

/// <summary>
/// Sets of bar textures specifically for StatBar nodes
/// </summary>
[GlobalClass]
public partial class BarTextureSet : Resource
{
    /// <summary>
    /// Must be ordered in descending format for optimisation reasons
    /// </summary>
    [Export]
    public Array<BarTexture> MyTextures = [];

    public BarTexture GetCurrentBar(double value)
    {
        foreach (var thisTexture in MyTextures)
        {
            if (value <= thisTexture.Threshold)
            {
                return thisTexture;
            }
        }
        throw new InvalidOperationException("Value exceed minimum threshold");
    }

    public BarTextureSet()
    {
        MyTextures = [];
    }
}