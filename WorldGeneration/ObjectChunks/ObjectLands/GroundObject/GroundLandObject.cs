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

        public GroundLandObject(LandType landType): 
            base()
        {
            this.Type = landType;
        }

        public void SetTransition(LandTransition landTransition)
        {
            this.LandTransition = landTransition;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GroundLandObject groundLandObject = new GroundLandObject(this.Type);
                groundLandObject.SetTransition(landTransitionOverWall);

                return groundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GroundLandObject groundLandObject = new GroundLandObject(this.Type);
            groundLandObject.SetTransition(this.LandTransition);

            return groundLandObject;
        }
    }

    public enum LandType
    {
        GROUND = 0,
        SAND = 1,
        GRASS = 2,
        STONE = 3,
        SNOW = 4
    }
}
