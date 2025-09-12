namespace BossRush2;

/// <summary>
/// For anything that needs a reference to an <c>Entity</c> without needing to inherit <c>EntitySegment</c>
/// </summary>
public interface IEntitySegment
{
	/// <summary>
	/// Meant for controllable projectiles and potentially damage reflection
	/// </summary>
	/// <remarks>
	/// This is a general use field, it can mean a variety of things
	/// </remarks>
	public Entity owner { get; set; }

	/// <summary>
	/// Whether the entity can independently collide
	/// </summary>
	public bool isTopLevel { get; set; }

	/// <summary>
	/// Whether or not this node will remain after a tankload
	/// </summary>
	public bool persistent { get; set; }
}