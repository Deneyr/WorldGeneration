using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectLands.GrassObject
{
    public class GrassLandObject : GroundLandObject
    {
        public GrassType LandGrassType
        {
            get;
            private set;
        }

        public GrassLandObject(int landObjectId, GrassType grassType) :
            base(landObjectId, LandType.GRASS)
        {
            this.LandGrassType = grassType;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GrassLandObject grassLandObject = new GrassLandObject(this.LandObjectId, this.LandGrassType);
                grassLandObject.LandTransition = landTransitionOverWall;

                return grassLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GrassLandObject grassLandObject = new GrassLandObject(this.LandObjectId, this.LandGrassType);
            grassLandObject.LandTransition = this.LandTransition;

            return grassLandObject;
        }
    }

    public enum GrassType
    {
        NONE = -1,
        DRY = 0,
        LIGHT = 1,
        NORMAL = 2,
        BUSHY = 3
    }
}
