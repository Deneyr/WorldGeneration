using PokeU.View.Animations;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;

namespace PokeU.View.ElementLandObject
{
    public class TallGrassObject2D : AElementLandObject2D
    {
        public TallGrassObject2D()
        {
        }

        public TallGrassObject2D(TallGrassObject2DFactory factory, ATallGrassElementLandObject landObject, Vector2i position)
        {
            Texture texture = factory.GetTextureFromBiomeLandType(landObject.LandType);

            this.ObjectSprite = new Sprite(texture);

            if(texture.Size.X >= MainWindow.MODEL_TO_VIEW * 4
                && texture.Size.Y >= MainWindow.MODEL_TO_VIEW * 4)
            {
                if (landObject.LandTransition == LandTransition.NONE)
                {
                    this.ObjectSprite = new Sprite(texture, this.GetFillTextureCoord(landObject.LandObjectId));
                }
                else
                {
                    this.ObjectSprite.TextureRect = this.GetTransitionTextureCoord(landObject.LandTransition);
                }
            }
            else
            {
                this.ObjectSprite.TextureRect = new IntRect(0, 0, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
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

            result.Left *= MainWindow.MODEL_TO_VIEW;
            result.Top *= MainWindow.MODEL_TO_VIEW;
            result.Width *= MainWindow.MODEL_TO_VIEW;
            result.Height *= MainWindow.MODEL_TO_VIEW;

            return result;
        }

        protected override IntRect GetFillTextureCoord(int landObjectId)
        {
            switch (Math.Abs(landObjectId % 4))
            {
                case 0:
                    return new IntRect(MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case 1:
                    return new IntRect(0 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case 2:
                    return new IntRect(MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case 3:
                    return new IntRect(2 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
            }
            return new IntRect(0 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
        }
    }
}
