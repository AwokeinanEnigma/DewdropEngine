using Dewdrop;
using Dewdrop.Debugging;
using Dewdrop.Graphics;
using Dewdrop.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCO
{
    internal class basic : Scene
    {
        RenderPipeline pipeline;
        public basic(SpriteBatch batch) 
        {
            pipeline = new RenderPipeline(batch);
            pipeline.Add(new Sprite(VoyageCarpeOmnia.instance.assets.GetAssetByName("greenhairedgirl_b"), VoyageCarpeOmnia.instance.shaders.GetAssetByName("gss"), "walk south", 100, 0, Vector2.Zero, "hehe"));
        }

        private void Instance_OnContentLoaded()
        {
        }

        public override void Unpause()
        {
            DBG.Log("focus");
            pipeline.ForEach(p => {
                if (p is AnimatedRenderable)
                {
                    ((AnimatedRenderable)p).AnimationEnabled = true;
                }
            });
        }

        public override void Load()
        {
            DBG.Log("load");
            //Engine.SceneManager.Push(new overlay());
            //Engine.SceneManager.CompositeMode = true;
        }

        public override void Pause()
        {
            DBG.Log("pause");
            pipeline.ForEach(p => {
                if (p is Sprite)
                {
                    ((Sprite)p).AnimationEnabled = false;
                }
            });
        }

        public override void PreRender()
        {
            base.PreRender();
        }
        public override void Render()
        {
            pipeline.Draw();
        }
        public override void PostRender()
        {
            base.PostRender();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
        }
        public override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);
        }

        public override void Unload()
        {
            base.Unload();
            DBG.Log("unload");
        }
    }
}
