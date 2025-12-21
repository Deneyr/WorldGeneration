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

            if (this.Texture != null)
            {
                int nbCaseWidth = (int)(this.Texture.Width / MainGame.MODEL_TO_VIEW);
                int nbCaseHeight = (int)(this.Texture.Height / MainGame.MODEL_TO_VIEW);

                this.TextureRect = this.GetSpriteRectFrom(treeElementLandObject.Part, nbCaseWidth, nbCaseHeight);

                this.Position = position.ToVector2();
            }
        }


        public Rectangle GetSpriteRectFrom(TreePart treePart, int nbCaseWidth, int nbCaseHeight)
        {
            switch (treePart)
            {
                case TreePart.TOP_LEFT:
                    return new Rectangle(0, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.TOP_MID:
                    return new Rectangle(MainGame.MODEL_TO_VIEW, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.TOP_RIGHT:
                    return new Rectangle((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);

                case TreePart.MID_LEFT:
                    return new Rectangle(0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.MID_MID:
                    return new Rectangle(MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.MID_RIGHT:
                    return new Rectangle((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);

                case TreePart.BOT_LEFT:
                    return new Rectangle(0, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.BOT_MID:
                    return new Rectangle(MainGame.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.BOT_RIGHT:
                    return new Rectangle((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
            }   

            return new Rectangle();
        }

        //public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        //{
        //    base.DrawIn(window, ref boundsView);
        //}
    }
}
