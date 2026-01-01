using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.System;
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

        public TreeObject2D(TreeObject2DFactory factory, ATreeElementLandObject treeElementLandObject, Point position)
        {
            TreeObjectStructure treeObjectStructure = factory.CurrentObjectChunk.GetObjectStructure(treeElementLandObject.ParentStructureUID) as TreeObjectStructure;

            this.Texture = factory.GetTextureFromBiomeLandType(treeObjectStructure.LandType, treeElementLandObject.LandObjectId);

            if (this.Texture.Item1 != null)
            {
                int nbCaseWidth = (int)(this.Texture.Item2.Width / MainGame.MODEL_TO_VIEW);
                int nbCaseHeight = (int)(this.Texture.Item2.Height / MainGame.MODEL_TO_VIEW);

                this.TextureRect = this.GetSpriteRectFrom(treeElementLandObject.Part, nbCaseWidth, nbCaseHeight);

                this.Position = position.ToVector2();
            }
        }


        public Rectangle GetSpriteRectFrom(TreePart treePart, int nbCaseWidth, int nbCaseHeight)
        {
            Rectangle result = Rectangle.Empty;
            switch (treePart)
            {
                case TreePart.TOP_LEFT:
                    result = new Rectangle(0, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;
                case TreePart.TOP_MID:
                    result = new Rectangle(MainGame.MODEL_TO_VIEW, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;
                case TreePart.TOP_RIGHT:
                    result = new Rectangle((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;

                case TreePart.MID_LEFT:
                    result = new Rectangle(0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;
                case TreePart.MID_MID:
                    result = new Rectangle(MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;
                case TreePart.MID_RIGHT:
                    result = new Rectangle((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;

                case TreePart.BOT_LEFT:
                    result = new Rectangle(0, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;
                case TreePart.BOT_MID:
                    result = new Rectangle(MainGame.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;
                case TreePart.BOT_RIGHT:
                    result = new Rectangle((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                    break;
            }

            result.X += this.Texture.Item2.X;
            result.Y += this.Texture.Item2.Y;

            return result;
        }

        //public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        //{
        //    base.DrawIn(window, ref boundsView);
        //}
    }
}
