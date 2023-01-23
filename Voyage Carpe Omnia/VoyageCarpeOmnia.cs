using Dewdrop;
using Dewdrop.AssetLoading;
using Dewdrop.Graphics;
using Dewdrop.Scenes;
using Dewdrop.Scenes.Transitions;
using Dewdrop.StateMachines;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace VCO
{
    public class VoyageCarpeOmnia : Engine
    {
        private EntityStateMachine machine;
        public static VoyageCarpeOmnia instance;
        private SpriteBatch _spriteBatch;
        
        public AssetBank<SpriteSheetTexture> assets;
        public AssetBank<Effect> shaders;

        public VoyageCarpeOmnia() : base(320, 180, false, 3)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            instance = this;

            machine = new EntityStateMachine(null, "debug");
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Engine.RenderDebugUI += Engine_RenderDebugUI;
            Camera a = new Camera(Width, Height);
            machine.Initialize();
           // a.Position = new Vector2(20, 20);
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
            if (ImGui.Button("hehehe"))
            {
                SceneManager.CompositeMode = false;
                SceneManager.Push(new basic(_spriteBatch));
                SceneManager.Transition = new InstantTransition();
            }

            if (ImGui.Button("activate state machine"))
            {
                machine.SetStateInterrupt(new Stupido());
            }
        }

        public event Action OnContentLoaded;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
 
 
            assets = new AssetBank<SpriteSheetTexture>("SpriteTexture");
            shaders = new AssetBank<Effect>("Shaders");
            OnContentLoaded?.Invoke();

            SceneManager.Transition = new InstantTransition();
            SceneManager.Push(new basic(_spriteBatch));     // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            machine.Update();
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