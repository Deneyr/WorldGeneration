using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora
{
    public class BorealForestFloraElementLandObject : AFloraElementLandObject
    {
        public BorealForestFloraElementLandObject(int landElementObjectId) 
            : base(landElementObjectId)
        {
        }

        public override ILandObject Clone()
        {
            return new BorealForestFloraElementLandObject(this.LandObjectId);
        }
    }
}
