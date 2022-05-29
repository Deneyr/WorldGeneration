using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeStructure;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public abstract class ATreeElementLandObject : AElementLandObject
    {
        public TreePart Part
        {
            get;
            private set;
        }

        public ATreeElementLandObject(int landElementObjectId, TreePart part):
            base(landElementObjectId)
        {
            this.Part = part;
        }
    }
}
