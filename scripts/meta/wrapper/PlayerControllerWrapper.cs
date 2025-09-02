using Godot;
using Godot.Collections;

namespace BossRush2;

[GlobalClass]
public partial class PlayerControllerWrapper : Node, IBrObject
{
    public PlayerController playerController
    {
        get;
        private set;
    }

    /// <summary>
    /// Camera that this controller uses
    /// </summary>
    [Export]
    public Camera camera { get; set; }
    /// <summary>
    /// What this controller is controlling
    /// </summary>
    [Export]
    public Entity source { get; set; }
    /// <summary>
    /// The main player node
    /// </summary>
    [Export]
    public Entity player { get; set; }

    [Export]
    public Dictionary<string, Key> keyMapping = new()
    { 
        {"Up",Key.W },
        {"Down",Key.S },
        {"Left",Key.A },
        {"Right",Key.D },
        {"Fire",Key.Launch1 }
    };
    [Export]
    public Array<string> variantInput = ["Target"];
    [Export]
    public bool active { get; set; }
    public override void _Ready()
    {
        playerController = new()
        {
            keyMapping = keyMapping,
            variantInput = variantInput,
            source = source,
            player = player,
            camera = camera,
            active = active
        };
        playerController.InitInputMachine();
    }
}