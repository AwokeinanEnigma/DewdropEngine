using Dewdrop.Scenes.Transitions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Dewdrop.Scenes
{
    /// <summary>
    /// Manager for scenes, handling transitions to new scenes and such.
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// This exception is thrown when the scene manager is empty and does not have any scenes.
        /// </summary>
        public class EmptySceneStackException : Exception
        {

            public EmptySceneStackException() : base($"The scene stack is empty!") { }
        }

        /// <summary>
        /// Manages the stack of scenes.
        /// </summary>
        private class SceneStack
        {
            private readonly List<Scene> list;

            public Scene this[int i]
            {
                get
                {
                    return this.list[i];
                }
            }

            /// <summary>
            /// The amount of scenes in the stack
            /// </summary>
            public int Count
            {
                get
                {
                    return this.list.Count;
                }
            }

            /// <summary>
            /// Creates a new scene stack.  
            /// </summary>
            public SceneStack()
            {
                this.list = new List<Scene>();
            }

            /// <summary>
            /// Clears the entire scene list
            /// </summary>
            public void Clear()
            {
                this.list.Clear();
            }

            /// <summary>
            /// Adds a scene to the top of the list
            /// </summary>
            /// <param name="scene">The scene to add to the top of the scene list</param>
            public void Push(Scene scene)
            {
                this.list.Add(scene);
            }

            /// <summary>
            /// Retrieves the topmost <see cref="Scene"/> object in the list, without removing it.
            /// </summary>
            /// <returns>The topmost <see cref="Scene"/> object in the list, or null if the list is empty.</returns>
            public Scene Peek()
            {
                return this.Peek(0);
            }

            /// <summary>
            /// Retrieves a <see cref="Scene"/> object from a specific position in the list, without removing it.
            /// </summary>
            /// <param name="i">The index of the <see cref="Scene"/> object to retrieve. The index is zero-based, and it is calculated from the top of the list.</param>
            /// <returns>The <see cref="Scene"/> object at the specified index, or null if the index is out of range.</returns>
            public Scene Peek(int i)
            {
                //if we're outside of the list of scenes
                if (i < 0 || i >= this.list.Count)
                {
                    return null;
                }

                // return the scene from the list
                return this.list[this.list.Count - i - 1];
            }

            /// <summary>
            /// Retrieves and removes the topmost entry in the list of <see cref="Scene"/> objects.
            /// </summary>
            /// <returns>The removed <see cref="Scene"/> object at the top of the list, or null if the list is empty.</returns>
            public Scene Pop()
            {
                // create result
                Scene result = null;

                if (this.list.Count > 0)
                {
                    // go to top of list
                    result = this.list[this.list.Count - 1];
                    // remove entry at top of list
                    this.list.RemoveAt(this.list.Count - 1);
                }

                // return scene
                return result;
            }
        }

        private enum State
        {
            Scene,
            Transition
        }

        #region Properties
        /// <summary>
        /// The active instance of the SceneManager
        /// </summary>
        public static SceneManager Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// The transition that is currently being used.
        /// </summary>
        public ITransition Transition
        {
            get => this.transition;
            set
            {
                this.transition = value;
            }
        }

        /// <summary>
        /// If true, the scene manager is currently transitioning between scenes
        /// </summary>
        public bool IsTransitioning
        {
            get
            {
                return this.state == State.Transition;
            }
        }

        /// <summary>
        /// If true, the scene manager is empty, if not, it isn't.
        /// </summary>
        public bool IsEmpty
        {
            get => this.isEmpty;
        }

        /// <summary>
        /// If true, the scene manager is drawing multiple scenes at once.
        /// </summary>
        public bool CompositeMode
        {
            get => this.isCompositeMode;
            set
            {
                this.isCompositeMode = value;
            }
        }
        #endregion

        #region Scene related fields
        private static SceneManager instance;

        private State state;

        private readonly SceneStack scenes;

        private Scene previousScene;

        private ITransition transition;
        #endregion

        #region Boolean fields.
        private bool isEmpty;

        // if popped then we'll completely clean up the previous scene
        private bool popped;

        // if true then the new scene is currently being shown
        private bool newSceneShown;

        // if false and we're transitioning we need to clean up our shit
        private bool cleanupFlag;

        // if true we're drawing multiple scenes
        private bool isCompositeMode;
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new scene manager.
        /// </summary>
        public SceneManager()
        {
            instance = this;
            // make new scenestack
            this.scenes = new SceneStack();
            // we have no scenes so we set this to true
            this.isEmpty = true;
            // no transition so just use the empty one
            this.transition = new InstantTransition();
            // even though we have no scenes, still set the scene state to Scene
            this.state = State.Scene;
        }

        /// <summary>
        /// Pushes a new scene to the stack
        /// </summary>
        /// <param name="newScene">The scene to push to the stack</param>
        public void Push(Scene newScene)
        {
            this.Push(newScene, false);
        }

        public void Push(Scene newScene, bool swap)
        {
            // if we have other scenes
            if (this.scenes.Count > 0)
            {
                this.previousScene = (swap ? this.scenes.Pop() : this.scenes.Peek());
                this.popped = swap;
            }
            // push this scene to the top
            this.scenes.Push(newScene);
            // transition
            this.SetupTransition();
            // we're not empty
            this.isEmpty = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Scene Pop()
        {
            if (this.scenes.Count > 0)
            {
                // get scene from the top
                Scene result = this.scenes.Pop();
                // we've popped!
                this.popped = true;

                // ???
                if (this.scenes.Count > 0)
                {
                    this.scenes.Peek();
                    this.SetupTransition();
                }
                // if we don't have any other scenes, we're empty
                else
                {
                    this.isEmpty = true;
                }
                this.previousScene = result;
                return result;
            }
            // if our scene list is empty, throw an exception
            throw new EmptySceneStackException();
        }

        private void SetupTransition()
        {
            // reset transition
            this.transition.Reset();
            // set our state
            this.state = State.Transition;

            // disable input
            // InputManager.Instance.Enabled = false;
        }

        public Scene Peek()
        {
            if (this.scenes.Count > 0)
            {
                return this.scenes.Peek();
            }
            throw new EmptySceneStackException();
        }

        /// <summary>
        /// Clears the scene list
        /// </summary>
        public void Clear()
        {
            Scene scene = this.scenes.Peek();
            while (this.scenes.Count > 0)
            {
                Scene scene2 = this.scenes.Pop();
                if (scene2 == scene)
                {
                    scene2.Pause();
                }
                scene2.Unload();
            }
        }

        public void Update(GameTime time)
        {
            this.UpdateScene(time);
            if (this.state == State.Transition)
            {
                this.UpdateTransition(time);
            }
        }

        private void UpdateScene(GameTime time)
        {
            if (this.scenes.Count > 0)
            {
                Scene scene = this.scenes.Peek();
                if (scene.canGo)
                {
                    scene.PreUpdate(time);
                    scene.Update(time);
                    scene.PostUpdate(time);
                }
                return;
            }
            throw new EmptySceneStackException();
        }

        private void UpdateTransition(GameTime time)
        {
            // if we haven't shown a new scene but you can
            if (!this.newSceneShown && this.transition.ShowNewScene)
            {
                // and if there is a previous scene
                if (this.previousScene != null)
                {
                    // if popped is true,
                    // then we completely dispose of the previous scene
                    if (this.popped)
                    {
                        this.previousScene.Unload();
                        this.previousScene.Dispose();
                        this.popped = false;
                    }
                    // if it isn't
                    else
                    {
                        // just unfocus it lmao
                        this.previousScene.Pause();
                    }
                }

                // get the new scene which is at the bottom of the list
                Scene scene = this.scenes.Peek();
                scene.canGo = true;
                // focus!
                if (scene.hasDisplayed)
                {
                    scene.Unpause();
                }
                else
                {
                    scene.Load();
                    scene.hasDisplayed = true;
                }
                //scene.hasDisplayed ? scene.Load(); : scene.Focus() ;

                // set previous scene to null
                this.previousScene = null;

                // our new scene is shown and we can finally tell the world.
                // lmao.
                this.newSceneShown = true;
            }

            // a transition can allow the scene manager to show a new scene without being complete
            // like for example, if you spawn a bunch of hit on screen for a transition
            // then half way through you want to show the new scene behind all that shit
            // and then remove all that shit to reveal the scene
            if (!this.transition.IsComplete)
            {
                // update transition
                this.transition.Update();

                // update previous scene if this transition isn't blocking
                if (!this.transition.Blocking && this.previousScene != null)
                {
                    previousScene.PreUpdate(time);
                    previousScene.Update(time);
                    previousScene.PostUpdate(time);
                }

                // progress ranges between 0 & 1
                // 0.5 is halfway
                // so what this is saying is "Okay if the transition is halfway done let's clean up our shit and make sure we don't clean up our shit again"
                if (this.transition.Progress > 0.5f && !this.cleanupFlag)
                {
                    // TextureManager.Instance.Purge();
                    GC.Collect();
                    // transition.Destroy();
                    this.cleanupFlag = true;
                    return;
                }
            }
            // but if the transition is complete, then we've already cleaned up our shit
            // and we can set the state of the Scene Manager to scene
            else
            {
                this.state = State.Scene;
                this.newSceneShown = false;

                //InputManager.Instance.Enabled = true;

                // make sure upon a new transition we can clean up our shit again
                this.cleanupFlag = false;
            }
        }

        public void AbortTransition()
        {
            if (this.state == State.Transition)
            {
                if (this.previousScene != null)
                {
                    //this.previousScene.Pause();
                    this.previousScene.Unload();
                    this.previousScene.Dispose();
                    this.previousScene = null;
                }
                if (!this.newSceneShown)
                {
                    Scene scene = this.scenes.Peek();
                    scene.Load();
                }
                this.state = State.Scene;
                this.newSceneShown = false;

                //InputManager.Instance.Enabled = true;

                this.cleanupFlag = false;
            }
        }

        private void CompositeRender()
        {
            int count = this.scenes.Count;
            for (int i = 0; i < count - 1; i++)
            {
                if (this.scenes[i + 1].DrawBehind)
                {
                    this.scenes[i].Render();
                    this.scenes[i].PostRender();
                }
            }
        }

        private void CompositePreRender()
        {
            int count = this.scenes.Count;
            for (int i = 0; i < count - 1; i++)
            {
                if (this.scenes[i + 1].DrawBehind)
                {
                    this.scenes[i].PreRender();
                }
            }
        }


        public void PreRender()
        {
            if (this.scenes.Count > 0)
            {
                if (this.transition.ShowNewScene)
                {
                    if (this.isCompositeMode)
                    {
                        this.CompositePreRender();
                    }
                    Scene scene = this.scenes.Peek();
                    scene.PreRender();
                }
                else if (this.previousScene != null)
                {
                    this.previousScene.PreRender();
                }
            }
        }


        public void Render()
        {
            if (this.scenes.Count > 0)
            {
                if (this.transition.ShowNewScene)
                {
                    if (this.isCompositeMode)
                    {
                        this.CompositeRender();
                    }
                    Scene scene = this.scenes.Peek();
                    scene.Render();
                    scene.PostRender();
                }
                else if (this.previousScene != null)
                {
                    this.previousScene.Render();
                    this.previousScene.PostRender();
                }
                if (!this.transition.IsComplete)
                {
                    this.transition.Draw();
                }
            }
        }
        #endregion
    }
}
