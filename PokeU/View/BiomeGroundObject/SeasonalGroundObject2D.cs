using Microsoft.Xna.Framework;
using PokeU.View.GroundObject;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.BiomeGroundObject
{
    public class SeasonalGroundObject2D : AGroundObject2D
    {
        public SeasonalGroundObject2D(AGroundObject2DFactory factory, GroundLandObject landObject, Point position, bool isWall)
            : base(factory, landObject, position, isWall)
        {
        }
    }
}