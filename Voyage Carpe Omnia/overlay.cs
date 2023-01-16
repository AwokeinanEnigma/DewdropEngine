using Dewdrop;
using Dewdrop.Debugging;
using Dewdrop.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCO
{
    internal class overlay : Scene
    {
        public override void Unpause()
        {
            DBG.Log("focus");
        }

        public override void Load()
        {
            DBG.Log("load");
            SceneManager.Instance.CompositeMode = true;
            DrawBehind = true;
            //base.DrawBehind = true;
        }

        public override void Pause()
        {
            DBG.Log("pause");
        }

        public override void PreRender()
        {
            base.PreRender();
        }
        public override void Render()
        {
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
