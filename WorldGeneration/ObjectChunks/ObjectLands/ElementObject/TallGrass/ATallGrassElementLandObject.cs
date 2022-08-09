using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass
{
    public abstract class ATallGrassElementLandObject : AElementLandObject, ILandOverGround, IObjectStructureElement
    {
        public string ParentStructureUID
        {
            get;
            internal set;
        }

        public ATallGrassElementLandObject(int landElementObjectId) : base(landElementObjectId)
        {
        }
    }
}
