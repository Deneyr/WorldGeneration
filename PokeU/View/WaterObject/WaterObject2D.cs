using PokeU.View.Animations;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;

namespace PokeU.View.WaterObject
{
    public class WaterObject2D : ALandObject2D
    {
        private static WaterObject2D singletonWaterObject2D;

        private static IAnimation animationWater;

        private IntRect textureRect;

        static WaterObject2D()
        {
            singletonWaterObject2D = new WaterObject2D();

            IntRect[] waterMatrix = new IntRect[]
            {
                new IntRect(0, 0, 128, 128),
                new IntRect(128, 0, 128, 128),
                new IntRect(256, 0, 128, 128),
                new IntRect(384, 0, 128, 128)
            };

            animationWater = new Animation(waterMatrix, Time.FromMilliseconds(250), AnimationType.LOOP);

            animationManager.PlayAnimation(singletonWaterObject2D, animationWater);
        }

        public WaterObject2D()
        {
        }

        public WaterObject2D(IObject2DFactory factory, WaterLandObject landObject, Vector2i position)
        {
            Texture texture = factory.GetTextureByIndex(0);

            this.textureRect = this.GetTransitionTextureCoord(landObject.LandTransition);
            this.ObjectSprite = new Sprite(texture, this.textureRect);

            this.ObjectSprite.Position = this.ObjectSprite.Position;
            this.ObjectSprite.Color = new Color(255, 255, 255, 127);
            this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

            this.Position = new Vector2f(position.X, position.Y);
        }

        public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            animationWater.Visit(this);

            base.DrawIn(window, ref boundsView);
        }

        public override void SetCanevas(IntRect newCanevas)
        {
            this.sprite.TextureRect = new IntRect(
                newCanevas.Left + this.textureRect.Left, 
                newCanevas.Top + this.textureRect.Top, 
                this.textureRect.Width, 
                this.textureRect.Height);
        }
    }
}
