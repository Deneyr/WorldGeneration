using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;
using WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public class MainTreeElementLandObject : ATreeElementLandObject, ILandWall
    {
        public MainTreeElementLandObject(int landElementObjectId, TreePart part) 
            : base(landElementObjectId, part)
        {
        }

        public override ILandObject Clone()
        {
            return new MainTreeElementLandObject(this.LandObjectId, this.Part);
        }
    }
}
