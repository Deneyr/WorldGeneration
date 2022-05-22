using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.GroundObject
{
    public abstract class AGroundObject2DFactory : AObject2DFactory
    {
        public abstract Texture GetTextureByLandType(LandType landType);

        public abstract Texture GetWallTexture();

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
