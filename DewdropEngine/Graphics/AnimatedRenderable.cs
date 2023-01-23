using Dewdrop.Graphics;

/// <summary>
/// This class provides fields and an animation completion event for renderables that are animated.
/// </summary>
public abstract class AnimatedRenderable : Renderable
{
    /// <summary>
    /// The speeds of the animation
    /// </summary>
    protected float[] _speeds;

    /// <summary>
    /// Total number of frames in the animation
    /// </summary>
    protected int _totalFrames;

    /// <summary>
    /// The current frame of the animation
    /// </summary>
    protected float _currentFrame;

    /// <summary>
    /// The modifier to change the animation speed
    /// </summary>
    protected float _speedModifier;

    /// <summary>
    /// The index of the current speed in the speeds array
    /// </summary>
    protected float _speedIndex;

    /// <summary>
    /// Gets or sets a value indicating whether the animation is enabled
    /// </summary>
    public virtual bool AnimationEnabled
    {
        get => _animationEnabled;
        set => _animationEnabled = value;
    }
    protected bool _animationEnabled = true;

    /// <summary>
    /// Delegate for the animation completion event
    /// </summary>
    public delegate void OnAnimationComplete(AnimatedRenderable renderable);

    /// <summary>
    /// Event that is raised when the animation is completed
    /// </summary>
    public event OnAnimationComplete OnAnimationCompletedEvent;

    /// <summary>
    /// Raises the animation completed event
    /// </summary>
    protected void OnAnimationCompleted()
    {
        OnAnimationCompletedEvent?.Invoke(this);
    }
}