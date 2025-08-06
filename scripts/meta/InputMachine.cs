using Godot;
using Godot.Collections;

namespace BossRush2;

public class InputMachine
{
	public Array<string> InputRegistry = [];
	public Array<string> VariantInputRegistry = [];
	public Dictionary<string, bool> InputValues = [];

	public Dictionary VariantInputValues = [];
	public System.Collections.Generic.Dictionary<string, InputEvent> InputEvents = [];
	public InputMachine() { }
	public InputMachine(Array<string> _InputRegistry)
	{
		foreach (string id in _InputRegistry)
		{
			RegisterInput(id);
		}
	}
	public InputMachine(Array<string> _InputRegistry, Array<string> _VariantInputRegistry)
	{
		foreach (string id in _InputRegistry)
		{
			RegisterInput(id);
		}
		foreach (string id in _VariantInputRegistry)
		{
			RegisterVariantInput(id);
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
	public void TryRegisterInput(string id)
	{
		if (InputRegistry.Contains(id))
		{
			return;
		}
		InputValues.Add(id, false);
		InputRegistry.Add(id);
		InputEvents.Add(id, new());
	}
	public void RegisterVariantInput(string id)
	{
		if (VariantInputRegistry.Contains(id))
		{
			throw new("InputMachine already has variant a key called " + id);
		}
		VariantInputValues.Add(id, false);
		VariantInputRegistry.Add(id);
	}
	public void TryRegisterVariantInput(string id)
	{
		if (VariantInputRegistry.Contains(id))
		{
			return;
		}
		VariantInputValues.Add(id, false);
		VariantInputRegistry.Add(id);
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
	public bool GetInputEnabled(string id)
	{
		if (!InputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		return InputValues[id];
	}
	public void SetInputEnabled(string id, bool value)
	{
		if (!InputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		if (InputValues[id] != value)
		{
			if (value)
			{
				InputEnable(id);
			}
			else
			{ 
				InputDisable(id);
			}
		}
	}
	public bool TryGetInputEnabled(string id)
	{
		if (!InputRegistry.Contains(id))
		{
			return false;
		}
		return InputValues[id];
	}
	public Variant GetVariantInput(string id)
	{
		if (!VariantInputRegistry.Contains(id))
		{
			throw new("InputMachine has no variant key called " + id);
		}
		return VariantInputValues[id];
	}
	public void SetVariantInput(string id, Variant value)
	{
		if (!VariantInputRegistry.Contains(id))
		{
			throw new("InputMachine has no variant key called " + id);
		}
		VariantInputValues[id] = value;
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
