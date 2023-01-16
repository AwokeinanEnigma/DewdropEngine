﻿using Dewdrop;
using Dewdrop.AssetLoading;
using Dewdrop.Graphics;
using Dewdrop.Scenes;
using Dewdrop.Scenes.Transitions;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace VCO
{
    public class VoyageCarpeOmnia : Engine
    {
        public static VoyageCarpeOmnia instance;
        private SpriteBatch _spriteBatch;
        
        public AssetBank<SpriteSheetTexture> assets;
        public AssetBank<Effect> shaders;

        public VoyageCarpeOmnia() : base(320, 180, false, 3)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Engine.RenderDebugUI += Engine_RenderDebugUI;
            SceneManager.Transition = new InstantTransition();
            Camera a = new Camera(Width, Height);
            base.Initialize();
        }

        private void Engine_RenderDebugUI(Dewdrop.DewGui.ImGuiRenderer renderer)
        {
            if (ImGui.Button("pop"))
            {
                SceneManager.Transition = new InstantTransition();
                SceneManager.Pop();
            }
            if (ImGui.Button("goto"))
            {
                SceneManager.CompositeMode = true;
                SceneManager.Push(new overlay());
                SceneManager.Transition = new InstantTransition();
            }
        }

        public event Action OnContentLoaded;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SceneManager.Push(new basic(_spriteBatch));   
 
            assets = new AssetBank<SpriteSheetTexture>("SpriteTexture");
            shaders = new AssetBank<Effect>("Shaders");
            OnContentLoaded?.Invoke();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}