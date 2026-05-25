using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokeU.Animation;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;
using Color = Microsoft.Xna.Framework.Color;

namespace PokeU.View.WaterObject
{
    public class WaterObject2D : ALandObject2D
    {
        private static Rectangle defaultWaterFrame;
        private static MasterObject2D masterWaterObject2D;

        private static IAnimation animationWater;

        private Rectangle offsetTextureRect;

        public override Rectangle TextureRect
        {
            get
            {
                return new Rectangle(
                    masterWaterObject2D.TextureRect.Left + this.offsetTextureRect.Left,
                    masterWaterObject2D.TextureRect.Top + this.offsetTextureRect.Top,
                    this.offsetTextureRect.Width,
                    this.offsetTextureRect.Height);
            }
        }

        static WaterObject2D()
        {
            Rectangle[] waterMatrix =
            [
                new Rectangle(0, 0, 64, 64),
                new Rectangle(64, 0, 64, 64),
                new Rectangle(128, 0, 64, 64),
                new Rectangle(192, 0, 64, 64)
            ];

            defaultWaterFrame = waterMatrix[0];

            animationWater = new FrameAnimation(waterMatrix, Time.FromMilliseconds(1000), AnimationType.LOOP, InterpolationMethod.LINEAR);
            masterWaterObject2D = new MasterObject2D(animationWater);
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
            //this.Scale = new Vector2(0.5f, 0.5f);

            this.Position = position.ToVector2();
        }

        public override void RenderIn(Vector2 renderPosition, SpriteBatch spriteBatch, ref FloatRect boundsView)
        {
            spriteBatch.Draw(
                texture: this.Texture.Item1,
                position: renderPosition,
                sourceRectangle: defaultWaterFrame,
                color: Color.White,
                rotation: this.Rotation,
                origin: this.Origin,
                scale: this.Scale,
                effects: this.Effects,
                0);
        }
    }
}
