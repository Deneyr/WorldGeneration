using Microsoft.Xna.Framework;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;

namespace PokeU.View.WaterObject
{
    public class WaterObject2DFactory : AObject2DFactory
    {
        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\waterSea");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Point position)
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
