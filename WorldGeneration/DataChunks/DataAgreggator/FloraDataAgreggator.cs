using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.PerlinNoise.HPerlinNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class FloraDataAgreggator : IDataAgreggator
    {
        internal HPerlinDataChunkLayer FloraLayer
        {
            get;
            set;
        }

        public FloraDataAgreggator()
        {
        }

        public bool IsThereTreeAtWorldCoordinate(int x, int y, float treeRatio, double randomValue)
        {
            float perlinValue = (this.FloraLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;

            perlinValue = perlinValue * perlinValue;

            return randomValue < (treeRatio * perlinValue);
        }

        public bool IsThereFlowerAtWorldCoordinate(int x, int y, float flowerRatio, double randomValue)
        {
            return randomValue < flowerRatio;
        }
    }
}