using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.PerlinNoise.HPerlinNoise;
using WorldGeneration.DataChunks.PureNoise;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class TreeDataAgreggator : AStructureDataAgreggator
    {
        internal HPerlinDataChunkLayer ForestLayer
        {
            get;
            set;
        }

        internal PureNoiseDataChunkLayer PureNoiseLayer
        {
            get;
            set;
        }

        public override List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            List<IDataStructure> realResultDataStructures = new List<IDataStructure>();
            List<IDataStructure> resultDataStructures = base.GetDataStructuresInWorldArea(worldArea);

            resultDataStructures = resultDataStructures.Where(pElem => this.IsThereTreeAtWorldCoordinate(pElem.StructureWorldPosition.X, pElem.StructureWorldPosition.Y)).ToList();

            return resultDataStructures;
        }

        private bool IsThereTreeAtWorldCoordinate(int x, int y)
        {
            float perlinValue = (this.ForestLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;

            perlinValue = perlinValue * perlinValue;

            return this.GenerateRandomValue(x, y, "IsThereTreeAtWorldCoordinate".GetHashCode()) < perlinValue;
        }


        protected virtual float GenerateRandomValue(int x, int y, int additionaInteger)
        {
            additionaInteger = (Math.Abs(additionaInteger) % 1000) + 1;

            float noiseValue = (this.PureNoiseLayer.GetCaseAtWorldCoordinates(x, y) as PureNoiseDataCase).Value;

            noiseValue = noiseValue * additionaInteger;
            noiseValue = noiseValue - ((int)noiseValue);

            return noiseValue;
        }
    }
}
