using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise
{
    internal class BiomeVoronoiDataChunkLayer : VoronoiDataChunkLayer
    {
        public BiomeVoronoiDataChunkLayer(string id, int nbPointsInside, int nbCaseSide) 
            : base(id, nbPointsInside, nbCaseSide)
        {
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                BiomeVoronoiDataChunk voronoiDataChunk = new BiomeVoronoiDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NbPointsInside, this.SampleLevel);

                dataChunksMonitor.AddChunkToMonitor(voronoiDataChunk);
            }
        }
    }
}
