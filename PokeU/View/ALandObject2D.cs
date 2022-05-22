using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace PokeU.View
{
    public abstract class ALandObject2D: AObject2D, ILandObject2D
    {
        protected IntRect GetTransitionTextureCoord(LandTransition landTransition)
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

            result.Left *= 2 * MainWindow.MODEL_TO_VIEW;
            result.Top *= 2 * MainWindow.MODEL_TO_VIEW;
            result.Width *= 2 * MainWindow.MODEL_TO_VIEW;
            result.Height *= 2 * MainWindow.MODEL_TO_VIEW;

            return result;
        }

    }
}
