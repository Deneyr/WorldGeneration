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

        public IDataChunkLayer SecondTallGrassBiome
        {
            get;
            set;
        }

        public TallGrassDataAgreggator()
        {

        }

        public bool IsThereTallGrassAtWorldCoordinates(int x, int y)
        {
            TallGrassStructureCase tallGrassStructureCase = this.TallGrassBiome.GetCaseAtWorldCoordinates(x, y) as TallGrassStructureCase;

            if(tallGrassStructureCase == null)
            {
                tallGrassStructureCase = this.SecondTallGrassBiome.GetCaseAtWorldCoordinates(x, y) as TallGrassStructureCase;
            }

            return tallGrassStructureCase != null;
        }
    }
}
