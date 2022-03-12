using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration.DataChunks.PerlinNoise
{
    internal class PerlinDataChunkLayer : AExtendedDataChunkLayer
    {
        public int NoiseFrequency
        {
            get;
            private set;
        }

        public override int NbCaseBorder
        {
            get
            {
                return 1;
            }
        }

        public PerlinDataChunkLayer(string id, int noiseFrequency, int nbInstanceSide) 
            : base(id, noiseFrequency * nbInstanceSide)
        {
            this.NoiseFrequency = noiseFrequency;
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                PerlinDataChunk perlinDataChunk = new PerlinDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NoiseFrequency);
                perlinDataChunk.SampleLevel = this.SampleLevel;

                dataChunksMonitor.AddChunkToMonitor(perlinDataChunk);
            }
        }
    }
}
