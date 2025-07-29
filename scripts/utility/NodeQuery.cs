using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// Query related Node functionality, may extend <c>Node</c> or <c>Godot.Collections.Array</c>
/// </summary>
/// <remarks>
/// <br> Please only return <c>System.Collections.List</c>, if a method does return an Array </br>
/// <br> This is because they are also meant to serve as type conversion and static casters </br>
/// </remarks>
public static class NodeQuery
{
	/// <summary>
	/// Queries Godot's node array by type
	/// </summary>
	/// <remarks>
	/// <br> This also serves as a static caster, and an additional safety net for group related errors </br>
	/// <br> Returns List<T> because its easier to work with </br>
	/// </remarks>
	public static List<T> FilterNodeByType<T>(this Array<Node> nodeArray) where T : Node
	{
		List<T> toReturn = [];
		foreach (var thisNode in nodeArray)
		{
			if (thisNode is T typedNode) toReturn.Add(typedNode);
		}
		return toReturn;
	}

	/// <summary>
	/// Queries children by type
	/// </summary>
	/// <remarks>
	/// <br> This also serves as a static caster, and an additional safety net for group related errors </br>
	/// <br> Returns List<T> because its easier to work with </br>
	/// </remarks>
	public static List<T> GetChildrenOfType<T>(this Node parentNode) where T : Node
	{
		return parentNode.GetChildren().FilterNodeByType<T>();
	}

	/// <summary>
	/// Searches for the first child node that fits under a certain type
	/// </summary>
	/// <remarks>
	/// <br> I honestly don't recommend using this at all, since BossRush2's node structure typically ignores node path </br>
	/// <br> Instead, I recommend simply allowing a selector to set references </br>
	/// <br> Nevertheless, this still exists for the few cases in the future that might need it </br>
	/// </remarks>
	public static T GetFirstChildOfType<T>(this Node parentNode) where T : Node
	{
		foreach (var thisChild in parentNode.GetChildren())
		{
			if (thisChild is T typedChild)
			{
				return typedChild;
			}
		}
		return null;
	}
}