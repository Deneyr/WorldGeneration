using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;

namespace PokeU.View.ElementLandObject
{
    public class TallGrassObject2DFactory : AObject2DFactory
    {
        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\tallGrass.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            ATallGrassElementLandObject tallGrassElementLandObject = obj as ATallGrassElementLandObject;

            if (tallGrassElementLandObject != null)
            {
                return new TallGrassObject2D(this, tallGrassElementLandObject, position);
            }
            return null;
        }
    }
}
