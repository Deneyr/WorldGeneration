using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.GrassObject
{
    public class GrassElementLandObject : ALandObject, ILandOverGround
    {
        public GrassType LandGrassType
        {
            get;
            private set;
        }

        public int ElementIndex
        {
            get;
            private set;
        }

        public GrassElementLandObject(GrassType grassType, int elementIndex) :
            base()
        {
            this.LandGrassType = grassType;
            this.ElementIndex = elementIndex;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            if (wallLandTransition != LandTransition.NONE)
            {
                GrassElementLandObject grassLandObject = new GrassElementLandObject(this.LandGrassType, this.ElementIndex);

                return grassLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GrassElementLandObject grassLandObject = new GrassElementLandObject(this.LandGrassType, this.ElementIndex);

            return grassLandObject;
        }
    }
}
