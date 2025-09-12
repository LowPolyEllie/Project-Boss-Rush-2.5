using Godot;
using Godot.Collections;

namespace BossRush2;

/// <summary>
/// The data used to generate a polygon with the spawner
/// </summary>
[GlobalClass]
public partial class TeamTarget : Resource, IBrObject
{
    [Export]
    public Dictionary<string,string> teamData;
    [Export]
    public float distanceMultiplier = 1f;

    public TeamTarget()
    {
        distanceMultiplier = 1f;
    }
}
