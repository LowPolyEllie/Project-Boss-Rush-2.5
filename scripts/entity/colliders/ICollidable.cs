using Godot;
using Godot.Collections;
using System;

namespace BossRush2;

/// <summary>
/// An interface for any node that can hold a <c>Hitbox</c>
/// </summary>
public interface ICollidable
{
	/// <summary>
	/// Teams determine collision masks, collision layers, groups, and targeting queries
	/// </summary>
	string Team { get; set; }

	/// <summary>
	/// Access through TeamName_SubTeamName
	/// </summary>
	Array<string> SubTeams { get; set; }

	/// <summary>
	/// The instance of <c>Stats</c>, used for a variety of reasons
	/// </summary>
	Stats MyStats { get; set; }

	/// <summary>
	/// Triggers all hitboxes to forget it, and may also destroy its own hitbox
	/// </summary>
	/// <remarks>
	/// <br> Use mainly when entity is being destroyed </br>
	/// </remarks>
	event Action DisableCollision;

	/// <summary>
	/// Self explanatory
	/// </summary>
	void OnCollisionWith(float deltaF, ICollidable target, bool applyDamage);
}
