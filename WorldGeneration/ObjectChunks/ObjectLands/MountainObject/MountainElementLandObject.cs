using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.MountainObject
{
    public class MountainElementLandObject : ALandObject, ILandOverGround
    {
        public MountainType LandMountainType
        {
            get;
            private set;
        }

        public int ElementIndex
        {
            get;
            private set;
        }

        public MountainElementLandObject(MountainType mountainType, int elementIndex) :
            base()
        {
            this.LandMountainType = mountainType;
            this.ElementIndex = elementIndex;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            if (wallLandTransition != LandTransition.NONE)
            {
                MountainElementLandObject mountainLandObject = new MountainElementLandObject(this.LandMountainType, this.ElementIndex);

                return mountainLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            MountainElementLandObject mountainLandObject = new MountainElementLandObject(this.LandMountainType, this.ElementIndex);

            return mountainLandObject;
        }
    }
}
