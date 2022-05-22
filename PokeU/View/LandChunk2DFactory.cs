using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks;

namespace PokeU.View
{
    public class LandChunk2DFactory : AObject2DFactory
    {
        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            IObjectChunk landChunk = obj as IObjectChunk;

            return new LandChunk2D(landWorld2D, landChunk);
        }
    }
}
