using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DSNoise;
using WorldGeneration.DataChunks.DSNoise.BiomeDSNoise;
using WorldGeneration.DataChunks.VoronoiNoise;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class BiomeDataAgreggator : IDataAgreggator
    {
        internal IDataChunkLayer BiomeLayer
        {
            get;
            set;
        }

        public BiomeDataAgreggator()
        {
        }

        public BiomeType GetBiomeAtWorldCoordinates(int x, int y)
        {
            return (BiomeType) (this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as VoronoiDataCase).Value;
            //return (int) ((this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as DSDataCase).Value * 255);
            //return (this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as BiomeDSDataCase).CurrentBiome;

            // TEST
            //return 0;
        }
    }
}
