using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.PureNoise
{
    internal class PureNoiseDataChunkLayer : ADataChunkLayer
    {
        public PureNoiseDataChunkLayer(string id, int nbInstanceSide)
            : base(id, nbInstanceSide)
        {

        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                PureNoiseDataChunk perlinDataChunk = new PureNoiseDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.SampleLevel);

                dataChunksMonitor.AddChunkToMonitor(perlinDataChunk);
            }
        }
    }
}
