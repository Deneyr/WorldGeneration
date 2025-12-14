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

        public TallGrassObject2D(TallGrassObject2DFactory factory, ATallGrassElementLandObject landObject, Vector2i position)
        {
            TallGrassObjectStructure tallGrassObjectStructure = factory.CurrentObjectChunk.GetObjectStructure(landObject.ParentStructureUID) as TallGrassObjectStructure;

            this.Texture = factory.GetTextureFromBiomeLandType(landObject.LandType, tallGrassObjectStructure.IsFullPatch);

            this.ObjectSprite = new Sprite(DEFAULT_TEXTURE);

            if(tallGrassObjectStructure.IsFullPatch)
            {
                if (landObject.LandTransition == LandTransition.NONE)
                {
                    this.ObjectSprite = new Sprite(DEFAULT_TEXTURE, this.GetFillTextureCoord(landObject.LandObjectId));
                }
                else
                {
                    this.ObjectSprite.TextureRect = this.GetTransitionTextureCoord(landObject.LandTransition);
                }
            }
            else
            {
                this.ObjectSprite.TextureRect = new IntRect(0, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
            }

            this.ObjectSprite.Position = this.ObjectSprite.Position;
            //this.ObjectSprite.Color = new Color(255, 255, 255, 127);

            this.Position = new Vector2f(position.X, position.Y);
        }

        protected override IntRect GetTransitionTextureCoord(LandTransition landTransition)
        {
            IntRect result = new IntRect(0, 0, 1, 1);

            switch (landTransition)
            {
                case LandTransition.TOP:
                    result.Left = 1;
                    result.Top = 1;
                    break;
                case LandTransition.RIGHT:
                    result.Left = 2;
                    result.Top = 2;
                    break;
                case LandTransition.BOT:
                    result.Left = 1;
                    result.Top = 3;
                    break;
                case LandTransition.LEFT:
                    result.Left = 0;
                    result.Top = 2;
                    break;
                case LandTransition.TOP_LEFT:
                    result.Left = 0;
                    result.Top = 1;
                    break;
                case LandTransition.TOP_RIGHT:
                    result.Left = 2;
                    result.Top = 1;
                    break;
                case LandTransition.BOT_LEFT:
                    result.Left = 0;
                    result.Top = 3;
                    break;
                case LandTransition.BOT_RIGHT:
                    result.Left = 2;
                    result.Top = 3;
                    break;
                case LandTransition.TOP_INT_LEFT:
                    result.Left = 3;
                    result.Top = 0;
                    break;
                case LandTransition.TOP_INT_RIGHT:
                    result.Left = 3;
                    result.Top = 1;
                    break;
                case LandTransition.BOT_INT_LEFT:
                    result.Left = 3;
                    result.Top = 2;
                    break;
                case LandTransition.BOT_INT_RIGHT:
                    result.Left = 3;
                    result.Top = 3;
                    break;
            }

            result.Left *= MainGame.MODEL_TO_VIEW;
            result.Top *= MainGame.MODEL_TO_VIEW;
            result.Width *= MainGame.MODEL_TO_VIEW;
            result.Height *= MainGame.MODEL_TO_VIEW;

            return result;
        }

        protected override IntRect GetFillTextureCoord(int landObjectId)
        {
            switch (Math.Abs(landObjectId % 4))
            {
                case 0:
                    return new IntRect(MainGame.MODEL_TO_VIEW, 2 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case 1:
                    return new IntRect(0 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case 2:
                    return new IntRect(MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case 3:
                    return new IntRect(2 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
            }
            return new IntRect(0 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
        }
    }
}
