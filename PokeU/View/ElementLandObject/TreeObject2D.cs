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

        public TreeObject2D(TreeObject2DFactory factory, ATreeElementLandObject treeElementLandObject, Vector2i position)
        {
            TreeObjectStructure treeObjectStructure = factory.CurrentObjectChunk.GetObjectStructure(treeElementLandObject.ParentStructureUID) as TreeObjectStructure;

            this.Texture = factory.GetTextureFromBiomeLandType(treeObjectStructure.LandType, treeElementLandObject.LandObjectId);

            if (this.Texture != null)
            {
                int nbCaseWidth = (int)(this.Texture.Width / MainGame.MODEL_TO_VIEW);
                int nbCaseHeight = (int)(this.Texture.Height / MainGame.MODEL_TO_VIEW);

                this.ObjectSprite = new Sprite(DEFAULT_TEXTURE, this.GetSpriteRectFrom(treeElementLandObject.Part, nbCaseWidth, nbCaseHeight));

                this.ObjectSprite.Position = this.ObjectSprite.Position;
                //this.ObjectSprite.Color = new Color(255, 255, 255, 127);

                this.Position = new Vector2f(position.X, position.Y);
            }
        }


        public IntRect GetSpriteRectFrom(TreePart treePart, int nbCaseWidth, int nbCaseHeight)
        {
            switch (treePart)
            {
                case TreePart.TOP_LEFT:
                    return new IntRect(0, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.TOP_MID:
                    return new IntRect(MainGame.MODEL_TO_VIEW, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.TOP_RIGHT:
                    return new IntRect((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);

                case TreePart.MID_LEFT:
                    return new IntRect(0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.MID_MID:
                    return new IntRect(MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.MID_RIGHT:
                    return new IntRect((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);

                case TreePart.BOT_LEFT:
                    return new IntRect(0, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.BOT_MID:
                    return new IntRect(MainGame.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
                case TreePart.BOT_RIGHT:
                    return new IntRect((nbCaseWidth - 1) * MainGame.MODEL_TO_VIEW, (nbCaseHeight - 1) * MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
            }   

            return new IntRect();
        }

        //public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        //{
        //    base.DrawIn(window, ref boundsView);
        //}
    }
}
