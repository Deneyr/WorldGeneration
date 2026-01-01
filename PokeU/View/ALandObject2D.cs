using Microsoft.Xna.Framework;
using SFML.Graphics;
using System;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace PokeU.View
{
    public abstract class ALandObject2D: AObject2D, ILandObject2D
    {
        protected virtual Rectangle GetTransitionTextureCoord(LandTransition landTransition)
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

            result.X += this.Texture.Item2.X;
            result.Y += this.Texture.Item2.Y;

            return result;
        }

        protected virtual Rectangle GetFillTextureCoord(int landObjectId)
        {
            Rectangle result = new Rectangle(0 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW);

            switch (Math.Abs(landObjectId % 4))
            {
                case 0:
                    result = new Rectangle(1 * MainGame.MODEL_TO_VIEW, 2 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW);
                    break;
                case 1:
                    result = new Rectangle(0 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW);
                    break;
                case 2:
                    result = new Rectangle(1 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW);
                    break;
                case 3:
                    result = new Rectangle(2 * MainGame.MODEL_TO_VIEW, 0 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW, 1 * MainGame.MODEL_TO_VIEW);
                    break;
            }

            result.X += this.Texture.Item2.X;
            result.Y += this.Texture.Item2.Y;

            return result;
        }
    }
}
