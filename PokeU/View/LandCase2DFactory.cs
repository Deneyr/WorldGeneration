using Microsoft.Xna.Framework;
using SFML.System;
using WorldGeneration.ObjectChunks;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace PokeU.View
{
    public class LandCase2DFactory : AObject2DFactory
    {
        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Point position)
        {
            LandCase landCase = obj as LandCase;

            return new LandCase2D(landWorld2D, this.CurrentObjectChunk, landCase, position);
        }
    }
}
