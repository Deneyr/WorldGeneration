using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks
{
    internal abstract class AExtendedDataChunkLayer : ADataChunkLayer
    {
        protected List<ChunkContainer> newlyExtendedLoadedChunks;

        public virtual int NbCaseBorder
        {
            get
            {
                return 0;
            }
        }

        public ChunksMonitor ExtendedChunksMonitor
        {
            get;
            private set;
        }

        public AExtendedDataChunkLayer(string id, int nbCaseSide)
            : base(id, nbCaseSide)
        {
            this.newlyExtendedLoadedChunks = null;

            this.ExtendedChunksMonitor = new ChunksMonitor(0);
            this.ExtendedChunksMonitor.ChunksToLoad += OnExtendedChunksToLoad;
            this.ExtendedChunksMonitor.ChunksToUnload += OnExtendedChunksToUnload;
        }

        public override void UpdateLayerArea(IntRect newWorldArea)
        {
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

            // Generate
            if (this.newlyLoadedChunks != null)
            {
                this.InternalGenerateChunks(this.newlyLoadedChunks);

                this.newlyLoadedChunks = null;
            }

            //this.InternalEndingChunksGeneration();
        }

        protected void OnExtendedChunksToLoad(List<ChunkContainer> obj)
        {
            this.InternalCreateChunks(this.ExtendedChunksMonitor, obj);

            this.newlyExtendedLoadedChunks = obj;
        }

        protected override void OnChunksToLoad(List<ChunkContainer> obj)
        {
            foreach(ChunkContainer chunkContainer in obj)
            {
                ChunkContainer extendedChunkContainer = this.ExtendedChunksMonitor.GetChunkContainerAt(chunkContainer.Position.X, chunkContainer.Position.Y);

                this.ChunksMonitor.AddChunkToMonitor(extendedChunkContainer.ContainedChunk);
            }

            this.newlyLoadedChunks = obj;
        }

        protected virtual void OnExtendedChunksToUnload(List<ChunkContainer> obj)
        {
            //foreach (ChunkContainer chunkContainer in obj)
            //{
            //    this.notGeneratedChunkContainers.Remove(chunkContainer);
            //}
        }

        public override void Dispose()
        {
            this.ExtendedChunksMonitor.ChunksToLoad -= OnExtendedChunksToLoad;
            this.ExtendedChunksMonitor.ChunksToUnload -= OnExtendedChunksToUnload;

            base.Dispose();
        }
    }
}
