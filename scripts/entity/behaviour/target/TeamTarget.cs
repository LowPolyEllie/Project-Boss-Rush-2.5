using Godot;

namespace BossRush2;

/// <summary>
/// The data used to generate a polygon with the spawner
/// </summary>
[GlobalClass]
public partial class TeamTarget : Resource, IBrObject
{
    [Export]
    public string teamLayer, team;
    [Export]
    public float distanceMultiplier = 1f;

    public TeamTarget()
    {
        distanceMultiplier = 1f;
    }
}
