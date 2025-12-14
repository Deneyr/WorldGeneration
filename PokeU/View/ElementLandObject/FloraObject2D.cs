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

        public FloraObject2D(FloraObject2DFactory factory, AFloraElementLandObject landObject, Vector2i position)
        {
            this.Texture = factory.GetTextureFromBiomeLandType(landObject.LandType);
            if (this.Texture != null)
            {
                int nbFrames = (int)this.Texture.Width / MainGame.MODEL_TO_VIEW;

                this.ObjectSprite = new Sprite(DEFAULT_TEXTURE, new IntRect(landObject.LandObjectId % nbFrames * 16, 0, 16, 16));
                //this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

                this.ObjectSprite.Position = this.ObjectSprite.Position;
                //this.ObjectSprite.Color = new Color(255, 255, 255, 127);

                this.Position = new Vector2f(position.X, position.Y);
            }
        }
    }
}