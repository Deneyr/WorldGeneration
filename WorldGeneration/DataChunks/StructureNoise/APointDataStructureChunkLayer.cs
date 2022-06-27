using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.DataChunks.StructureNoise
{
    internal abstract class APointDataStructureChunkLayer : AExtendedDataChunkLayer
    {
        public override int NbCaseBorder
        {
            get
            {
                return 1;
            }
        }

        public virtual int StructureCellMargin
        {
            get
            {
                return Math.Max(this.StructDimension.Width, this.StructDimension.Height);
            }
        }

        public override int Margin
        {
            get
            {
                return this.StructureCellMargin;
            }
            set
            {
                // Nothing to do.
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

        public APointDataStructureChunkLayer(string id, int nbCaseSide)
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
                    return (chunk as APointDataStructureChunk).GetCaseAtLocal(this, casePosition.Width, casePosition.Height);
                }
            }
            return null;
        }

        public List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            // Only the left top corner is extended
            worldArea = new IntRect(
                worldArea.Left - this.StructureCellMargin,
                worldArea.Top - this.StructureCellMargin,
                worldArea.Width + this.StructureCellMargin,
                worldArea.Height + this.StructureCellMargin);

            IntRect chunkArea = ChunkHelper.GetChunkAreaFromWorldArea(this.NbCaseSide, worldArea);

            List<IDataStructure> resultDataStructures = new List<IDataStructure>();

            for (int i = 0; i < chunkArea.Height; i++)
            {
                for (int j = 0; j < chunkArea.Width; j++)
                {
                    APointDataStructureChunk pointStructureChunk = this.ChunksMonitor.GetChunkContainerAt(chunkArea.Left + j, chunkArea.Top + i).ContainedChunk as APointDataStructureChunk;

                    pointStructureChunk.AddDataStructuresFromWorldArea(worldArea, resultDataStructures);
                }
            }

            return resultDataStructures;
        }
    }
}
