using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora;

namespace PokeU.View.ElementLandObject
{
    public class FloraObject2D : AElementLandObject2D
    {
        public FloraObject2D()
        {
        }

        public FloraObject2D(FloraObject2DFactory factory, AFloraElementLandObject landObject, Point position)
        {
            this.Texture = factory.GetTextureFromBiomeLandType(landObject.LandType);

            if (this.Texture.Item1 != null)
            {
                int nbFrames = this.Texture.Item2.Width / MainGame.MODEL_TO_VIEW;
                this.TextureRect = new Rectangle(this.Texture.Item2.X + landObject.LandObjectId % nbFrames * MainGame.MODEL_TO_VIEW, this.Texture.Item2.Y, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);

                this.Position = position.ToVector2();
            }
        }
    }
}