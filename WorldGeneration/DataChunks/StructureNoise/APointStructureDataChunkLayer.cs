using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise
{
    internal abstract class APointStructureDataChunkLayer : AExtendedDataChunkLayer
    {
        public override int NbCaseBorder
        {
            get
            {
                return 1;
            }
        }

        public IntRect StructDimension
        {
            get;
            set;
        }

        public int NbMinDataStructure
        {
            get;
            set;
        }

        public int nbMaxDataStructure
        {
            get;
            set;
        }

        public APointStructureDataChunkLayer(string id, int nbCaseSide)
            : base(id, nbCaseSide)
        {
            this.StructDimension = new IntRect();
            this.NbMinDataStructure = 0;
            this.nbMaxDataStructure = 0;
        }

        public override ICase GetCaseAtWorldCoordinates(int x, int y)
        {
            IntRect casePosition = ChunkHelper.GetChunkPositionFromWorldPosition(this.NbCaseSide, new Vector2i(x, y));

            ChunkContainer chunkContainer = this.ChunksMonitor.GetChunkContainerAt(casePosition.Left, casePosition.Top);
            if (chunkContainer != null && chunkContainer.ContainedChunk != null)
            {
                IChunk chunk = chunkContainer.ContainedChunk;

                if (casePosition.Width >= 0
                    && casePosition.Width < chunk.NbCaseSide
                    && casePosition.Height >= 0
                    && casePosition.Height < chunk.NbCaseSide)
                {
                    return (chunk as APointStructureDataChunk).GetCaseAtLocal(this, casePosition.Width, casePosition.Height);
                }
            }
            return null;
        }
    }
}
