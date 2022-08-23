using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora
{
    public class RainForestFloraElementLandObject : AFloraElementLandObject
    {
        public RainForestFloraElementLandObject(int landElementObjectId)
            : base(landElementObjectId)
        {
        }

        public override ILandObject Clone()
        {
            return new RainForestFloraElementLandObject(this.LandObjectId);
        }
    }
}