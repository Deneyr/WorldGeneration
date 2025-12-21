using Microsoft.Xna.Framework;
using SFML.System;
using WorldGeneration.ObjectChunks;

namespace PokeU.View
{
    public class LandChunk2DFactory : AObject2DFactory
    {
        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Point position)
        {
            IObjectChunk landChunk = obj as IObjectChunk;

            return new LandChunk2D(landWorld2D, landChunk);
        }
    }
}
