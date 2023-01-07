using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject
{
    public abstract class ATownGroundLandObject : GroundLandObject, IObjectStructureElement
    {
        public string ParentStructureUID
        {
            get;
            internal set;
        }

        public ATownGroundLandObject(int landObjectId, LandType landType) 
            : base(landObjectId, landType)
        {
        }
    }
}

