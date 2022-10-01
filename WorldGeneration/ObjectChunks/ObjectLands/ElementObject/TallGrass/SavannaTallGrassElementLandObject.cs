using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass
{
    public class SavannaTallGrassElementLandObject : ATallGrassElementLandObject
    {
        public override LandTransition LandTransition
        {
            get;
            set;
        }

        public SavannaTallGrassElementLandObject(int landElementObjectId)
            : base(landElementObjectId)
        {
            this.LandTransition = LandTransition.NONE;
        }

        public override ILandObject Clone()
        {
            return new SavannaTallGrassElementLandObject(this.LandObjectId);
        }
    }
}