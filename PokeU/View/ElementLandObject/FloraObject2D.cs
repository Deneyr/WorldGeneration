using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Texture texture = factory.GetTextureFromBiomeLandType(landObject.LandType);
            int nbFrames = (int) texture.Size.X / 16;

            this.ObjectSprite = new Sprite(texture, new IntRect(landObject.LandObjectId % nbFrames * 16, 0, 16, 16));
            //this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

            this.ObjectSprite.Position = this.ObjectSprite.Position;
            //this.ObjectSprite.Color = new Color(255, 255, 255, 127);

            this.Position = new Vector2f(position.X, position.Y);
        }
    }
}