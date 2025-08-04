using System.Linq;
using System.Reflection.Metadata;
using Godot;
using Godot.Collections;

namespace BossRush2;

public class PlayerController
{
    public InputMachine inputMachine;
    public Camera camera;
    public Entity player;
    private bool _active = false;
    [Export]
    public bool active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;
            camera.Enabled = _active;
        }
    }
    public Dictionary<string, Key> keyMapping = [];
    public void InitInputMachine()
    {
        inputMachine = new([.. keyMapping.Keys]);
        WorldInputHandler.worldInputEvent += HandleInput;
        player.inputMachine = inputMachine;
        camera.TargetEntity = player;
    }
    public void HandleInput(Godot.InputEvent inputEvent)
    {
        if (inputEvent is InputEventKey inputEventKey)
        {
            foreach ((string input, Key key) in keyMapping)
            {
                if (key == inputEventKey.Keycode)
                {
                    if (inputEventKey.Pressed)
                    {
                        inputMachine.InputEnable(input);
                    }
                    else
                    {
                        inputMachine.InputDisable(input);
                    }
                }
            }
        }
    }
}