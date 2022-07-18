using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.PerlinNoise.HPerlinNoise;
using WorldGeneration.DataChunks.PureNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class FloraDataAgreggator : IDataAgreggator
    {
        internal HPerlinDataChunkLayer FloraLayer
        {
            get;
            set;
        }

        internal PureNoiseDataChunkLayer PureNoiseLayer
        {
            get;
            set;
        }

        public FloraDataAgreggator()
        {
        }

        public float GetFloraVariationAtWorldCoordinate(int x, int y)
        {
            return (this.FloraLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;
        }

        public bool IsThereTreeAtWorldCoordinate(int x, int y, float treeRatio)
        {
            float perlinValue = (this.FloraLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;

            perlinValue = perlinValue * perlinValue;

            return this.GenerateRandomValue(x, y, "IsThereTreeAtWorldCoordinate".GetHashCode()) < (treeRatio * perlinValue);
        }

        public bool IsThereFlowerAtWorldCoordinate(int x, int y, float flowerRatio)
        {
            return this.GenerateRandomValue(x, y, "IsThereFlowerAtWorldCoordinate".GetHashCode()) < flowerRatio;
        }

        public bool IsThereRockAtWorldCoordinate(int x, int y, float rockRatio)
        {
            return this.GenerateRandomValue(x, y, "IsThereRockAtWorldCoordinate".GetHashCode()) < rockRatio;
        }

        protected virtual float GenerateRandomValue(int x, int y, int additionaInteger)
        {
            additionaInteger = (Math.Abs(additionaInteger) % 1000) + 1;

            float noiseValue = (this.PureNoiseLayer.GetCaseAtWorldCoordinates(x, y) as PureNoiseDataCase).Value;

            noiseValue = noiseValue * additionaInteger;
            noiseValue = noiseValue - ((int)noiseValue);

            return noiseValue;
        }

        //public bool IsThereBlockAtWorldCoordinate(int x, int y, float blockRatio)
        //{
        //    int additionaInteger = "IsThereBlockAtWorldCoordinate".GetHashCode();

        //    float perlinValue = (this.FloraLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;

        //    int seed = (int) (((perlinValue * 1000) % 1000) * ((perlinValue * 1000000) % 1000) + perlinValue % 1000);
        //    seed = seed * seed * seed * seed;

        //    int generatedInteger = (x - y + additionaInteger + seed) % (y - additionaInteger) - x + y * additionaInteger * y + y - seed + additionaInteger;

        //    float result = 0;
        //    float multiplier = 0.1f;
        //    for (int i = 0; i < 4; i++)
        //    {
        //        result += (generatedInteger % 10) * multiplier;

        //        multiplier = multiplier / 10;
        //        generatedInteger = generatedInteger / 10;
        //    }

        //    result = (float)Math.Abs(Math.Sin(generatedInteger * Math.PI / 180));

        //    return result < blockRatio;
        //}
    }
}