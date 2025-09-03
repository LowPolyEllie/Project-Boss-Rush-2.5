using Godot;

namespace BossRush2;

/// <summary>
/// A single set of bar textures
/// </summary>
[GlobalClass]
public partial class BarTexture : Resource
{
    [Export]
    public Texture2D under;
    [Export]
    public Texture2D over;
    [Export]
    public Texture2D progress;

    /// <summary>
    /// <br> The point where this group is meant to show </br>
    /// <br> Yes it can exceed 1f, as long as proper offsetting is implemented </br>
    /// </summary>
    [Export]
    public double threshold = 1.0;

    public BarTexture()
    {
        threshold = 1.0;
    }
}
