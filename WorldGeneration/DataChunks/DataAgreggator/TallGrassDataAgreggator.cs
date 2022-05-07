using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.TallGrassStructure;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class TallGrassDataAgreggator : IDataAgreggator
    {
        public IDataChunkLayer TallGrassBiome
        {
            get;
            set;
        }

        public TallGrassDataAgreggator()
        {

        }

        public int IsThereTallGrassAtWorldCoordinates(int x, int y)
        {
            TallGrassStructureCase tallGrassStructureCase = this.TallGrassBiome.GetCaseAtWorldCoordinates(x, y) as TallGrassStructureCase;

            if(tallGrassStructureCase == null)
            {
                return -1;
            }

            return tallGrassStructureCase.IsNull ? 0 : 1;
        }
    }
}
