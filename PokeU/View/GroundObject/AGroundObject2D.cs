using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.System;
using System;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.GroundObject
{
    public abstract class AGroundObject2D : ALandObject2D
    {
        private bool isWall;

        public AGroundObject2D(AGroundObject2DFactory factory, GroundLandObject landObject, Point position, bool isWall)
        {
            this.isWall = isWall;

            if (this.isWall)
            {
                this.Texture = factory.GetWallTexture();
            }
            else
            {
                this.Texture = factory.GetTextureByLandType(landObject.Type);
            }

            if (landObject.LandTransition == LandTransition.NONE)
            {
                this.TextureRect = this.GetFillTextureCoord(landObject.LandObjectId);
            }
            else
            {
                this.TextureRect = this.GetTransitionTextureCoord(landObject.LandTransition);
            }

            //this.Scale = new Vector2(0.5f, 0.5f);
            this.Position = position.ToVector2();
        }
    }
}
