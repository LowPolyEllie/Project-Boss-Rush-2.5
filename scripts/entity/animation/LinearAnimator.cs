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

        /// <summary>
    /// The curve to represent the change in direction over time
    /// </summary>
    [Export]
    public Curve CurveDir;

    public override void OnAnimationStep(double delta, float deltaF)
    {
        //This exists because godot for some reason won't let me change X and Y individually
        float newX = Position.X;
        float newY = Position.Y;

        if (CurveX != null)
        {
            newX = CurveX.Sample(AnimationStep);
        }
        if (CurveY != null)
        {
            newY = CurveY.Sample(AnimationStep);
        }
        Position = new Vector2(newX, newY);

        if (CurveRot != null)
        {
            Rotation = CurveRot.Sample(AnimationStep);
        }
    }
}
