using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public class TallGrassElementLandObject : AElementLandObject, ILandOverGround, IObjectStructureElement
    {
        public string ParentStructureUID
        {
            get;
            internal set;
        }

        public TallGrassElementLandObject(int landElementObjectId) : base(landElementObjectId)
        {
        }

        public override ILandObject Clone()
        {
            return new TallGrassElementLandObject(this.LandObjectId);
        }
    }
}
