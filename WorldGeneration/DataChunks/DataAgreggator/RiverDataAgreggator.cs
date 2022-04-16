using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class RiverDataAgreggator: IDataAgreggator
    {
        private static readonly float RIVER_WIDTH = 3;

        public int NbBiomeLevel
        {
            get;
            private set;
        }

        internal IDataChunkLayer RiverLayer
        {
            get;
            set;
        }

        public RiverDataAgreggator()
        {
        }

        public float GetRiverValueAtWorldCoordinates(int x, int y)
        {
            float riverBorderValue = (this.RiverLayer.GetCaseAtWorldCoordinates(x, y) as BiomeVoronoiDataCase).RiverBorderValue;

            if(riverBorderValue < RIVER_WIDTH)
            {
                riverBorderValue = (RIVER_WIDTH - riverBorderValue) / RIVER_WIDTH;
                riverBorderValue = riverBorderValue / 2;

                return this.GetRiverDepthFrom(riverBorderValue);
            }

            return 0;
        }


        public float GetRiverDepthFrom(float x)
        {
            return -0.9f * x * x * x - 2.6f * x * x + 3.5f * x; 
        }
    }
}