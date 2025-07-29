using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// Base class for a node that is used to add new nodes into the game
/// </summary>
/// <remarks>
/// <br> Functionality here is minimal, this only exists to simplify a bit of setup code </br>
/// </remarks>
public partial class EntitySpawner : Node
{
	protected World WorldRef;

	public override void _Ready()
	{
		WorldRef = GetParent<World>();
		WorldRef.InitSetup += EnterGame;
	}

	public virtual void EnterGame() { }

	/// <summary>
	/// Functionality is obviously basic, this exists in case it needs to be expanded in the future
	/// </summary>
	protected void Spawn(Entity toAdd, int zindex)
	{
		toAdd.ZIndex = zindex;
		WorldRef.AddChild(toAdd);
	}

}
