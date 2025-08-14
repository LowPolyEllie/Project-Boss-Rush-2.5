using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;

namespace BossRush2;

/// <summary>
/// A template that can hold tanks in game
/// </summary>
[GlobalClass]
public partial class TankLoader : Resource
{
    /// <summary>
    /// The scene to be loaded, any entity is allowed
    /// </summary>
    [Export]
    public PackedScene toLoad;

    /// <summary>
    /// The ui version of the scene, only use Sprite2D
    /// </summary>
    [Export]
    public PackedScene uiDisplay;

    public void LoadTank(Entity target)
    {
        var toSteal = toLoad.Instantiate<Entity>();
        target.stats = toSteal.stats;

        foreach (var toDelete in target.GetChildren())
        {
            target.RemoveChild(toDelete);
            toDelete.QueueFree();
        }
        foreach (var toAdd in toSteal.GetChildren())
        {
            toAdd.Owner = null; //to shut the compiler up
            toSteal.RemoveChild(toAdd);
            target.AddChild(toAdd);
        }

        toSteal.QueueFree();
    }
}
