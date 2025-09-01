using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Basically just World, except for UI
/// </summary>
[GlobalClass]
public partial class UIManager : Control, IBrObject
{
	public static UIManager activeManager { get; set; }

    public override void _EnterTree()
	{
		activeManager = this;
	}
}
