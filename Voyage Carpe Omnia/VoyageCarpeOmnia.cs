using Dewdrop;
using Dewdrop.AssetLoading;
using Dewdrop.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VCO
{
    public class VoyageCarpeOmnia : Engine
    {
        private SpriteBatch _spriteBatch;
        private AssetBank<SpriteSheetTexture> assets;
        private AssetBank<Effect> shaders;
        private RenderPipeline pipeline;

        public VoyageCarpeOmnia() : base(320, 180, false, 3)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            assets = new AssetBank<SpriteSheetTexture>("SpriteTexture");
            shaders = new AssetBank<Effect>("Shaders");

            Camera a = new Camera(Width, Height);   

            pipeline = new RenderPipeline(_spriteBatch);
            pipeline.Add(new Sprite(assets.GetAssetByName("greenhairedgirl_b"), shaders.GetAssetByName("gss"), "walk south", 100, 0, Vector2.Zero, "hehe"));
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

            pipeline.Draw();
        }
    }
}