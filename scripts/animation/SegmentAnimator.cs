using Godot;
using System;

namespace BrAnimator;

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
    public bool loop = false;

    /// <summary>
    /// A temporary version of Loop, meant to be called without interfering with config
    /// </summary>
    protected bool stopAtNext = false;

    /// <summary>
    /// How much time in seconds the animation will take
    /// </summary>
    [Export]
    public float animationTime = 1f;

    /// <summary>
    /// The point that is the animation is at
    /// </summary>
    protected float animationStep = 0f;

    public bool Process = false;
    public Node2D subject;

    /// <summary>
    /// Starts the animation, restarts if called mid animation
    /// </summary>
    public virtual void StartAnimation()
    {
        animationStep = 0f;
        Process = true;
    }

    /// <summary>
    /// Stops the animation
    /// </summary>
    public virtual void StopAnimation(bool immediate)
    {
        if (immediate)
        {
            animationStep = 0f;
            Process = true;
        }
        else
        {
            stopAtNext = true;
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
        animationStep += deltaF;
        if (animationStep > animationTime)
        {
            if (loop && !stopAtNext)
            {
                animationStep -= animationTime;
            }
            else
            {
                animationStep = 0f;
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
