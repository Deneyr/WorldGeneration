using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public class TallGrassElementLandObject : AElementLandObject, ILandOverGround
    {
        public TallGrassElementLandObject(int landElementObjectId) : base(landElementObjectId)
        {
        }

        public override ILandObject Clone()
        {
            return new TallGrassElementLandObject(this.LandObjectId);
        }
    }
}
