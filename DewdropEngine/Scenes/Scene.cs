using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.Scenes
{
    public abstract class Scene : IDisposable
    {

        public bool DrawBehind
        {
            get => drawBehind;
            set => drawBehind = value;
        }
        public bool canGo;
        public bool hasDisplayed;
        private bool drawBehind;

        protected bool hasDisposed;

        /// <summary>
        /// Called when the scene is first loaded.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// If two scenes are being rendered,
        /// </summary>
        public abstract void Focus();

        public virtual void PreUpdate(GameTime gameTime) { }
        public abstract void Update(GameTime gameTime);
        public virtual void PostUpdate(GameTime gameTime) { }

        public virtual void PreRender() { }
        public abstract void Render();
        public virtual void PostRender() { }


        public abstract void Pause();

        public virtual void Unload() { }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Scene()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
        public virtual void Dispose(bool disposing)
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
