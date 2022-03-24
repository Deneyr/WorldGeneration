using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DSNoise;
using WorldGeneration.DataChunks.DSNoise.BiomeDSNoise;
using WorldGeneration.DataChunks.VoronoiNoise;
using WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise;
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

        public BiomeType GetBiomeAtWorldCoordinates(int x, int y, out float borderValue)
        {
            BiomeVoronoiDataCase biomeVoronoiDataCase = this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as BiomeVoronoiDataCase;

            borderValue = biomeVoronoiDataCase.BorderValue;

            return (BiomeType) biomeVoronoiDataCase.Value;
            //return (int) ((this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as DSDataCase).Value * 255);
            //return (this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as BiomeDSDataCase).CurrentBiome;

            // TEST
            //return 0;
        }
    }
}
