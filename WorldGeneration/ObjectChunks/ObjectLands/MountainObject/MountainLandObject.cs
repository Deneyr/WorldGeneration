using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectLands.MountainObject
{
    public class MountainLandObject : GroundLandObject
    {
        public MountainType LandMountainType
        {
            get;
            private set;
        }

        public MountainLandObject(MountainType grassType) :
            base(LandType.STONE)
        {
            this.LandMountainType = grassType;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                MountainLandObject grassLandObject = new MountainLandObject(this.LandMountainType);
                grassLandObject.SetTransition(landTransitionOverWall);

                return grassLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            MountainLandObject grassLandObject = new MountainLandObject(this.LandMountainType);
            grassLandObject.SetTransition(this.LandTransition);

            return grassLandObject;
        }
    }

    public enum MountainType
    {
        NONE = -1,
        ROUGH = 0,
        PROJECTING = 1,
    }
}
