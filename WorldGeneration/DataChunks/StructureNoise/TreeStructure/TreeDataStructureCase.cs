using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.DataChunks.StructureNoise.TreeStructure
{
    internal class TreeDataStructureCase : ADataStructureCase
    {
        public TreeDataStructureCase(IDataStructure parentDataStructure, int x, int y)
            : base(parentDataStructure, x, y)
        {

        }
    }
}
