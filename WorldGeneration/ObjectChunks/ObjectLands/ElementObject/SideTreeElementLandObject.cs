using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeStructure;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public class SideTreeElementLandObject : ATreeElementLandObject, ILandOverGround
    {
        public SideTreeElementLandObject(int landElementObjectId, TreePart part)
            : base(landElementObjectId, part)
        {
        }

        public override ILandObject Clone()
        {
            return new SideTreeElementLandObject(this.LandObjectId, this.Part);
        }
    }
}
