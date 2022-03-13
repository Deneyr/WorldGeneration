using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.DSNoise.BiomeDSNoise
{
    internal class BiomeDSDataChunkLayer : DSDataChunkLayer
    {
        public int NbBiome
        {
            get;
            private set;
        }

        public BiomeDSDataChunkLayer(string id, int nbPower, int nbBiome) 
            : base(id, nbPower)
        {
            this.NbBiome = nbBiome;
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                BiomeDSDataChunk biomeDSDataChunk = new BiomeDSDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NbBiome, this.SampleLevel);

                dataChunksMonitor.AddChunkToMonitor(biomeDSDataChunk);
            }
        }
    }
}
