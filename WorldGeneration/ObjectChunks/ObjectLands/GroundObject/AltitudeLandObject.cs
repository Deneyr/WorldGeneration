using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.GroundObject
{
    public class AltitudeLandObject: ALandObject, ILandWall
    {
        private LandType landType;

        public LandType Type
        {
            get
            {
                return this.landType;
            }
        }

        public AltitudeLandObject(int landObjectId, LandType landType) :
            base(landObjectId)
        {
            this.landType = landType;

        }

        public void SetLandTransition(LandTransition landTransition)
        {
            this.LandTransition = landTransition;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                AltitudeLandObject altitudeLandObject = new AltitudeLandObject(this.LandObjectId, landType);
                altitudeLandObject.SetLandTransition(landTransitionOverWall);

                return altitudeLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            AltitudeLandObject altitudeLandObject = new AltitudeLandObject(this.LandObjectId, this.landType);
            altitudeLandObject.SetLandTransition(this.LandTransition);

            return altitudeLandObject;
        }
    }
}
