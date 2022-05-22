using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.GroundObject
{
    public class GroundLandObject: ALandObject, ILandGround
    {
        public LandType Type
        {
            get;
            protected set;
        }

        public GroundLandObject(int landObjectId, LandType landType): 
            base(landObjectId)
        {
            this.Type = landType;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GroundLandObject groundLandObject = new GroundLandObject(this.LandObjectId, this.Type);
                groundLandObject.LandTransition = landTransitionOverWall;

                return groundLandObject;
            }
            return null;
        }

        public virtual ILandObject Clone(LandType landType, LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GroundLandObject groundLandObject = new GroundLandObject(this.LandObjectId, landType);
                groundLandObject.LandTransition = landTransitionOverWall;

                return groundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GroundLandObject groundLandObject = new GroundLandObject(this.LandObjectId, this.Type);
            groundLandObject.LandTransition = this.LandTransition;

            return groundLandObject;
        }
    }

    public enum LandType
    {
        GROUND = 0,
        SEA_DEPTH = 1,
        SAND = 2,
        GRASS = 3,
        MONTAIN = 4,
        SNOW = 5,
    }
}
