using Godot;
using System;

namespace BossRush2;

/// <summary>
/// The data used to generate a polygon with the spawner
/// </summary>
[GlobalClass]
public partial class PolygonTemplate : Resource
{
	/// <summary>
	/// The higher the value, the more likely this template is to be selected
	/// </summary>
	[Export]
	public float Weight;

	/// <summary>
	/// Polygon stats, note that speed would be ignored unless custom ai is implemented
	/// </summary>
	[Export]
	public Stats MyStats;

	/// <summary>
	/// The texture that the polygon's sprite will hold
	/// </summary>
	[Export]
	public Texture2D Sprite;

	/// <summary>
	/// Only uses a single circular collider, wall colliders and general hitboxes are shared
	/// </summary>
	[Export]
	public CircleShape2D Collider;

	/// <summary>
	/// If not null, will override the template, instead using this scene
	/// </summary>
	/// <remarks>
	/// <br> Use for special polygons types that require multiple segments or custom scripts </br>
	/// </remarks>
	[Export]
	public PackedScene SceneOverride;

	public PolygonTemplate()
	{
		Weight = 1f;
		Collider = new CircleShape2D();
		MyStats = new();
	}
}
