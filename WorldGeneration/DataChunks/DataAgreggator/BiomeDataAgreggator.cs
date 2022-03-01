using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.VoronoiNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class BiomeDataAgreggator : IDataAgreggator
    {
        public int NbBiomeLevel
        {
            get;
            private set;
        }

        internal IDataChunkLayer BiomeLayer
        {
            get;
            set;
        }

        public BiomeDataAgreggator(int nbBiomeLevel)
        {
            this.NbBiomeLevel = nbBiomeLevel;
        }

        public int GetBiomeAtWorldCoordinates(int x, int y)
        {
            return (this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as VoronoiDataCase).Value % this.NbBiomeLevel;
        }
    }
}
