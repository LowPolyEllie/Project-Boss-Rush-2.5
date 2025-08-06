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
	public Curve CurveX;

	/// <summary>
	/// The curve to represent the change in Y over time
	/// </summary>
	[Export]
	public Curve CurveY;

	/// <summary>
	/// The curve to represent the change in rotation over time
	/// </summary>
	[Export]
	public Curve CurveRot;

	public override void OnAnimationStep(double delta, float deltaF)
	{        
		//This exists because godot for some reason won't let me change X and Y individually
		float newX = Subject.Position.X;
		float newY = Subject.Position.Y;

		if (CurveX != null)
		{
			newX = CurveX.Sample(CurveX.MinDomain + (CurveX.MaxDomain - CurveX.MinDomain) / AnimationTime * AnimationStep);
		}
		if (CurveY != null)
		{
			newY = CurveY.Sample(CurveY.MinDomain + (CurveY.MaxDomain - CurveY.MinDomain) / AnimationTime * AnimationStep);
		}
		Subject.Position = new Vector2(newX, newY);

		if (CurveRot != null)
		{
			Subject.Rotation = CurveRot.Sample(CurveRot.MinDomain + (CurveRot.MaxDomain - CurveRot.MinDomain) / AnimationTime * AnimationStep);
		}
	}
}
