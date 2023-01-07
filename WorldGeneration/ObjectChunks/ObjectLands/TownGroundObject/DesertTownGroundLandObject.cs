using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject
{
    public class DesertTownGroundLandObject : ATownGroundLandObject
    {
        public DesertTownGroundLandObject(int landObjectId, LandType landType)
            : base(landObjectId, landType)
        {
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                DesertTownGroundLandObject townGroundLandObject = new DesertTownGroundLandObject(this.LandObjectId, this.Type);
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
                GroundLandObject townGroundLandObject = new DesertTownGroundLandObject(this.LandObjectId, landType);
                townGroundLandObject.LandTransition = landTransitionOverWall;

                return townGroundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            DesertTownGroundLandObject townGroundLandObject = new DesertTownGroundLandObject(this.LandObjectId, this.Type);
            townGroundLandObject.LandTransition = this.LandTransition;

            return townGroundLandObject;
        }
    }
}
