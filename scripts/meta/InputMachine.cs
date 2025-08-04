using Godot;
using Godot.Collections;

namespace BossRush2;

public class InputMachine
{
    public Array<string> InputRegistry = [];
    public Dictionary<string, bool> InputValues = [];
    public System.Collections.Generic.Dictionary<string, InputEvent> InputEvents = [];

    public InputMachine(Array<string> _InputRegistry)
    {
        foreach (string id in _InputRegistry)
        {
            RegisterInput(id);
        }
    }
    public void RegisterInput(string id)
    {
        if (InputRegistry.Contains(id))
        {
            throw new("InputMachine already has a key called " + id);
        }
        InputValues.Add(id, false);
        InputRegistry.Add(id);
        InputEvents.Add(id, new());
    }
    public void InputEnable(string id)
    {
        if (!InputRegistry.Contains(id))
        {
            throw new("InputMachine has no key called " + id);
        }
        if (!InputValues[id])
        { 
            InputFire(id);
        }
        InputValues[id] = true;
    }
    public void InputDisable(string id)
    {
        if (!InputRegistry.Contains(id))
        {
            throw new("InputMachine has no key called " + id);
        }
        InputValues[id] = false;
    }
    public bool InputEnabled(string id)
    {
        if (!InputRegistry.Contains(id))
        {
            throw new("InputMachine has no key called " + id);
        }
        return InputValues[id];
    }
    public void InputFire(string id)
    { 
        if (!InputRegistry.Contains(id))
        {
            throw new("InputMachine has no key called " + id);
        }
        InputEvents[id].Fire();
    }
    public InputEvent GetInputEvent(string id)
    { 
        if (!InputRegistry.Contains(id))
        {
            throw new("InputMachine has no key called " + id);
        }
        return InputEvents[id];
    }
}
public delegate void InputEventDelegate();
public class InputEvent {
    private event InputEventDelegate InputEventDelegate;
    public void Fire()
    {
        if (InputEventDelegate is not null)
        {
            InputEventDelegate();
        }
    }
    public static InputEvent operator +(InputEvent _this, InputEventDelegate _delegate)
    {
        _this.InputEventDelegate += _delegate;
        return _this;
    }
    public static InputEvent operator -(InputEvent _this, InputEventDelegate _delegate)
    {
        _this.InputEventDelegate -= _delegate;
        return _this;
    }
}