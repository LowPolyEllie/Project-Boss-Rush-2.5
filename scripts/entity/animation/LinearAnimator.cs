using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Use if you want to animate from a linear viewpoint instead of with Curve2D
/// </summary>
[GlobalClass]
public partial class LinearAnimator : SegmentAnimator
{
	/// <summary>
	/// The curve to represent the change in X over time
	/// </summary>
	[Export]
	public Curve curveX;

	/// <summary>
	/// The curve to represent the change in Y over time
	/// </summary>
	[Export]
	public Curve curveY;

	/// <summary>
	/// The curve to represent the change in rotation over time
	/// </summary>
	[Export]
	public Curve curveRot;

	/// <summary>
	/// Whether or not the position will be relative to global rotation
	/// </summary>
	[Export]
	public bool rotAsPivot;

	public override void OnAnimationStep(double delta, float deltaF)
	{
		//This exists because godot for some reason won't let me change X and Y individually
		float newX = subject.Position.X;
		float newY = subject.Position.Y;

		if (curveX != null)
		{
			newX = curveX.Sample(curveX.MinDomain + (curveX.MaxDomain - curveX.MinDomain) / animationTime * animationStep);
		}
		if (curveY != null)
		{
			newY = curveY.Sample(curveY.MinDomain + (curveY.MaxDomain - curveY.MinDomain) / animationTime * animationStep);
		}
		subject.Position = new Vector2(newX, newY);

		if (curveRot != null)
		{
			subject.Rotation = curveRot.Sample(curveRot.MinDomain + (curveRot.MaxDomain - curveRot.MinDomain) / animationTime * animationStep);
		}

		if (rotAsPivot) subject.Position = subject.Position.Rotated(subject.Rotation);
	}
}
