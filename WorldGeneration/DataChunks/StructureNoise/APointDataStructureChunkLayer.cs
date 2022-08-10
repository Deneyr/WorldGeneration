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
                return Math.Max(0, Math.Max(this.StructDimension.Width, this.StructDimension.Height) - 1);
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
            IntRect newWorldArea = new IntRect(
                worldArea.Left - this.StructureCellMargin,
                worldArea.Top - this.StructureCellMargin,
                worldArea.Width + this.StructureCellMargin,
                worldArea.Height + this.StructureCellMargin);

            IntRect chunkArea = ChunkHelper.GetChunkAreaFromWorldArea(this.NbCaseSide, newWorldArea);

            List<IDataStructure> resultDataStructures = new List<IDataStructure>();

            for (int i = 0; i < chunkArea.Height; i++)
            {
                for (int j = 0; j < chunkArea.Width; j++)
                {
                    APointDataStructureChunk pointStructureChunk = this.ChunksMonitor.GetChunkContainerAt(chunkArea.Left + j, chunkArea.Top + i).ContainedChunk as APointDataStructureChunk;

                    pointStructureChunk.AddDataStructuresFromWorldArea(worldArea, newWorldArea, resultDataStructures);
                }
            }

            // TEST
            //List<IDataStructure> resultDataStructures2 = new List<IDataStructure>();
            //foreach (ChunkContainer container in this.ChunksMonitor.CurrentChunksLoaded.Values)
            //{
            //    APointDataStructureChunk pointStructureChunk = container.ContainedChunk as APointDataStructureChunk;

            //    pointStructureChunk.DirtyAddDataStructuresFromWorldArea(worldArea, resultDataStructures2);
            //}

            //var set1 = new HashSet<IDataStructure>(resultDataStructures);
            //var set2 = new HashSet<IDataStructure>(resultDataStructures2);
            //if (set1.SetEquals(set2) == false)
            //{
            //    Console.WriteLine();
            //}
            //foreach (IDataStructure testDataStruct in resultDataStructures)
            //{
            //    IntRect testBB = testDataStruct.StructureWorldBoundingBox;

            //    IntRect chunkBB = worldArea;

            //    if (chunkBB.Intersects(testBB, out IntRect overlap) == false)
            //    {
            //        Console.WriteLine(overlap);
            //    }
            //}

            return resultDataStructures;
        }
    }
}
