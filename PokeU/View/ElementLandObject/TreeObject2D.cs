using PokeU.View.GroundObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree;
using WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace PokeU.View.ElementLandObject
{
    public class TreeObject2D : AElementLandObject2D
    {
        public TreeObject2D()
        {
        }

        public TreeObject2D(TreeObject2DFactory factory, ATreeElementLandObject treeElementLandObject, Vector2i position)
        {
            TreeObjectStructure treeObjectStructure = factory.CurrentObjectChunk.GetObjectStructure(treeElementLandObject.ParentStructureUID) as TreeObjectStructure;

            Texture texture = factory.GetTextureFromBiomeLandType(treeObjectStructure.LandType, treeElementLandObject.LandObjectId);

            int nbCaseWidth = (int) (texture.Size.X / MainWindow.MODEL_TO_VIEW);
            int nbCaseHeight = (int) (texture.Size.Y / MainWindow.MODEL_TO_VIEW);

            this.ObjectSprite = new Sprite(texture, this.GetSpriteRectFrom(treeElementLandObject.Part, nbCaseWidth, nbCaseHeight));

            this.ObjectSprite.Position = this.ObjectSprite.Position;
            //this.ObjectSprite.Color = new Color(255, 255, 255, 127);

            this.Position = new Vector2f(position.X, position.Y);
        }


        public IntRect GetSpriteRectFrom(TreePart treePart, int nbCaseWidth, int nbCaseHeight)
        {
            switch (treePart)
            {
                case TreePart.TOP_LEFT:
                    return new IntRect(0, 0, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case TreePart.TOP_MID:
                    return new IntRect(MainWindow.MODEL_TO_VIEW, 0, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case TreePart.TOP_RIGHT:
                    return new IntRect((nbCaseWidth - 1) * MainWindow.MODEL_TO_VIEW, 0, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);

                case TreePart.MID_LEFT:
                    return new IntRect(0, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case TreePart.MID_MID:
                    return new IntRect(MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case TreePart.MID_RIGHT:
                    return new IntRect((nbCaseWidth - 1) * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);

                case TreePart.BOT_LEFT:
                    return new IntRect(0, (nbCaseHeight - 1) * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case TreePart.BOT_MID:
                    return new IntRect(MainWindow.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
                case TreePart.BOT_RIGHT:
                    return new IntRect((nbCaseWidth - 1) * MainWindow.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);
            }

            return new IntRect();
        }

        //public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        //{
        //    base.DrawIn(window, ref boundsView);
        //}
    }
}
