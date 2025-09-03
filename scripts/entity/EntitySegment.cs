using Godot;

namespace BossRush2;

/// <summary>
/// Base class for in game objects that behave independantly but are related to <c>Entity</c>
/// </summary>
[GlobalClass]
public partial class EntitySegment : Node2D, IBrObject, IEntitySegment
{
	/// <summary>
	/// Meant for controllable projectiles and potentially damage reflection
	/// </summary>
	/// <remarks>
	/// This is a general use field, it can mean a variety of things
	/// </remarks>
	[Export]
	public Entity owner { get; set; }

	/// <summary>
	/// Whether the entity can independently collide
	/// </summary>
	[Export]
	public bool isTopLevel { get; set; } = false;

	/// <summary>
	/// Please call in _Ready() for every derived class
	/// </summary>
	public void FindOwner()
	{
		// Climbs up the scene ladder in search of an owner, if it needs yet lacks one
		if (owner is null && !isTopLevel)
		{
			owner = this.SearchForParent<Entity>();
		}
	}
}
