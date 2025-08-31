using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Draws a grid, based on world boundaries
/// </summary>
[Tool]
public partial class GridBG : Node2D
{
	/// <summary>
	/// Godot just let me pronounce it as colour
	/// </summary>
	[Export]
	public Color ColorFill = new("#c8c8c8"), ColorStroke = new("#bebebe");

	[Export]
	public float StrokeWidth = 4f, TileSize = 35f;

	[Export]
	bool ClickToRedraw
	{
		get { return false; }
		set { QueueRedraw(); }
	}

	public override void _Ready()
	{
		QueueRedraw();
	}
	
	public override void _Draw()
	{
		Vector2 start;
		Vector2 end;
		Vector2 worldSize;

		worldSize = World.worldSize;
		start = -World.worldSize;
		end = World.worldSize;

		float left = start.X;
		float right = end.X;
		float top = start.Y;
		float bottom = end.Y;

		//Draws the solid colour background
		DrawRect(
			new Rect2(start, 2 * worldSize),
			ColorFill
		);

		//Finally draws the grids
		for (float currentX = start.X; currentX < end.X; currentX += TileSize)
		{
			DrawRect(
				new Rect2(currentX, top, StrokeWidth, bottom - top),
				ColorStroke
			);
		}
		for (float currentY = start.Y; currentY < end.Y; currentY += TileSize)
		{
			DrawRect(
				new Rect2(left, currentY, right - left, StrokeWidth),
				ColorStroke
			);
		}
	}
}
