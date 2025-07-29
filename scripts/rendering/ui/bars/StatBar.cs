using Godot;
using System;

namespace BossRush2;

/// <summary>
/// An extended <c>TextureProgressBar</c>, with more functionality fit for my game
/// </summary>
/// <remarks>
/// <br> Utilises automatic referencing by passing a lambda to <c>TargetRef</c> </br>
/// <br> Interpolates changes in the value, with a controllable multiplier </br>
/// <br> Can dynamically change textures from the <c>TexturePool</c>, based on value </br>
/// <br> (The value is ratio based, meaning it doesn't care about min or max) </br>
/// </remarks>
[GlobalClass]
public partial class StatBar : TextureProgressBar
{
    double targetValue;
    [Export]
    protected double TargetValue
    {
        get => targetValue;
        set
        {
            if (targetValue != value)
            {
                targetValue = value;
                OnTargetValueChanged();
            }
        }
    }

    /// <summary>
    /// "Fuck you I'm gonna fuck over your lerp anyways even if Rounded is false", Godot said.
    /// </summary>
    protected double TrueValue;

    Func<(double min, double max, double value)> targetRef;

    /// <summary>
    /// Pass a lambda function for to connect this bar to any stat
    /// </summary>
    public Func<(double min, double max, double value)> TargetRef
    {
        get => targetRef;
        set
        {
            targetRef = value;
            OnTargetRefChanged();
        }
    }

    [Export(PropertyHint.Range, "0.0,1.0,0.01")]
    public double Interpolation = 0.1;

    // A set of cached textures ready to be loaded and used
    [Export]
    protected BarTextureSet TexturePool;

    public override void _Process(double delta)
    {
        if (TargetRef is not null)
        {
            MinValue = TargetRef().min;
            MaxValue = TargetRef().max;
            TargetValue = TargetRef().value;
        }
        TrueValue = Mathf.Lerp(TrueValue, TargetValue, 1 - Mathf.Pow(1 - Interpolation, delta));
        Value = TrueValue;
    }

    protected void OnTargetRefChanged()
    {
        MinValue = TargetRef().min;
        MaxValue = TargetRef().max;
        TargetValue = TargetRef().value;
        TrueValue = TargetValue;
        Value = TrueValue;
    }

    protected void OnTargetValueChanged()
    {
        BarTexture newTexture = TexturePool.GetCurrentBar((targetValue - MinValue) / (MaxValue - MinValue));
        TextureUnder = newTexture.Under;
        TextureOver = newTexture.Over;
        TextureProgress = newTexture.Progress;
    }
}
