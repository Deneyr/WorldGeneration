using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokeU.View;
using PokeU.View.ElementLandObject;
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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static readonly int MODEL_TO_VIEW = 16;

        private WorldMonitor landWorld;

        private LandWorld2D landWorld2D;

        KeyboardState oldState;

        public MainGame()
        {
            // TODO static constructor need that
            WaterObject2D waterObject2D = new WaterObject2D();

            this.landWorld = new WorldMonitor(32, 16, 123456789);
            this.landWorld.InitWorldMonitor();

            this.landWorld2D = new LandWorld2D(this.landWorld);
            LandWorld2D.TextureManager.MainGame = this;

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;

            //graphics.SynchronizeWithVerticalRetrace = false;

            graphics.ApplyChanges();

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //ballTexture = Content.Load<Texture2D>(@"Autotiles\treeSwamp");
            //ballTexture2 = Content.Load<Texture2D>(@"Autotiles\tree");
        }

        protected override void Update(GameTime gameTime)
        {
            //System.IO.File.AppendAllText("test.txt", "> Start Logic\n");

            this.UpdateInput();

            // TODO: Add your update logic here
            Time timeElapsed = Time.FromMilliseconds((int)gameTime.ElapsedGameTime.TotalMilliseconds);

            AObject2D.UpdateZoomAnimationManager(timeElapsed);

            this.landWorld.UpdateWorld(timeElapsed);

            //this.landWorld2D.UpdateWorld2D(gameTime);

            base.Update(gameTime);

            //System.IO.File.AppendAllText("test.txt", "< End Logic\n");
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
            //System.IO.File.AppendAllText("test.txt", "> Start Draw\n");

            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.RoyalBlue);

            this.landWorld2D.DrawIn(spriteBatch, gameTime);

            // TODO: Add your drawing code here
            //spriteBatch.Begin(transformMatrix: this.camera.GetTransform());
            ////for (int z = 0; z < 30; z++)
            ////{
            ////    for (int i = 0; i < 80; i++)
            ////    {
            ////        for (int j = 0; j < 130; j++)
            ////        {
            ////            _spriteBatch.Draw(ballTexture, new Vector2(j * 16, i * 16), Microsoft.Xna.Framework.Color.White);
            ////        }
            ////    }
            ////}

            //spriteBatch.Draw(ballTexture, new Vector2(0, 0), Microsoft.Xna.Framework.Color.White);

            ////Rectangle source = new Rectangle(0, 0, 48, 48);
            ////Rectangle destination = new Rectangle(2 * 16, 2 * 16, (int)(0.5f * 48), (int)(0.5f * 48));

            ////_spriteBatch.Draw(ballTexture, destination, source, Microsoft.Xna.Framework.Color.White);

            //spriteBatch.End();

            base.Draw(gameTime);

            //System.IO.File.AppendAllText("test.txt", "< End Draw\n");
        }
    }
}