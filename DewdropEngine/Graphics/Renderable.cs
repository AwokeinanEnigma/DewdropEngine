using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dewdrop.Graphics
{
    /// <summary>
    /// A renderable is anything that can be rendered on the screen, like sprites and text.
    /// </summary>
    public abstract class Renderable : IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this renderable is visible.
        /// </summary>
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                OnVisibilityChanged();
            }
        }

        /// <summary>
        /// Gets or sets the depth of this renderable.
        /// </summary>
        public virtual int Depth
        {
            get => _depth;
            set => _depth = value;
        }

        /// <summary>
        /// Gets or sets the position of this renderable.
        /// </summary>
        public virtual Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        /// <summary>
        /// Gets or sets the size of this renderable.
        /// </summary>
        public virtual Vector2 Size
        {
            get => _size;
            set => _size = value;
        }

        /// <summary>
        /// Gets or sets the color of this renderable.
        /// </summary>
        public virtual Color Color
        {
            get => _color;
            set => _color = value;
        }

        public virtual Rectangle RenderableRectangle
        {
            get => _renderableRect;
            set => _renderableRect = value;
        }

        /// <summary>
        /// The name of this renderable
        /// </summary>
        public string name;

        protected int _depth;
        protected bool _visible;
        protected Vector2 _position;
        protected Vector2 _size;
        protected Color _color;
        protected Rectangle _renderableRect;

        protected bool hasDisposed;

        /// <summary>
        /// Draws this renderable using the specified sprite batch.
        /// </summary>
        /// <param name="batch">The sprite batch to use for drawing.</param>
        public abstract void Draw(SpriteBatch batch);

        /// <summary>
        /// Called when the visibility of the renderable changes
        /// </summary>
        protected virtual void OnVisibilityChanged() { }

        /// <summary>
        /// Releases all resources used by this renderable.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                hasDisposed = true;
            }
        }

        ~Renderable()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
