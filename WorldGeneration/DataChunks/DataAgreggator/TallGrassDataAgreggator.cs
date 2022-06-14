using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.StructureNoise.TallGrassStructure;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class TallGrassDataAgreggator : IDataAgreggator
    {
        public TallGrassStructureDataChunkLayer TallGrassBiome
        {
            get;
            set;
        }

        public TallGrassStructureDataChunkLayer SecondTallGrassBiome
        {
            get;
            set;
        }

        public TallGrassDataAgreggator()
        {

        }

        public List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            List<IDataStructure> resultDataStructures = this.TallGrassBiome.GetDataStructuresInWorldArea(worldArea);

            resultDataStructures.AddRange(this.SecondTallGrassBiome.GetDataStructuresInWorldArea(worldArea));

            return resultDataStructures;
        }

        public bool IsThereTallGrassAtWorldCoordinates(int x, int y)
        {
            TallGrassStructureCase tallGrassStructureCase = this.TallGrassBiome.GetCaseAtWorldCoordinates(x, y) as TallGrassStructureCase;

            if (tallGrassStructureCase == null)
            {
                tallGrassStructureCase = this.SecondTallGrassBiome.GetCaseAtWorldCoordinates(x, y) as TallGrassStructureCase;
            }

            return tallGrassStructureCase != null;
        }
    }
}
