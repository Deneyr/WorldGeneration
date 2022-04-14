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
            return (this.RiverLayer.GetCaseAtWorldCoordinates(x, y) as BiomeVoronoiDataCase).RiverBorderValue < 3 ? 0 : 1;
        }
    }
}