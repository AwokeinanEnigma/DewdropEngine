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
        /// If two scenes are being rendered, and this is the scene being drawn on, when that second scene drawing on top goes away, this method will be called
        /// </summary>
        public abstract void Unpause();

        /// <summary>
        /// Called before the update.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PreUpdate(GameTime gameTime) { }
        
        /// <summary>
        /// This is the update method, which is called every frame, like every other update function. However, it's best to do things that run every frame here.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
        
        /// <summary>
        /// Called after the update method, do any cleanup you need to do here.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime) { }

        /// <summary>
        /// Called before the render method, if you need to do any effects, set them up here
        /// </summary>
        public virtual void PreRender() { }
        
        /// <summary>
        /// This is the render method, draw stuff here.
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Called after the render method, you can do any post-processing here.
        /// </summary>
        public virtual void PostRender() { }

        /// <summary>
        /// This is used for the composite mode, where it'll draw multiple scenes at once. Pause game stuff here.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// If you need to 
        /// </summary>
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
