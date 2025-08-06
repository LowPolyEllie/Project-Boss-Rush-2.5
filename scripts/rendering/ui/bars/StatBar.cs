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
[GlobalClass]
public partial class StatBar : TextureProgressBar
{
	protected double _targetValue;
	[Export]
	public Entity subject;
	protected double targetValue
	{
		get => _targetValue;
		set
		{
			if (_targetValue != value)
			{
				_targetValue = value;
				OntargetValueChanged();
			}
		}
	}
	public float min;
	public float max;

	/// <summary>
	/// "Fuck you I'm gonna fuck over your lerp anyways even if Rounded is false", Godot said.
	/// </summary>
	protected double TrueValue;

	[Export(PropertyHint.Range, "0.0,1.0,0.01")]
	public double Interpolation = 0.1;

	// A set of cached textures ready to be loaded and used
	[Export]
	protected BarTextureSet TexturePool;

	public override void _Ready()
	{
		MinValue = min;
		MaxValue = max;
	}

	public override void _Process(double delta)
	{
		TrueValue = Mathf.Lerp(TrueValue, targetValue, 1 - Mathf.Pow(1 - Interpolation, delta));
		Value = TrueValue;
	}

	protected void OntargetValueChanged()
	{
		BarTexture newTexture = TexturePool.GetCurrentBar((targetValue - MinValue) / (MaxValue - MinValue));
		TextureUnder = newTexture.under;
		TextureOver = newTexture.over;
		TextureProgress = newTexture.progress;
	}
}
