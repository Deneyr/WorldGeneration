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

            if (this.Texture != null)
            {
                int nbFrames = (int)this.Texture.Width / MainGame.MODEL_TO_VIEW;
                this.TextureRect = new Rectangle(landObject.LandObjectId % nbFrames * 16, 0, 16, 16);

                this.Position = position.ToVector2();
            }
        }
    }
}