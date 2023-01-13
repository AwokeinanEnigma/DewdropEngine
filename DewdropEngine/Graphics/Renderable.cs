using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dewdrop.Graphics
{
    public abstract class Renderable : IDisposable
    {
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                OnVisiblityChanged();
            }
        }

        public virtual int Depth
        {
            get => _depth;
            set => _depth = value;
        }

        public virtual Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public virtual Vector2 Size
        {
            get => _size;
            set => _size = value;
        }

        public virtual Color Color
        {
            get => _color;
            set => _color = value;
        }

        public string name;

        protected int _depth;
        protected bool _visible;
        protected Vector2 _position;
        protected Vector2 _size;
        protected Color _color;

        protected bool hasDisposed;

        public abstract void Draw(SpriteBatch batch);

        protected virtual void OnVisiblityChanged() { }

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
