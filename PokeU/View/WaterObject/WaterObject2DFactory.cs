using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;

namespace PokeU.View.WaterObject
{
    public class WaterObject2DFactory : AObject2DFactory
    {
        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\waterSea.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            WaterLandObject waterLandObject = obj as WaterLandObject;

            if (waterLandObject != null)
            {
                return new WaterObject2D(this, waterLandObject, position);
            }
            return null;
        }
    }
}
