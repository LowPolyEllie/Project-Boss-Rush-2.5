using Godot;
using Godot.Collections;

namespace BossRush2;

[GlobalClass]
public partial class PlayerControllerWrapper : Node, IBrObject
{
    private PlayerController playerController;
    [Export]
    public Camera camera { get; set; }
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
            source = player,
            camera = camera,
            active = active
        };
        playerController.InitInputMachine();
    }
}