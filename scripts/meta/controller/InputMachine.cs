using Godot;
using Godot.Collections;

namespace BossRush2;

public class InputMachine : IBrObject
{
	public Array<string> inputRegistry = [];
	public Array<string> variantinputRegistry = [];
	public Dictionary<string, bool> inputValues = [];

	public Dictionary variantinputValues = [];
	public System.Collections.Generic.Dictionary<string, InputEvent> inputEvents = [];
	public InputMachine() { }
	public InputMachine(Array<string> _inputRegistry)
	{
		foreach (string id in _inputRegistry)
		{
			RegisterInput(id);
		}
	}
	public InputMachine(Array<string> _inputRegistry, Array<string> _variantinputRegistry)
	{
		foreach (string id in _inputRegistry)
		{
			RegisterInput(id);
		}
		foreach (string id in _variantinputRegistry)
		{
			RegisterVariantInput(id);
		}
	}
	public void RegisterInput(string id)
	{
		if (inputRegistry.Contains(id))
		{
			throw new("InputMachine already has a key called " + id);
		}
		inputValues.Add(id, false);
		inputRegistry.Add(id);
		inputEvents.Add(id, new());
	}
	public void TryRegisterInput(string id)
	{
		if (inputRegistry.Contains(id))
		{
			return;
		}
		inputValues.Add(id, false);
		inputRegistry.Add(id);
		inputEvents.Add(id, new());
	}
	public void RegisterVariantInput(string id)
	{
		if (variantinputRegistry.Contains(id))
		{
			throw new("InputMachine already has variant a key called " + id);
		}
		variantinputValues.Add(id, false);
		variantinputRegistry.Add(id);
	}
	public void TryRegisterVariantInput(string id)
	{
		if (variantinputRegistry.Contains(id))
		{
			return;
		}
		variantinputValues.Add(id, false);
		variantinputRegistry.Add(id);
	}
	public void InputEnable(string id)
	{
		if (!inputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		if (!inputValues[id])
		{ 
			InputFire(id);
		}
		inputValues[id] = true;
	}
	public void InputDisable(string id)
	{
		if (!inputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		inputValues[id] = false;
	}
	public bool GetInputEnabled(string id)
	{
		if (!inputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		return inputValues[id];
	}
	public void SetInputEnabled(string id, bool value)
	{
		if (!inputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		if (inputValues[id] != value)
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
		if (!inputRegistry.Contains(id))
		{
			return false;
		}
		return inputValues[id];
	}
	public Variant GetVariantInput(string id)
	{
		if (!variantinputRegistry.Contains(id))
		{
			throw new("InputMachine has no variant key called " + id);
		}
		return variantinputValues[id];
	}
	public void SetVariantInput(string id, Variant value)
	{
		if (!variantinputRegistry.Contains(id))
		{
			throw new("InputMachine has no variant key called " + id);
		}
		variantinputValues[id] = value;
	}
	public void InputFire(string id)
	{ 
		if (!inputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		inputEvents[id].Fire();
	}
	public InputEvent GetInputEvent(string id)
	{ 
		if (!inputRegistry.Contains(id))
		{
			throw new("InputMachine has no key called " + id);
		}
		return inputEvents[id];
	}
}
public delegate void InputEventDelegate();
public class InputEvent {
	private event InputEventDelegate InputEventDelegate;
	public void Fire()
	{
		InputEventDelegate?.Invoke();
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
