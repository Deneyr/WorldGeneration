using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora
{
    public abstract class AFloraElementLandObject : AElementLandObject, ILandOverGround
    {
        public LandType LandType
        {
            get;
            internal set;
        }

        public AFloraElementLandObject(int landElementObjectId) 
            : base(landElementObjectId)
        {
        }
    }
}
