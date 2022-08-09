using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass
{
    public class TemperateRainForestTallGrassElementLandObject : ATallGrassElementLandObject
    {
        public TemperateRainForestTallGrassElementLandObject(int landElementObjectId)
            : base(landElementObjectId)
        {
        }

        public override ILandObject Clone()
        {
            return new TemperateRainForestTallGrassElementLandObject(this.LandObjectId);
        }
    }
}
