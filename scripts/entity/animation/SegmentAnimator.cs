using Godot;
using System;

namespace BossRush2;

/// <summary>
/// An abstract base class for every animation pivot in game
/// </summary>
[GlobalClass]
public abstract partial class SegmentAnimator : Resource
{
    /// <summary>
    /// Whether or not the animation will repeat
    /// </summary>
    [Export]
    public bool Loop = false;

    /// <summary>
    /// A temporary version of Loop, meant to be called without interfering with config
    /// </summary>
    protected bool StopAtNext = false;

    /// <summary>
    /// How much time in seconds the animation will take
    /// </summary>
    [Export]
    public float AnimationTime = 1f;

    /// <summary>
    /// The point that is the animation is at
    /// </summary>
    protected float AnimationStep = 0f;

    public bool Process = false;
    public Node2D Subject;

    /// <summary>
    /// Starts the animation, restarts if called mid animation
    /// </summary>
    public virtual void StartAnimation()
    {
        AnimationStep = 0f;
        Process = true;
    }

    /// <summary>
    /// Stops the animation
    /// </summary>
    public virtual void StopAnimation(bool immediate)
    {
        if (immediate)
        {
            AnimationStep = 0f;
            Process = true;
        }
        else
        {
            StopAtNext = true;
        }
    }

    /// <summary>
    /// Called on each frame of animation
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="deltaF"></param>
    public abstract void OnAnimationStep(double delta, float deltaF);

    public void StepAnimation(double delta)
    {
        if (!Process){ return; }
        
        float deltaF = (float)delta;
        AnimationStep += deltaF;
        if (AnimationStep > AnimationTime)
        {
            if (Loop && !StopAtNext)
            {
                AnimationStep -= AnimationTime;
            }
            else
            {
                AnimationStep = 0f;
                OnAnimationStep(delta, deltaF);
                Process = false;
            }
        }
        else
        {
            OnAnimationStep(delta, deltaF);
        }
    }
}
