using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree
{
    public class SeasonalSideTreeElementLandObject : ASideTreeElementLandObject
    {
        public SeasonalSideTreeElementLandObject(int landElementObjectId, TreePart part)
            : base(landElementObjectId, part)
        {
        }

        public override ILandObject Clone()
        {
            return new SeasonalSideTreeElementLandObject(this.LandObjectId, this.Part);
        }
    }
}
