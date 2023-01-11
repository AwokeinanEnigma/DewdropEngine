using Dewdrop.Utilities.fNbt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dewdrop.Graphics;
using System;
using System.Diagnostics;
using Dewdrop.Utilities;
using Dewdrop.ImGui;
using Microsoft.Xna.Framework.Content;
using Dewdrop.AssetLoading;
using System.IO;

namespace Dewdrop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D johnLemon;
        private SpriteTexture texture;

        private SpriteTexture textureTest;
        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get => _graphics;
        }
        public ContentManager ContentManager
        {
            get => Content;
        }

        public static Game1 instance { get; private set; }
        public Effect _effect;
        public ImGuiRenderer GuiRenderer; //This is the ImGuiRenderer
        public RenderTarget2D renderTarget;
        public AssetBank<SpriteTexture> sprites;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            instance = this;

            _graphics.PreferredBackBufferHeight = 180;
            _graphics.PreferredBackBufferWidth = 320;
            _graphics.ApplyChanges();
        }

        public SpriteTexture pal;
        public SpriteTexture[] test;
        private RenderPipeline pipeline;
        private Sprite iCG;
        private Sprite floyd;
        private Sprite zack;
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
         
            GuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();
            Logger.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _effect = Content.Load<Effect>("genericSpriteShader");
            Stopwatch watch = new Stopwatch();
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            pipeline = new RenderPipeline(_spriteBatch);

            sprites = new AssetBank<SpriteTexture>("IndexedTextures");
           iCG = new Sprite(sprites.GetAssetByName("greenhairedgirl_b"), _effect, "walk south", 10, 0, new Vector2(160, 90), "zack");
            floyd = new Sprite(sprites.GetAssetByName("floyd"), _effect, "walk south", 20, 0, new Vector2(160, 90), "zack");
            zack = new Sprite(sprites.GetAssetByName("zack"), _effect, "walk south", 30, 0, new Vector2(160, 90), "zack");

           pipeline.Add(iCG);
            pipeline.Add(floyd);
            pipeline.Add(zack);


            Logger.Log($"Loaded .gdat from xnb in {watch.ElapsedMilliseconds}ms");


            // TODO: use this.Content to load your game content here

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                iCG.Save();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                iCG.Palette = 0;
                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                iCG.Palette = 1;
                return;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                iCG.Palette = 2;
                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                iCG.Palette = 3;
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                iCG.Palette = 4;
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D6))
            {
                iCG.Palette = 5;
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D7))
            {
                iCG.Palette = 6;
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D8))
            {
                iCG.Palette = 7;
                return;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            pipeline.Draw();


            base.Draw(gameTime);

            GuiRenderer.BeginLayout(gameTime);

            ImGuiNET.ImGui.Text("i already do");
            //Insert Your ImGui code

            GuiRenderer.EndLayout();
        }
    }
}