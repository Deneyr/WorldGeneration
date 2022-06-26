using PokeU.View.Animations;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;

namespace PokeU.View.ElementLandObject
{
    public class TallGrassObject2D : AElementLandObject2D
    {
        public TallGrassObject2D()
        {
        }

        public TallGrassObject2D(IObject2DFactory factory, TallGrassElementLandObject landObject, Vector2i position)
        {
            Texture texture = factory.GetTextureByIndex(0);

            this.ObjectSprite = new Sprite(texture);

            this.ObjectSprite.Position = this.ObjectSprite.Position;
            this.ObjectSprite.Color = new Color(255, 255, 255, 127);

            this.Position = new Vector2f(position.X, position.Y);
        }
    }
}
