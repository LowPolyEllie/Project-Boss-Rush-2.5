using Godot;

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

	/// <summary>
	/// A recursive function that climbs up the scene ladder to get the true <c>ZIndex</c> of a <c>CanvasItem</c>
	/// </summary>
	public static int TrueZIndex(this Node targetNode, int carry = 0)
	{
		bool flag = false; //To stop the recursion
		if (targetNode is CanvasItem canvasNode)
		{
			carry += canvasNode.ZIndex;
			if(!canvasNode.ZAsRelative) flag = true;
		}

		var parent = targetNode.GetParentOrNull<Node>();
		if (parent is null) flag = true;

		if (flag)
		{
			return carry;
		}
		else
		{
			return parent.TrueZIndex(carry);
		}
	}
}
