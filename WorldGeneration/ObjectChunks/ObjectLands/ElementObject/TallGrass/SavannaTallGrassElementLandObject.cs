using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass
{
    public class SavannaTallGrassElementLandObject : ATallGrassElementLandObject
    {
        public SavannaTallGrassElementLandObject(int landElementObjectId)
            : base(landElementObjectId)
        {
        }

        public override ILandObject Clone()
        {
            return new SavannaTallGrassElementLandObject(this.LandObjectId);
        }
    }
}