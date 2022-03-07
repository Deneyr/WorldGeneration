using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.DSNoise
{
    internal class DSDataChunkLayer : AExtendedDataChunkLayer
    {
        public int NbPower
        {
            get;
            private set;
        }

        public int CurrentNbStep
        {
            get;
            private set;
        }

        public int CurrentStepLength
        {
            get;
            private set;
        }

        public int CurrentStepPower
        {
            get;
            private set;
        }

        public override int NbCaseBorder
        {
            get
            {
                return 2;
            }
        }

        public DSDataChunkLayer(string id, int nbPower)
            : base(id, (int) Math.Pow(2, nbPower))
        {
            this.NbPower = nbPower;
            this.CurrentNbStep = 0;
            this.CurrentStepLength = 0;
            this.CurrentStepPower = 1;
        }

        public override void UpdateLayerArea(IntRect newWorldArea)
        {
            this.CurrentNbStep = 0;
            this.CurrentStepLength = this.NbCaseSide;
            this.CurrentStepPower = 1;

            IntRect newLayerArea = ChunkHelper.GetChunkAreaFromWorldArea(this.NbCaseSide, newWorldArea);

            IntRect newExtendedLayerArea = new IntRect(
                newLayerArea.Left - this.NbCaseBorder,
                newLayerArea.Top - this.NbCaseBorder,
                newLayerArea.Width + this.NbCaseBorder * 2,
                newLayerArea.Height + this.NbCaseBorder * 2);

            this.ExtendedChunksMonitor.UpdateChunksArea(newExtendedLayerArea);

            this.ChunksMonitor.UpdateChunksArea(newLayerArea);

            // Prepare
            if (this.newlyExtendedLoadedChunks != null)
            {
                this.InternalPrepareChunks(this.newlyExtendedLoadedChunks);

                this.newlyExtendedLoadedChunks = null;
            }

            //Dictionary<Vector2i, ChunkContainer> chunksToGenerateDictionary = new Dictionary<Vector2i, ChunkContainer>();
            //if (this.newlyExtendedLoadedChunks != null)
            //{
            //    foreach (ChunkContainer chunkContainer in this.newlyExtendedLoadedChunks)
            //    {
            //        chunksToGenerateDictionary[chunkContainer.Position] = chunkContainer;
            //    }
            //}
            //if (this.newlyLoadedChunks != null)
            //{
            //    foreach (ChunkContainer chunkContainer in this.newlyLoadedChunks)
            //    {
            //        chunksToGenerateDictionary[chunkContainer.Position] = chunkContainer;
            //    }
            //}
            //List<ChunkContainer> chunksToGenerate = chunksToGenerateDictionary.Values.ToList();

            // Generate
            if (this.newlyLoadedChunks != null)
            {
                List<ChunkContainer> chunksToGenerate = this.ExtendedChunksMonitor.CurrentChunksLoaded.Values.Where(pElem => (pElem.ContainedChunk as DSDataChunk).MustBeGenerated(this)).ToList();

                for (int i = 0; i < this.NbPower; i++)
                {
                    this.InternalGenerateChunks(chunksToGenerate);

                    this.CurrentNbStep++;

                    this.InternalGenerateChunks(chunksToGenerate);

                    this.CurrentNbStep++;

                    this.CurrentStepLength /= 2;
                    this.CurrentStepPower *= 2;
                }

                this.newlyLoadedChunks = null;
            }

            //this.InternalEndingChunksGeneration();
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                DSDataChunk dSDataChunk = new DSDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide);

                dataChunksMonitor.AddChunkToMonitor(dSDataChunk);
            }
        }
    }
}
