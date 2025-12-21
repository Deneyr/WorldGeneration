using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.System;
using System;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;
using WorldGeneration.ObjectChunks.ObjectStructures.TallGrassStructures;

namespace PokeU.View.ElementLandObject
{
    public class TallGrassObject2D : AElementLandObject2D
    {
        public TallGrassObject2D()
        {
        }

        public TallGrassObject2D(TallGrassObject2DFactory factory, ATallGrassElementLandObject landObject, Point position)
        {
            TallGrassObjectStructure tallGrassObjectStructure = factory.CurrentObjectChunk.GetObjectStructure(landObject.ParentStructureUID) as TallGrassObjectStructure;

            this.Texture = factory.GetTextureFromBiomeLandType(landObject.LandType, tallGrassObjectStructure.IsFullPatch);

            if(tallGrassObjectStructure.IsFullPatch)
            {
                if (landObject.LandTransition == LandTransition.NONE)
                {
                    this.TextureRect = this.GetFillTextureCoord(landObject.LandObjectId);
                }
                else
                {
                    this.TextureRect = this.GetTransitionTextureCoord(landObject.LandTransition);
                }
            }
            else
            {
                this.TextureRect = new Rectangle(0, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
            }

            this.Position = position.ToVector2();
        }

        protected override Rectangle GetTransitionTextureCoord(LandTransition landTransition)
        {
            Rectangle result = new Rectangle(0, 0, 1, 1);

            switch (landTransition)
            {
                case LandTransition.TOP:
                    result.X = 1;
                    result.Y = 1;
                    break;
                case LandTransition.RIGHT:
                    result.X = 2;
                    result.Y = 2;
                    break;
                case LandTransition.BOT:
                    result.X = 1;
                    result.Y = 3;
                    break;
                case LandTransition.LEFT:
                    result.X = 0;
                    result.Y = 2;
                    break;
                case LandTransition.TOP_LEFT:
                    result.X = 0;
                    result.Y = 1;
                    break;
                case LandTransition.TOP_RIGHT:
                    result.X = 2;
                    result.Y = 1;
                    break;
                case LandTransition.BOT_LEFT:
                    result.X = 0;
                    result.Y = 3;
                    break;
                case LandTransition.BOT_RIGHT:
                    result.X = 2;
                    result.Y = 3;
                    break;
                case LandTransition.TOP_INT_LEFT:
                    result.X = 3;
                    result.Y = 0;
                    break;
                case LandTransition.TOP_INT_RIGHT:
                    result.X = 3;
                    result.Y = 1;
                    break;
                case LandTransition.BOT_INT_LEFT:
                    result.X = 3;
                    result.Y = 2;
                    break;
                case LandTransition.BOT_INT_RIGHT:
                    result.X = 3;
                    result.Y = 3;
                    break;
            }

            result.X *= MainGame.MODEL_TO_VIEW;
            result.Y *= MainGame.MODEL_TO_VIEW;
            result.Width *= MainGame.MODEL_TO_VIEW;
            result.Height *= MainGame.MODEL_TO_VIEW;

            return result;
        }

        protected override Rectangle GetFillTextureCoord(int landObjectId)
        {
            switch (Math.Abs(landObjectId % 4))
            {
                case 0:
                    return new Rectangle(MainGame.MODEL_TO_VIEW, 2 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case 1:
                    return new Rectangle(0 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case 2:
                    return new Rectangle(MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case 3:
                    return new Rectangle(2 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
            }
            return new Rectangle(0 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
        }
    }
}
