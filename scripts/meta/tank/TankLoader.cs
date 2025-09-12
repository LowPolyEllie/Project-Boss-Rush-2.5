using Godot;

namespace BossRush2;

/// <summary>
/// A template that can hold tanks in game
/// </summary>
[GlobalClass]
public partial class TankLoader : Resource, IBrObject
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
            if (
                    toDelete is IEntitySegment entitySegment && !entitySegment.persistent
                )
            {
                target.RemoveChild(toDelete);
                toDelete.QueueFree();
            }
        }
        foreach (var toAdd in toSteal.GetChildren())
        {
            toAdd.Owner = null; //to shut the compiler up
            if (
                    toAdd is IEntitySegment entitySegment && !entitySegment.persistent
                )
            {
                toSteal.RemoveChild(toAdd);
                target.AddChild(toAdd);
            }
        }

        toSteal.QueueFree();
    }
}
