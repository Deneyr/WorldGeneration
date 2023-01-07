using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject
{
    public class SavannaTownGroundLandObject : ATownGroundLandObject
    {
        public SavannaTownGroundLandObject(int landObjectId, LandType landType)
            : base(landObjectId, landType)
        {
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                SavannaTownGroundLandObject townGroundLandObject = new SavannaTownGroundLandObject(this.LandObjectId, this.Type);
                townGroundLandObject.LandTransition = landTransitionOverWall;

                return townGroundLandObject;
            }
            return null;
        }

        public override ILandObject Clone(LandType landType, LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GroundLandObject townGroundLandObject = new SavannaTownGroundLandObject(this.LandObjectId, landType);
                townGroundLandObject.LandTransition = landTransitionOverWall;

                return townGroundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            SavannaTownGroundLandObject townGroundLandObject = new SavannaTownGroundLandObject(this.LandObjectId, this.Type);
            townGroundLandObject.LandTransition = this.LandTransition;

            return townGroundLandObject;
        }
    }
}