using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace BossRush2;

/// <summary>
/// To be utilised by other functions here
/// </summary>
public enum TargetMode
{
    /// <summary>
    /// Placeholder for when targeting isn't used
    /// </summary>
    NONE,

    /// <summary>
    /// Target the player
    /// </summary>
    OWNER,

    /// <summary>
    /// Target what the owner of this node is targeting
    /// </summary>
    OWNER_TARGET,

    /// <summary>
    /// Target the nearest entity in one of the opposing teams
    /// </summary>
    PLAYER,

    /// <summary>
    /// Finds the closest enemies in the selected opposing teams
    /// </summary>
    NEAREST,

    /// <summary>
    /// Target the global mouse position
    /// </summary>
    MOUSE
}

public class Target : IBrObject
{
    Node2D TargetEntity;
    Vector2 TargetPosition = new();
    public bool emptyTarget = true;
    public Target() { }
    public Target(Node2D _TargetEntity)
    {
        TargetEntity = _TargetEntity;
        emptyTarget = false;
    }
    public Target(Vector2 _TargetPosition)
    {
        TargetPosition = _TargetPosition;
        emptyTarget = false;
    }
    public Vector2 GetTargetPosition()
    {
        if (TargetEntity is not null)
        {
            return TargetEntity.GlobalPosition;
        }
        return TargetPosition;
    }
    public Vector2? GetTargetPositionOrNull()
    {
        if (emptyTarget)
        {
            return null;
        }
        else
        {
            if (TargetEntity is not null)
            {
                return TargetEntity.GlobalPosition;
            }
            return TargetPosition;
        }

    }
    public float GetTargetDirection(Vector2 subject, float defaultRot)
    {
        if (emptyTarget)
        {
            return defaultRot;
        }
        else
        {
            Vector2 targPos = GetTargetPosition();
            return subject.AngleToPoint(targPos);
        }
    }
}