using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.GroundObject
{
    public class GroundElementLandObject : ALandObject, ILandOverGround
    {
        public LandType LandType
        {
            get;
            private set;
        }

        public int ElementIndex
        {
            get;
            private set;
        }

        public GroundElementLandObject(int landObjectId, LandType landType, int elementIndex) :
            base(landObjectId)
        {
            this.LandType = landType;
            this.ElementIndex = elementIndex;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            if (wallLandTransition != LandTransition.NONE)
            {
                GroundElementLandObject groundLandObject = new GroundElementLandObject(this.LandObjectId, this.LandType, this.ElementIndex);

                return groundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GroundElementLandObject groundLandObject = new GroundElementLandObject(this.LandObjectId, this.LandType, this.ElementIndex);

            return groundLandObject;
        }
    }
}
