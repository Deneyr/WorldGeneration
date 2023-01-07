using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace WorldGeneration.DataChunks.StructureNoise.TownStructure
{
    internal class TownDataStructureCase: ADataStructureCase
    {
        public LandTransition LandTransition
        {
            get;
            set;
        }

        public TownDataStructureCase(IDataStructure parentDataStructure, int x, int y)
            : base(parentDataStructure, x, y)
        {
            this.LandTransition = LandTransition.NONE;
        }
    }
}
