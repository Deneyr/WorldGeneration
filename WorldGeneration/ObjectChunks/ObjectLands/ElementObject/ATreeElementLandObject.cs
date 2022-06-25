using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectStructures;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeStructure;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public abstract class ATreeElementLandObject : AElementLandObject, IObjectStructureElement
    {
        public string ParentStructureUID
        {
            get;
            internal set;
        }

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
