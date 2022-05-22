using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.BiomeGroundObject
{
    public class SavannaGroundLandObject : GroundLandObject, ILandWall
    {
        public SavannaGroundLandObject(int landObjectId, LandType landType)
            : base(landObjectId, landType)
        {
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                SavannaGroundLandObject grassLandObject = new SavannaGroundLandObject(this.LandObjectId, this.Type);
                grassLandObject.LandTransition = landTransitionOverWall;

                return grassLandObject;
            }
            return null;
        }

        public override ILandObject Clone(LandType landType, LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GroundLandObject groundLandObject = new SavannaGroundLandObject(this.LandObjectId, landType);
                groundLandObject.LandTransition = landTransitionOverWall;

                return groundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            SavannaGroundLandObject grassLandObject = new SavannaGroundLandObject(this.LandObjectId, this.Type);
            grassLandObject.LandTransition = this.LandTransition;

            return grassLandObject;
        }
    }
}