using Godot;

namespace BossRush2;

public class Basis2D
{
	public PlayerController playerController;


    private static Vector2 worldBasis = new(0, 1);
    private Vector2 _upVector = worldBasis;
    public Vector2 upVector
    {
        get
        {
            return _upVector;
        }
        set
        {
            if (_upVector != value && playerController is not null)
            {
                if (playerController.active && playerController.camera is not null)
                {
                    playerController.camera.RotationDegrees = axisAngle;
                }
            }

            _upVector = value;
        }
    }
    public Vector2 rightVector
    {
        get
        {
            return _upVector.RotatedClockwiseDeg(90);
        }
        set
        {
            _upVector = value.RotatedClockwiseDeg(-90);
        }
    }
    /// <summary>
    /// Angle compared to worldBasis, in degrees
    /// </summary>
    public float axisAngle
    {
        get
        {
            return Mathf.RadToDeg(upVector.AngleTo(worldBasis));
        }
        set
        {
            upVector = worldBasis.RotatedClockwiseDeg(value);
        }
    }
    public Vector2 RotateControlVector(Vector2 control)
    {
        return ToWorldSystem(upVector.RotatedClockwise(control.Normalized().AngleTo(worldBasis)));
    }
    public static Vector2 ToWorldSystem(Vector2 basisVector)
    {
        return new(basisVector.X, -basisVector.Y);
    }
}
public static class Vector2Extendsion
{
    public static Vector2 RotatedClockwise(this Vector2 target, float rad)
    {
        return target.Rotated(-rad);
    }
    public static Vector2 RotatedClockwiseDeg(this Vector2 target, float deg)
    {
        return target.RotatedClockwise(Mathf.DegToRad(deg));
    }
}