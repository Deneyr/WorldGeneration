using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace PokeU.View
{
    public class LandCase2DFactory : AObject2DFactory
    {
        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            LandCase landCase = obj as LandCase;

            return new LandCase2D(landWorld2D, this.CurrentObjectChunk, landCase, position);
        }
    }
}
