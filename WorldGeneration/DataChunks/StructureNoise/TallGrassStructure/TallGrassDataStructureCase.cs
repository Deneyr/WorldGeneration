using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace WorldGeneration.DataChunks.StructureNoise.TallGrassStructure
{
    internal class TallGrassDataStructureCase : ADataStructureCase
    {
        public LandTransition LandTransition
        {
            get;
            set;
        }

        public TallGrassDataStructureCase(IDataStructure parentDataStructure, int x, int y) 
            : base(parentDataStructure, x, y)
        {
            this.LandTransition = LandTransition.NONE;
        }
    }
}
