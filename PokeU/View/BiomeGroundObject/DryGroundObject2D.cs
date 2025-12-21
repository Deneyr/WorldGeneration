using Microsoft.Xna.Framework;
using PokeU.View.GroundObject;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.BiomeGroundObject
{
    public class DryGroundObject2D : AGroundObject2D
    {
        public DryGroundObject2D(AGroundObject2DFactory factory, GroundLandObject landObject, Point position, bool isWall)
            : base(factory, landObject, position, isWall)
        {
        }
    }
}