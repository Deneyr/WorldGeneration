using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.TreeStructure
{
    internal class TreeDataStructureChunkLayer : APointDataStructureChunkLayer
    {
        public TreeDataStructureChunkLayer(string id, int nbCaseSide)
            : base(id, nbCaseSide)
        {
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                TreeDataStructureChunk structureDataChunk = new TreeDataStructureChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NbMinDataStructure, this.nbMaxDataStructure, this.StructDimension);

                dataChunksMonitor.AddChunkToMonitor(structureDataChunk);
            }
        }
    }
}