using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass
{
    public class TropicalWoodlandTallGrassElementLandObject : ATallGrassElementLandObject
    {
        public TropicalWoodlandTallGrassElementLandObject(int landElementObjectId)
            : base(landElementObjectId)
        {
        }

        public override ILandObject Clone()
        {
            return new TropicalWoodlandTallGrassElementLandObject(this.LandObjectId);
        }
    }
}