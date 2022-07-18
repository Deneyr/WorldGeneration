using PokeU.View.GroundObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace PokeU.View.ElementLandObject
{
    public class TreeObject2DFactory : AGroundObject2DFactory
    {
        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\tree.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            ATreeElementLandObject treeElementLandObject = obj as ATreeElementLandObject;

            if (treeElementLandObject != null)
            {
                return new TreeObject2D(this, treeElementLandObject, position);
            }
            return null;
        }

        public override Texture GetTextureByLandType(LandType landType)
        {
            return this.GetTextureByIndex(0);
        }

        public override Texture GetWallTexture()
        {
            return this.GetTextureByIndex(0);
        }
    }
}
