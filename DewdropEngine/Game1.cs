using fNbt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DewdropEngine.Graphics;
using System.IO;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace DewdropEngine 
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D johnLemon;
        private IndexedTexture2D texture;
        public GraphicsDeviceManager GraphicsDeviceManager { 
            get => _graphics;
        }
        public static Game1 instance { get; private set; }
        private Effect _effect;
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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        protected override void LoadContent()
        {
            long tickas = DateTime.Now.Ticks;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _effect = Content.Load<Effect>("genericSpriteShader");
                johnLemon = Content.Load<Texture2D>("john lemon");
            Console.WriteLine($"in {(DateTime.Now.Ticks - tickas) / 10000L}ms");

            long ticks = DateTime.Now.Ticks;
            NbtFile nbtFile = new NbtFile("C:\\Users\\Tom\\Documents\\VoyageCarpeOmnia\\VoyageCO\\bin\\Release\\Data\\Graphics\\greenhairedgirl.dat");
            NBTImageLoader.LoadFromNbtTag(nbtFile.RootTag);
            this.texture = NBTImageLoader.LoadFromNbtTag(nbtFile.RootTag);
            Console.WriteLine($"in {(DateTime.Now.Ticks - ticks) / 10000L}ms");

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
            _effect.Parameters["img"].SetValue(texture.Image);
            _effect.Parameters["pal"].SetValue(texture.Palette);

            _effect.Parameters["palIndex"].SetValue(texture.CurrentPaletteFloat);
            _effect.Parameters["palSize"].SetValue(texture.PaletteSize);
            _effect.Parameters["blend"].SetValue(Color.BlueViolet.ToVector4());
            _effect.Parameters["blendMode"].SetValue(1);

            SpriteDefinition def = texture.GetRandomSpriteDefinition();
            _spriteBatch.Begin(effect: _effect);
            _spriteBatch.Draw(
                texture.Image, 
                Vector2.Zero, 
                new Rectangle((int)def.Coords.X, (int)def.Coords.Y, (int)def.Bounds.X, (int)def.Bounds.Y),
                Color.White
                );
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}