using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.TallGrassStructure
{
    internal class TallGrassDataStructureChunkLayer : APointDataStructureChunkLayer
    {
        public TallGrassDataStructureChunkLayer(string id, int nbCaseSide) 
            : base(id, nbCaseSide)
        {
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                TallGrassDataStructureChunk structureDataChunk = new TallGrassDataStructureChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NbMinDataStructure, this.nbMaxDataStructure, this.StructDimension);

                dataChunksMonitor.AddChunkToMonitor(structureDataChunk);
            }
        }
    }
}
