using Godot;
using Godot.Collections;
using System;

namespace BossRush2;

public class PlayerController : Controller, IBrObject
{
	public Entity player;
	public Camera camera;
	private bool _active = false;
	public override bool active
	{
		set
		{
			_active = value;
			camera.Enabled = _active;
		}
	}
	public override void InitInputMachine()
	{
		variantInput = ["Target"];

		base.InitInputMachine();

		camera.target = new(source);

	}
	public override void ProcessInput(double delta)
	{
		//Kinda jank but it works
		inputMachine.SetVariantInput("Target", source.GetGlobalMousePosition());
	}
}
