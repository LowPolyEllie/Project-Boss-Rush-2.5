using Godot;

namespace BossRush2;

/// <summary>
/// Base class for an entity that is used to add new entities into the game
/// </summary>
/// <remarks>
/// <br> Functionality here is minimal, this only exists to simplify a bit of setup code </br>
/// </remarks>
public partial class EntitySpawner : Node, IBrObject
{

	public override void _Ready()
	{
		World.activeWorld.InitSetup += EnterGame;
	}

	public virtual void EnterGame() { }

	/// <summary>
	/// Functionality is obviously basic, this exists in case it needs to be expanded in the future
	/// </summary>
	protected void Spawn(Entity toAdd, int zindex)
	{
		toAdd.ZIndex = zindex;
		World.activeWorld.AddChild(toAdd);
	}
}
