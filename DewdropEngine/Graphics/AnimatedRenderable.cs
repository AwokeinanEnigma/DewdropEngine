namespace Dewdrop.Graphics
{
    public abstract class AnimatedRenderable : Renderable
    {
        protected float[] _speeds;
        protected int _totalFrames;
        protected float _currentFrame;
        protected float _speedModifier;
        protected float _speedIndex;

        public delegate void OnAnimationComplete(AnimatedRenderable renderable);
        public event OnAnimationComplete OnAnimationCompletedEvent;

        protected void OnAnimationCompleted()
        {
            OnAnimationCompletedEvent?.Invoke(this);
        }
    }
}
