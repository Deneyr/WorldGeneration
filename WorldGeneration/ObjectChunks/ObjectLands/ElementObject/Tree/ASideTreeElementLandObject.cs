using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree
{
    public abstract class ASideTreeElementLandObject : ATreeElementLandObject, ILandOverGround
    {
        public ASideTreeElementLandObject(int landElementObjectId, TreePart part)
            : base(landElementObjectId, part)
        {
        }

        //public override ILandObject Clone()
        //{
        //    return new ASideTreeElementLandObject(this.LandObjectId, this.Part);
        //}
    }
}
