using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokeU.View;
using PokeU.View.ElementLandObject;
using PokeU.View.ResourcesManager;
using PokeU.View.WaterObject;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.IO;
using WorldGeneration.WorldGenerating;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace PokeU
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public static readonly int MODEL_TO_VIEW = 16;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private WorldMonitor landWorld;

        private LandWorld2D landWorld2D;

        private KeyboardState oldState;
        public MainGame()
        {
            this.landWorld = new WorldMonitor(32, 16, 123456789);
            this.landWorld.InitWorldMonitor();

            // TODO static constructor need that
            WaterObject2D waterObject2D = new WaterObject2D();

            graphics = new GraphicsDeviceManager(this);

            DisplayMode currentDisplay = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;

            //graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            this.landWorld2D = new LandWorld2D(this.landWorld, this.GraphicsDevice.Viewport);//new Viewport(1920/2, 0, 1920/2, 1080));
            LandWorld2D.TextureManager = new TextureManager(this.GraphicsDevice);
            LandWorld2D.TextureManager.MainGame = this;
            this.landWorld2D.RegisterFactoryEvents();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            oldState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //ballTexture = Content.Load<Texture2D>(@"Autotiles\treeSwamp");
            //ballTexture2 = Content.Load<Texture2D>(@"Autotiles\tree");
        }

        protected override void Update(GameTime gameTime)
        {
            Time timeElapsed = Time.FromMilliseconds((int)gameTime.ElapsedGameTime.TotalMilliseconds);

            this.UpdateInput();

            this.landWorld.UpdateWorld(timeElapsed);

            AObject2D.UpdateAnimationManager(timeElapsed);

            this.landWorld2D.UpdateWorld2D(gameTime);

            base.Update(gameTime);
        }

        private void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //this.landWorld2D.Dispose(this.landWorld);
                this.landWorld.Dispose();
                this.landWorld2D.Dispose(this.landWorld);

                Exit();
            }

            if (!oldState.IsKeyDown(Keys.Right) && newState.IsKeyDown(Keys.Right))
            {
                this.landWorld2D.CurrentAltitude++;
            }
            else if (!oldState.IsKeyDown(Keys.Left) && newState.IsKeyDown(Keys.Left))
            {
                this.landWorld2D.CurrentAltitude--;
            }

            if (!oldState.IsKeyDown(Keys.Up) && newState.IsKeyDown(Keys.Up))
            {
                this.landWorld2D.CurrentZoom += 1;
            }
            else if (!oldState.IsKeyDown(Keys.Down) && newState.IsKeyDown(Keys.Down))
            {
                this.landWorld2D.CurrentZoom -= 1;
            }

            // Update saved state.
            oldState = newState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.RoyalBlue);

            this.landWorld2D.DrawIn(this, spriteBatch);

            base.Draw(gameTime);
        }
    }
}