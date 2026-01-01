using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.GroundObject
{
    public abstract class AGroundObject2DFactory : AObject2DFactory
    {
        public abstract (Texture2D, Rectangle) GetTextureByLandType(LandType landType);

        public abstract (Texture2D, Rectangle) GetWallTexture();

        public bool IsWall
        {
            get;
            set;
        }

        public AGroundObject2DFactory()
        {
            this.IsWall = false;
        }
    }
}
