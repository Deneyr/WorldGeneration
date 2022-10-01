using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public abstract class AElementLandObject : ILandObject
    {
        public int LandObjectId
        {
            get;
            private set;
        }

        public virtual LandTransition LandTransition
        {
            get
            {
                return LandTransition.NONE;
            }
            set
            {
                // Nothing to do
            }
        }

        public AElementLandObject(int landElementObjectId)
        {
            this.LandObjectId = landElementObjectId;
        }

        public ILandObject Clone(LandTransition wallLandTransition)
        {
            return this.Clone();
        }

        public abstract ILandObject Clone();
    }
}
