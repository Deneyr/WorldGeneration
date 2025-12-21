using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokeU.View.Animations;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;
using Color = Microsoft.Xna.Framework.Color;

namespace PokeU.View.WaterObject
{
    public class WaterObject2D : ALandObject2D
    {
        private static WaterObject2D singletonWaterObject2D;

        private static IAnimation animationWater;

        private Rectangle offsetTextureRect;

        static WaterObject2D()
        {
            singletonWaterObject2D = new WaterObject2D();

            Rectangle[] waterMatrix =
            [
                new Rectangle(0, 0, 128, 128),
                new Rectangle(128, 0, 128, 128),
                new Rectangle(256, 0, 128, 128),
                new Rectangle(384, 0, 128, 128)
            ];

            animationWater = new Animation(waterMatrix, Time.FromMilliseconds(250), AnimationType.LOOP);

            animationManager.PlayAnimation(singletonWaterObject2D, animationWater);
        }

        public WaterObject2D()
        {
        }

        public WaterObject2D(IObject2DFactory factory, WaterLandObject landObject, Point position)
        {
            this.Texture = factory.GetTextureByIndex(0);

            this.offsetTextureRect = this.GetTransitionTextureCoord(landObject.LandTransition);
            this.TextureRect = this.offsetTextureRect;
            this.EffectColor = new Color(255, 255, 255, 127);
            this.Scale = new Vector2(0.5f, 0.5f);

            this.Position = position.ToVector2();
        }

        public override void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView)
        {
            //TODO remove this from draw
            animationWater.Visit(this);

            base.DrawIn(spriteBatch, ref boundsView);
        }

        public override void SetCanevas(Rectangle newCanevas)
        {
            //this.TextureRect = new IntRect(
            //    newCanevas.Left + this.textureRect.Left, 
            //    newCanevas.Top + this.textureRect.Top, 
            //    this.textureRect.Width, 
            //    this.textureRect.Height);

            this.TextureRect = new Rectangle(
                newCanevas.Left + this.offsetTextureRect.Left,
                newCanevas.Top + this.offsetTextureRect.Top,
                this.offsetTextureRect.Width,
                this.offsetTextureRect.Height);
        }
    }
}
