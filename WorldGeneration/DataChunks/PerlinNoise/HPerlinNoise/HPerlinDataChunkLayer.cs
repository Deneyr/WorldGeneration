using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.PerlinNoise.HPerlinNoise
{
    internal class HPerlinDataChunkLayer : PerlinDataChunkLayer
    {
        public HPerlinDataChunkLayer(string id, int noiseFrequency, int nbInstanceSide)
            : base(id, noiseFrequency, nbInstanceSide)
        {
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                HPerlinDataChunk hPerlinDataChunk = new HPerlinDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NoiseFrequency, this.SampleLevel);

                dataChunksMonitor.AddChunkToMonitor(hPerlinDataChunk);
            }
        }
    }
}
