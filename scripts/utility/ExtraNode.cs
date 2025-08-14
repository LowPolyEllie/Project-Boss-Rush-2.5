using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Mrowwwww
/// </summary>
public static class ExtraNode
{
	/// <summary>
	/// A recursive function that climbs up the scene ladder until a parent of a certain type is found
	/// </summary>
	public static T SearchForParent<T>(this Node targetNode) where T : Node
	{
		Node parent = targetNode.GetParent();
		if (parent is null) return null;

		if (parent is T typedParent)
		{
			return typedParent;
		}
		else
		{
			return parent.SearchForParent<T>();
		}
	}
}
