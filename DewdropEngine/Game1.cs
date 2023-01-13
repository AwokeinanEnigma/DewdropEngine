using Dewdrop.AssetLoading;
using Dewdrop.DewGui;
using Dewdrop.Graphics;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Dewdrop
{
    public class Engine : Game
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
        // time
        public static float DeltaTime { get; private set; }
        public static float RawDeltaTime { get; private set; }

        public static Engine instance { get; private set; }
        public Effect _effect;
        public ImGuiRenderer GuiRenderer; //This is the ImGuiRenderer
        public RenderTarget2D renderTarget;
        public AssetBank<SpriteTexture> sprites;
        public static float TimeRate = 1f;

        public static int Width = 320;
        public static int Height = 180;
        public static int ViewWidth { get; private set; }
        public static int ViewHeight { get; private set; }

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            /* if (true)
             {
                 _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                 _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                 _graphics.IsFullScreen = true;
             }*/
            _graphics.ApplyChanges(); Content.RootDirectory = "Content";

            IsMouseVisible = true;
            instance = this;

            _graphics.PreferredBackBufferHeight = 180;
            _graphics.PreferredBackBufferWidth = 320;
            _graphics.ApplyChanges();
        }

        public static Viewport Viewport { get; private set; }
        public static Matrix ScreenMatrix;


        public SpriteTexture pal;
        public SpriteTexture[] test;
        private RenderPipeline pipeline;
        private Sprite iCG;
        private Sprite floyd;
        private Sprite zack;
        public static int ViewPadding
        {
            get { return viewPadding; }
            set
            {
                viewPadding = value;
                instance.UpdateView();
            }
        }
        private static int viewPadding = 0;
        private static bool resizing;
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            GuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();
            Logger.Initialize();

            /*resizing = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            Console.WriteLine("FULLSCREEN");
            resizing = false;*/
            UpdateView();
        }

        Camera cam;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _effect = Content.Load<Effect>("genericSpriteShader");
            Stopwatch watch = new Stopwatch();
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            pipeline = new RenderPipeline(_spriteBatch);
            cam = new Camera(320, 180);
            //UpdateView();

            if (true)
            {


            }

            sprites = new AssetBank<SpriteTexture>("IndexedTextures");
            iCG = new Sprite(sprites.GetAssetByName("greenhairedgirl_b"), _effect, "walk south", 10, 0, new Vector2(160, 90), "zack");
            floyd = new Sprite(sprites.GetAssetByName("floyd"), _effect, "walk south", 20, 0, new Vector2(160, 90), "zack");
            zack = new Sprite(sprites.GetAssetByName("zack"), _effect, "walk south", 30, 0, new Vector2(160, 90), "zack");

            pipeline.Add(iCG);
            pipeline.Add(floyd);
            pipeline.Add(zack);

            array = new byte[] {


            };
            Logger.Log($"Loaded .gdat from xnb in {watch.ElapsedMilliseconds}ms");


            // TODO: use this.Content to load your game content here

            // TODO: use this.Content to load your game content here
        }

        public void SetWindowed(int width, int height)
        {
#if !CONSOLE
            if (width > 0 && height > 0)
            {
                resizing = true;
                _graphics.PreferredBackBufferWidth = width;
                _graphics.PreferredBackBufferHeight = height;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                Console.WriteLine("WINDOW-" + width + "x" + height);
                resizing = false;
            }
#endif
        }
        private void UpdateView()
        {
            float screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            float screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            // get View Size
            if (screenWidth / Width > screenHeight / Height)
            {
                ViewWidth = (int)(screenHeight / Height * Width);
                ViewHeight = (int)screenHeight;
            }
            else
            {
                ViewWidth = (int)screenWidth;
                ViewHeight = (int)(screenWidth / Width * Height);
            }

            // apply View Padding
            var aspect = ViewHeight / (float)ViewWidth;
            ViewWidth -= ViewPadding * 2;
            ViewHeight -= (int)(aspect * ViewPadding * 2);

            // update screen matrix
            ScreenMatrix = Matrix.CreateScale(ViewWidth / (float)Width);

            // update viewport
            Viewport = new Viewport
            {
                X = (int)(screenWidth / 2 - ViewWidth / 2),
                Y = (int)(screenHeight / 2 - ViewHeight / 2),
                Width = ViewWidth,
                Height = ViewHeight,
                MinDepth = 0,
                MaxDepth = 1
            };

            //Debug Log
            //Calc.Log("Update View - " + screenWidth + "x" + screenHeight + " - " + viewport.Width + "x" + viewport.GuiHeight + " - " + viewport.X + "," + viewport.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * TimeRate;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
        byte[] array;
        public string input = "";
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            pipeline.Draw();


            base.Draw(gameTime);

            GuiRenderer.BeginLayout(gameTime);

            if (ImGui.Button("GC.Collect"))
            {
                GC.Collect();
            };
            if (ImGui.InputTextWithHint("entity to remove", "removes an entity", ref input, 256))
            {
                Logger.Log("aaa");
            }
            if (ImGui.Button("Dispose"))
            {

                pipeline.Remove(zack);
                zack.Dispose();

                pipeline.Remove(floyd);
                floyd.Dispose();
            };
            if (ImGui.Button("Access"))
            {
                //iCG.SwitchSprite("heh");

                pipeline.Add(new Sprite(sprites.GetAssetByName("zack"), _effect, "walk south", 30, 0, new Vector2(new Random().Next(0, 320), new Random().Next(0, 180)), "zack"));
            };


            for (int i = 1; i < 6; i++)
            {
                if (ImGui.Button($"screen scale: {i}"))
                {
                    //  pipeline.Add( new Sprite(sprites.GetAssetByName("zack"), _effect, "walk south", 30, 0, new Vector2(160 + 3, 90 + 3), "zack") );
                    SetWindowed(320 * i, 180 * i);
                    UpdateView();
                }
            }
            ImGui.Text($"gcmb: {GC.GetTotalMemory(true)}");
            //Insert Your ImGui code

            GuiRenderer.EndLayout();
        }
    }
}