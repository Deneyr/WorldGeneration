using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DSNoise;
using WorldGeneration.DataChunks.PerlinNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class TestNoiseDataAgreggator : IDataAgreggator
    {
        internal IDataChunkLayer TestLayer
        {
            get;
            set;
        }

        public TestNoiseDataAgreggator()
        {
        }

        public float GetTestValueAtWorldCoordinates(int x, int y)
        {
            //return ((this.TestLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value + 1) / 2;
            //return (int) ((this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as DSDataCase).Value * 255);
            //return (this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as BiomeDSDataCase).CurrentBiome;
            return (this.TestLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;
            // TEST
            //return 0;
        }
    }
}