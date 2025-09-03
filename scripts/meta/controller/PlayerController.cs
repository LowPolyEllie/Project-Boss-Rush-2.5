using Godot;
using Godot.Collections;

namespace BossRush2;

public class PlayerController : Controller, IBrObject
{
	public Entity player;
	public Camera camera;
	private bool _active = false;
	public Dictionary<string, Key> keyMapping = [];
	public override bool active
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
	public override void InitInputMachine()
	{
		base.InitInputMachine();

		WorldInputHandler.WorldInputEvent += HandleInput;
		WorldInputHandler.WorldProcessEvent += ProcessInput;

		camera.target = new(source);
	}
	public override void Init()
	{
		InitInputMachine();
		basis.playerController = this;
	}
	public void ProcessInput(double delta)
	{
		//Kinda jank but it works
		inputMachine.SetVariantInput("Target", source.GetGlobalMousePosition());
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
}
