using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject
{
    public class TemperateRainForestTownGroundLandObject : ATownGroundLandObject
    {
        public TemperateRainForestTownGroundLandObject(int landObjectId, LandType landType)
            : base(landObjectId, landType)
        {
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                TemperateRainForestTownGroundLandObject townGroundLandObject = new TemperateRainForestTownGroundLandObject(this.LandObjectId, this.Type);
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
                GroundLandObject townGroundLandObject = new TemperateRainForestTownGroundLandObject(this.LandObjectId, landType);
                townGroundLandObject.LandTransition = landTransitionOverWall;

                return townGroundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            TemperateRainForestTownGroundLandObject townGroundLandObject = new TemperateRainForestTownGroundLandObject(this.LandObjectId, this.Type);
            townGroundLandObject.LandTransition = this.LandTransition;

            return townGroundLandObject;
        }
    }
}
