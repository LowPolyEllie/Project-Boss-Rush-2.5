using Godot;
using System;

namespace BossRush2;

/// <summary>
/// Hell :3
/// </summary>
public static class ExtraMath
{
	/// <summary>
	/// Assuming a constant acceleration and friction ratio, predicts velocity after a certain amount of time
	/// </summary>
	/// <remarks>
	/// <br> I went through hell and back to get this function to work properly </br>
	/// </remarks>
	public static float PredictVelocity(float vel, float acc, float frict, float time)
	{
		float frictB = Mathf.Pow(frict, time);
		return (frictB * vel) + acc * (frictB - 1) / (frict - 1);
	}

	/// <summary>
	/// Assuming a constant acceleration and friction ratio, predicts velocity after a certain amount of time
	/// </summary>
	/// <remarks>
	/// <br> I went through hell and back to get this function to work properly </br>
	/// </remarks>
	public static Vector2 PredictVelocity(Vector2 vel, Vector2 acc, float frict, float time)
	{
		return new(
			PredictVelocity(vel.X, acc.X, frict, time),
			PredictVelocity(vel.Y, acc.Y, frict, time)
		);
	}

	/// <summary>
	/// Godot why the fuck didn't you just add this for floats as well
	/// </summary>
	public static float RandRange(float min, float max)
	{
		return GD.Randf() * (max - min) + min;
	}

	/// <summary>
	/// GD.RandRange, but for Vectors
	/// </summary>
	public static Vector2 RandVector(Vector2 min, Vector2 max)
	{
		return new(
			GD.Randf() * (max.X - min.X) + min.X,
			GD.Randf() * (max.Y - min.Y) + min.Y
		);
	}
}
