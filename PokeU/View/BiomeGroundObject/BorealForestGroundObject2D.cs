using PokeU.View.GroundObject;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.BiomeGroundObject
{
    public class BorealForestGroundObject2D : AGroundObject2D
    {
        public BorealForestGroundObject2D(AGroundObject2DFactory factory, GroundLandObject landObject, Vector2i position, bool isWall)
            : base(factory, landObject, position, isWall)
        {
        }
    }
}
