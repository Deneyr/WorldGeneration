using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.BiomeGroundObject
{
    public class TundraGroundLandObject : GroundLandObject, ILandWall
    {
        public TundraGroundLandObject(LandType landType)
            : base(landType)
        {
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                TundraGroundLandObject grassLandObject = new TundraGroundLandObject(this.Type);
                grassLandObject.LandTransition = landTransitionOverWall;

                return grassLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            TundraGroundLandObject grassLandObject = new TundraGroundLandObject(this.Type);
            grassLandObject.LandTransition = this.LandTransition;

            return grassLandObject;
        }
    }
}