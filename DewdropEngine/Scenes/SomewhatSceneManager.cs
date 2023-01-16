using Dewdrop.Scenes.Transitions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.Scenes
{
    public class SomewhatSceneManager
    {
        private enum DisplayMode
        {
            Composite,
            Single,
        }

        private enum SceneKillmode
        {
            DisposePreviousScene,

        }

        private enum SceneState
        {
            Transitioning,
            InScene,
        }

        private Scene currentScene;
        public Scene nextScene;

        private SceneState currentState;

        private ITransition currentTransition;
        private bool showCurrentScene;
 
        public void PushScene(Scene scene, ITransition transition)
        {
            currentTransition = transition;
            nextScene = scene;

            currentState = SceneState.Transitioning;
        }

        private void UpdateScene(GameTime time)
        {

            currentScene.PreUpdate(time);
            currentScene.Update(time);
            currentScene.PostUpdate(time);

        }

        private void UpdateTransition()
        {
            if (!showCurrentScene && currentTransition.ShowNewScene)
            {
                if (currentTransition.Progress >= 0.5f)
                {
                    nextScene.Load();
                }
                currentScene.Unload();
                currentScene.Dispose();
                currentScene = nextScene;

                currentState = SceneState.InScene;
            }
        }

        public void Update(GameTime time)
        {
            switch (currentState)
            {
                case SceneState.Transitioning:
                    UpdateTransition();
                    break;

                case SceneState.InScene:
                    UpdateScene(time);
                    break;
            }
        }

        public void Draw() {

            currentScene.PreRender();
            currentScene.Render();
            currentScene.PostRender();
        }
    }
}
