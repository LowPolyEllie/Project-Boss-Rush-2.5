using Godot;
using Godot.Collections;

namespace BossRush2;

[GlobalClass]
public partial class PlayerControllerWrapper : Node
{
    private PlayerController playerController;
    [Export]
    public Camera camera { get; set; }
    [Export]
    public Entity player { get; set; }
    [Export]
    public Dictionary<string, Key> keyMapping { get; set; } = [];
    [Export]
    public bool active { get; set; }
    public override void _Ready()
    {
        playerController = new()
        {
            keyMapping = keyMapping,
            player = player,
            camera = camera,
            active = active
        };
        playerController.InitInputMachine();
    }
}