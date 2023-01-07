using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.TownStructure
{
    internal class TownDataStructureChunkLayer : APointDataStructureChunkLayer
    {
        public TownDataStructureChunkLayer(string id, int nbCaseSide)
            : base(id, nbCaseSide)
        {
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                TownDataStructureChunk structureDataChunk = new TownDataStructureChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NbMinDataStructure, this.NbMaxDataStructure, this.StructDimension);

                dataChunksMonitor.AddChunkToMonitor(structureDataChunk);
            }
        }
    }
}