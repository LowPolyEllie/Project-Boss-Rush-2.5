using Godot;
using Godot.Collections;

namespace BossRush2;

public class Controller : IBrObject
{
    public InputMachine inputMachine;
    public Entity source;
    private bool _active = false;
    public virtual bool active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;
        }
    }
    public Dictionary<string, Key> keyMapping = [];
    public Array<string> variantInput = [];
    public virtual void InitInputMachine()
    {
        inputMachine = new([.. keyMapping.Keys], variantInput);
        WorldInputHandler.WorldInputEvent += HandleInput;
        WorldInputHandler.WorldProcessEvent += ProcessInput;
        source.inputMachine = inputMachine;
    }
    public void AddKeybind(string input, Key key, bool @override = false)
    {
        if (keyMapping.ContainsKey(input))
        {
            if (@override)
            {
                keyMapping[input] = key;
            }
        }
        else
        {
            keyMapping.Add(input, key);
        }
    }
    public virtual void HandleInput(Godot.InputEvent inputEvent)
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
        //"Launch" key corresponds to "MouseButton" eg. Launch1 = MouseButton[1](Left click). Reference enum values
        if (inputEvent is InputEventMouseButton inputEventMouseButton)
        {
            foreach ((string input, Key key) in keyMapping)
            {
                if (key >= Key.Launch0 && key <= Key.Launch9)
                {
                    if ((int)inputEventMouseButton.ButtonIndex == key - Key.Launch0)
                    {
                        if (inputEventMouseButton.Pressed)
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
    public virtual void ProcessInput(double delta) { }
}