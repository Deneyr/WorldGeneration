using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace WorldGeneration.ObjectChunks.ObjectLands
{
    public abstract class ALandObject : ILandObject
    {
        public int LandObjectId
        {
            get;
            private set;
        }

        public LandTransition LandTransition
        {
            get;
            set;
        }

        public ALandObject(int landObjectId)
        {
            this.LandObjectId = landObjectId;

            this.LandTransition = LandTransition.NONE;
        }

        public abstract ILandObject Clone(LandTransition wallLandTransition);

        public abstract ILandObject Clone();

        public LandTransition GetLandTransitionOverWall(LandTransition wallLandTransition)
        {
            return LandTransitionHelper.IntersectionLandTransition(wallLandTransition, this.LandTransition);
        }

        public static LandTransition InverseLandTransition(LandTransition landTransition)
        {
            return LandTransitionHelper.ReverseLandTransition(landTransition);
        }
    }

    public enum LandTransition
    {
        NONE,
        RIGHT,
        LEFT,
        TOP,
        BOT,
        TOP_LEFT,
        BOT_LEFT,
        TOP_RIGHT,
        BOT_RIGHT,
        TOP_INT_LEFT,
        BOT_INT_LEFT,
        TOP_INT_RIGHT,
        BOT_INT_RIGHT,

        // To implement
        DIAGONAL_RIGHT,
        DIAGONAL_LEFT
    }
}
