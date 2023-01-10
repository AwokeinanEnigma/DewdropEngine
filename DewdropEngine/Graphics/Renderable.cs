using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.Graphics
{
    public abstract class Renderable : IDisposable
    {
        public bool Visible
        {
            get => _visible;
            set {
                _visible = value;
                OnVisiblityChanged();
            }
        }

        public int Depth
        {
           get => _depth; 
           set => _depth = value;
        }

        public Vector2 Position { 
            get => _position;
            set => _position = value;
        }


        private int _depth;
        private bool _visible;
        private Vector2 _position;

        private bool hasDisposed;

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
