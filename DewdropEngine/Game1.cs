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

namespace Dewdrop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D johnLemon;
        private IndexedTexture texture;

        private IndexedTexture textureTest;
        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get => _graphics;
        }
        public ContentManager ContentManager
        {
            get => Content;
        }

        public static Game1 instance { get; private set; }
        private Effect _effect;
        public ImGuiRenderer GuiRenderer; //This is the ImGuiRenderer

        private AssetBank<IndexedTexture> sprites;
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

        public IndexedTexture pal;
        public IndexedTexture[] test;

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


            sprites = new AssetBank<IndexedTexture>("IndexedTextures", System.IO.SearchOption.AllDirectories);
            watch.Restart();
            textureTest = sprites.GetAssetByName("greenhairedgirl_b");
            watch.Stop();
            Logger.Log($"Loaded .gdat from xnb in {watch.ElapsedMilliseconds}ms");


            // TODO: use this.Content to load your game content here

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
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            _effect.Parameters["img"].SetValue(textureTest.Texture);
            _effect.Parameters["pal"].SetValue(textureTest.Palette);

            _effect.Parameters["palIndex"].SetValue(0);
            _effect.Parameters["palSize"].SetValue(textureTest.PaletteSize);
            _effect.Parameters["blend"].SetValue(Color.BlueViolet.ToVector4());
            _effect.Parameters["blendMode"].SetValue(1);

            SpriteDefinition def = textureTest.GetDefaultSpriteDefinition();
            _spriteBatch.Begin(effect: _effect);
            _spriteBatch.Draw(
                textureTest.Texture, 
                Vector2.Zero, 
                new Rectangle((int)def.Coords.X, (int)def.Coords.Y, (int)def.Bounds.X, (int)def.Bounds.Y),
                Color.White
                );
            _spriteBatch.End();

            base.Draw(gameTime);

            GuiRenderer.BeginLayout(gameTime);

            ImGuiNET.ImGui.Text("i already do");
            //Insert Your ImGui code

            GuiRenderer.EndLayout();
        }
    }
}