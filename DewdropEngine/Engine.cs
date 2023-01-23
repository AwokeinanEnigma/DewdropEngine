using Dewdrop.Debugging;
using Dewdrop.DewGui;
using Dewdrop.Scenes;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dewdrop
{
    /// <summary>
    /// "You must be ahead to quit. Too many people quit when they’re behind instead of attempting to get ahead. Failure!"
    /// </summary>
    public class Engine : Game
    {
        public static GraphicsDeviceManager GraphicsManager;
        public ContentManager ContentManager
        {
            get => Content;
        }

        public static SceneManager SceneManager;

        /// <summary>
        /// Time since last frame. Adjusted by the time rate.
        /// </summary>
        public static float DeltaTime { get; private set; }

        /// <summary>
        /// Time since the last frame, unadjusted.
        /// </summary>
        public static float RawDeltaTime { get; private set; }
        
        public static int TimeRate;
        
        public static Engine instance { get; private set; }

        /// <summary>
        /// The width of the window
        /// </summary>
        public static int ViewWidth { get; private set; }

        /// <summary>
        /// The width of the window
        /// </summary>
        public static int ViewHeight { get; private set; }

        /// <summary>
        /// Padding
        /// </summary>
        public static int ViewPadding { get; private set; }

        /// <summary>
        /// The width of the game in pixels
        /// </summary>
        public static int Width { get; private set; }
        /// <summary>
        /// The width of the game in pixels
        /// </summary>
        public static int Height { get; private set; }


        public delegate void RenderImGui(ImGuiRenderer renderer);
        public static event RenderImGui RenderDebugUI;

        public ImGuiRenderer imGuiRenderer;

        public static Viewport Viewport { get; private set; }

        /// <summary>
        /// Represents the game's space. Render here.
        /// </summary>
        public static Matrix ScreenMatrix;

        // see the all new girl, rehearsed and well
        public Engine(int pixelWidth, int pixelHeight, bool allowResizing, int screenScale)
        {
            Width = pixelWidth;
            Height = pixelHeight;


            GraphicsManager = new GraphicsDeviceManager(this);

            // subscribe to events so we can resize the window when needed
            GraphicsManager.DeviceCreated += UpdateView;
            GraphicsManager.DeviceReset += UpdateView;
            

            //GraphicsManager.SynchronizeWithVerticalRetrace = true;
            GraphicsManager.PreferMultiSampling = false;
            GraphicsManager.GraphicsProfile = GraphicsProfile.HiDef;
            GraphicsManager.PreferredBackBufferFormat = SurfaceFormat.Color;
            GraphicsManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            GraphicsManager.PreferredBackBufferWidth = Width * screenScale;
            GraphicsManager.PreferredBackBufferHeight = Height * screenScale;
            GraphicsManager.ApplyChanges();

            Window.AllowUserResizing= allowResizing;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            //SetWindowed(pixelWidth, pixelHeight);
            instance = this;
            SceneManager = new SceneManager();
        }


        public static void SetWindowed(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                resizing = true;
                GraphicsManager.PreferredBackBufferWidth = width;
                GraphicsManager.PreferredBackBufferHeight = height;
                GraphicsManager.IsFullScreen = false;
                GraphicsManager.ApplyChanges();
                Console.WriteLine("WINDOW-" + width + "x" + height);
                resizing = false;
            }
        }

        private static bool resizing;
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            // initialize debug class
            DBG.Initialize();

            // initialize 
            imGuiRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();
            ImGuiStylePtr ptr =  ImGui.GetStyle();
            ptr.ChildRounding = 100;
            ptr.FrameRounding = 100;
            ptr.WindowRounding = 10;
            ptr.FrameRounding = 50;
            //style.Colors[(int)ImGuiCol.text] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
        }

        protected override void LoadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * TimeRate;

            SceneManager.Update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneManager.PreRender();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Viewport = Viewport;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SceneManager.Render();

            base.Draw(gameTime);

            imGuiRenderer.BeginLayout(gameTime);
            //she was young once, girl of the bohemian kind
            RenderDebugUI?.Invoke(imGuiRenderer);
           
            imGuiRenderer.EndLayout();
        }


        #region Window

        private void UpdateView(object sender, EventArgs e)
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
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0 && !resizing)
            {
                resizing = true;

                GraphicsManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
                GraphicsManager.PreferredBackBufferHeight = Window.ClientBounds.Height;
                UpdateView(null, null);

                resizing = false;
            }
        }
        #endregion
    }
}