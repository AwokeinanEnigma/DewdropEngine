using fNbt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DewdropEngine.Graphics;
using System.IO;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Dewdrop.ImGui;
using ImGuiNET;

namespace DewdropEngine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D johnLemon;
        private IndexedTexture2D texture;
        public GraphicsDeviceManager GraphicsDeviceManager
        {
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
            // TODO: Add your initialization     logic here
            GuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();

            Color[] colors = new Color[]
{
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Yellow,
                Color.Purple,
};

            // Compress the array of Microsoft.XNA.Framework.Color into a single integer
            int compressedData = 0;
            foreach (Color color in colors)
            {
                // Pack the red, green, and blue components into the lower 12 bits of the integer
                compressedData |= (color.R >> 4) << 8;
                compressedData |= (color.G >> 4) << 4;
                compressedData |= color.B >> 4;

                // Pack the alpha component into the upper 4 bits of the integer
                compressedData |= (color.A >> 6) << 12;
                Console.WriteLine(color);
            }
            Console.WriteLine($"orig l: {colors.Length}");

            Console.WriteLine($"---");

            // Decompress the single integer back into an array of Microsoft.XNA.Framework.Color
            Color[] decompressedColors = new Color[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                // Extract the red, green, and blue components from the lower 12 bits of the integer
                int r = (compressedData >> 8) & 0xF;
                int g = (compressedData >> 4) & 0xF;
                int b = compressedData & 0xF;

                // Extract the alpha component from the upper 4 bits of the integer
                int a = (compressedData >> 12) & 0x3;

                // Create a new Microsoft.XNA.Framework.Color from the extracted components
                decompressedColors[i] = new Color((byte)(r << 4), (byte)(g << 4), (byte)(b << 4), (byte)(a << 6));

                // Create a new Microsoft.XNA.Framework.Color from the extracted components
                decompressedColors[i] = new Color(r, g, b, a);
                Console.WriteLine(colors[i]);
            }
            Console.WriteLine($"new l: {decompressedColors.Length}");

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

        public ImGuiRenderer GuiRenderer;

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


            GuiRenderer.BeginLayout(gameTime);

            ImGui.Text("SUPER SWAG ");
            ImGui.Text("swag messiah");
            //Insert Your ImGui code

            GuiRenderer.EndLayout();
        }
    }
}