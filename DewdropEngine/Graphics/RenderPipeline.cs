using Dewdrop.Debugging;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dewdrop.Graphics
{
    /// <summary>
    /// A render pipeline sorts renderables by their depth. Renderables with a higher depth are rendered over those with a lower depth. 
    /// Additionally (and most importantly) it allows renderables to draw onto a SpriteBatch.
    /// </summary>
    public class RenderPipeline
    {
        private class RenderableComparer : IComparer<Renderable>
        {
            //used for uids
            private RenderPipeline pipeline;

            /// <summary>
            /// Creates a new renderable comparer using a renderpipeline
            /// </summary>
            /// <param name="pipeline">The render pipeline to use</param>
            public RenderableComparer(RenderPipeline pipeline)
            {
                this.pipeline = pipeline;
            }

            // i'll break this down
            //
            // if our depths aren't equal to each other
            // then subtract their depths to see which one is greater
            //
            // but if they are not
            // then subtract their ids to see which one is greater.
            public int Compare(Renderable a, Renderable b)
            {
                return a.Depth != b.Depth ? a.Depth - b.Depth : this.pipeline._renderableIds[b] - this.pipeline._renderableIds[a];
            }
        }

        #region Properties
        /// <summary>
        /// The sprite batch that this render pipeline is using.
        /// </summary>
        public SpriteBatch Target
        {
            get
            {
                return this._target;
            }
        }

        public int PipelineDepth {
            get => _depth;
            set => _depth = value;
        }
        #endregion

        #region Fields
        private int _depth;

        // the sprite batch to draw to. this what every renderable is drawing to
        private SpriteBatch _target;

        // the renderables in this mothafuckin' pipeline!
        private List<Renderable> _renderables;

        // stacks to determine what renderables to add and remove
        private Stack<Renderable> _renderablesToAdd;
        private Stack<Renderable> _renderablesToRemove;

        // called through Update(), if true then we need to use RenderableComparer to sort our list of renderables
        private bool _needToSort;

        // compares renderables
        private RenderableComparer _depthComparer;

        // when we can't sort by depth, we sort by renderable ids. this is determined when a renderable is added
        private Dictionary<Renderable, int> _renderableIds;

        // how many renderables this pipeline is rendering rendering 
        private int _renderableCount;
        #endregion 

        /// <summary>
        /// Creates a new RenderPipeline using a sprite batch
        /// </summary>
        /// <param name="target">The sprite batch to draw to</param>
        public RenderPipeline(SpriteBatch target, int depth = 0)
        {
            // set target
            this._target = target;
            // create list of renderables
            this._renderables = new List<Renderable>();

            // create stack of renderables (wowie!)
            this._renderablesToAdd = new Stack<Renderable>();
            this._renderablesToRemove = new Stack<Renderable>();

            this._renderableIds = new Dictionary<Renderable, int>();

            this._depthComparer = new RenderableComparer(this);

            _depth = depth;
        }

        /// <summary>
        /// Adds a renderable object to the render pipeline's list of renderables to be drawn.
        /// </summary>
        /// <param name="renderable">The renderable to add.</param>
        public virtual void Add(Renderable renderable)
        {
            // if we don't already have this renderable in this pipeline
            if (_renderables.Contains(renderable))
            {
                DBG.LogError("Tried to add renderable that already exists in the RenderPipeline.", null);
                return;
            }
            this._renderablesToAdd.Push(renderable);
        }

        /// <summary>
        /// Adds a list of renderables to the render pipeline's list of renderables to be drawn.
        /// </summary>
        /// <param name="renderablesToAdd">List of renderables to add.</param>
        public virtual void AddAll(List<Renderable> renderablesToAdd)
        {
            renderablesToAdd.ForEach(x => Add(x));
        }

        /// <summary>
        /// Adds an array of renderables to the render pipeline's list of renderables to be drawn.
        /// </summary>
        /// <param name="renderablesToAdd">Array of renderables to add.</param>
        public virtual void AddAll(Renderable[] renderablesToAdd)
        {
            for (int i = 0; i < renderablesToAdd.Length; i++)
            {
                Add(renderablesToAdd[i]);
            }
        }

        /// <summary>
        /// Removes a renderable from the render pipeline's list of renderables to be drawn.
        /// </summary>
        /// <param name="renderable">The renderable to remove.</param>
        public virtual void Remove(Renderable renderable)
        {
            if (renderable != null)
            {
                this._renderablesToRemove.Push(renderable);
            }
        }

        /// <summary>
        /// Forces the render pipeline to sort again
        /// </summary>
        public virtual void ForceSort()
        {
            this._needToSort = true;
        }

        protected virtual void DoAdditions()
        {
            while (this._renderablesToAdd.Count > 0)
            {
                // remove the thing from the top of this
                Renderable key = this._renderablesToAdd.Pop();

                // add it to the list
                this._renderables.Add(key);

                // determine its renderable ID
                this._renderableIds.Add(key, this._renderableCount);

                // force our render pipeline to sort renderables after adding
                this._needToSort = true;

                ++this._renderableCount;
            }
        }

        protected virtual void DoRemovals()
        {
            while (this._renderablesToRemove.Count > 0)
            {
                Renderable key = this._renderablesToRemove.Pop();
                this._renderables.Remove(key);
                this._renderableIds.Remove(key);

                // unlike DoAdditions, we don't need to force sort our renderables again
                // this is pretty obvious, but you don't need to sort again if something was removed 
            }
        }

        /// <summary>
        /// Executes a function for every renderable in the renderer.
        /// </summary>
        /// <param name="forEachFunc">The function to execute for each renderable. The renderable is passed as a parameter to the function.</param>
        public virtual void ForEach(Action<Renderable> forEachFunc)
        {
            _renderables.ForEach(x => forEachFunc(x));
        }


        /// <summary>
        /// Removes all renderables from the renderer, disposing of them.
        /// </summary>
        public void Clear()
        {
            this.Clear(true);
        }

        /// <summary>
        /// Removes all renderables from the renderer.
        /// </summary>
        /// <param name="dispose">If true, the dispose method of every renderable will be called before they are removed.</param>
        public void Clear(bool dispose)
        {
            this._renderablesToRemove.Clear();
            if (dispose)
            {
                foreach (Renderable renderable in this._renderables)
                {
                    renderable.Dispose();
                }
            }
            this._renderables.Clear();

            if (dispose)
            {
                while (this._renderablesToAdd.Count > 0)
                {
                    this._renderablesToAdd.Pop().Dispose();
                }
            }
            this._renderablesToAdd.Clear();
            _renderableIds.Clear();
        }

        /// <summary>
        /// Determines whether or not a renderable is in view. Used to determine whether or not a renderable should be drawn.
        /// </summary>
        /// <param name="renderable">The renderable to check.</param>
        /// <returns>If true, the renderable is in view. If false, it isn't.</returns>
        protected virtual bool IsRenderableInView(Renderable renderable) {
            // if the renderable is visible and it's in the view of the camera
            return renderable.Visible && Camera.Instance.Viewport.Bounds.Intersects(renderable.RenderableRectangle);
        }

        public virtual void Draw()
        {
            this.DoAdditions();
            this.DoRemovals();
            if (this._needToSort)
            {
                this._renderables.Sort(_depthComparer);
                this._needToSort = false;
            }

            int count = this._renderables.Count;
            // go through each renderable
            for (int index = 0; index < count; ++index)
            {
                // get renderable at index
                Renderable renderable = this._renderables[index];

                // if the renderable is visible, allow it to draw
                if (IsRenderableInView(renderable))
                {
                    renderable.Draw(this._target);

                }
            }
        }
    }
}
