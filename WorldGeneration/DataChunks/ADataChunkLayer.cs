using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration.DataChunks
{
    internal abstract class ADataChunkLayer : IDataChunkLayer, IDisposable
    {
        //protected HashSet<ChunkContainer> notGeneratedChunkContainers;

        protected List<ChunkContainer> newlyLoadedChunks;

        public DataChunkLayersMonitor DataChunksMonitor
        {
            protected get;
            set;
        }

        public ChunksMonitor ChunksMonitor
        {
            get;
            private set;
        }

        public string Id
        {
            get;
            private set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        public virtual int Margin
        {
            get;
            set;
        }

        public int SampleLevel
        {
            get;
            set;
        }

        public ADataChunkLayer(string id, int nbCaseSide)
        {
            this.Id = id;
            this.NbCaseSide = nbCaseSide;

            this.Margin = 0;
            this.SampleLevel = 1;

            this.newlyLoadedChunks = null;

            this.ChunksMonitor = new ChunksMonitor(0);
            this.ChunksMonitor.ChunksToLoad += OnChunksToLoad;
            this.ChunksMonitor.ChunksToUnload += OnChunksToUnload;

            //this.notGeneratedChunkContainers = new HashSet<ChunkContainer>();
            //this.LoadedChunks = new Dictionary<Vector2i, IDataChunk>();
        }

        protected virtual void OnChunksToLoad(List<ChunkContainer> obj)
        {
            this.InternalCreateChunks(this.ChunksMonitor, obj);

            this.newlyLoadedChunks = obj;
        }

        protected virtual void OnChunksToUnload(List<ChunkContainer> obj)
        {
            //foreach (ChunkContainer chunkContainer in obj)
            //{
            //    this.notGeneratedChunkContainers.Remove(chunkContainer);
            //}
        }

        public virtual void UpdateLayerArea(IntRect newWorldArea)
        {
            if (this.Margin > 0)
            {
                newWorldArea = new IntRect(
                    newWorldArea.Left - this.Margin,
                    newWorldArea.Top - this.Margin,
                    newWorldArea.Width + 2 * this.Margin,
                    newWorldArea.Height + 2 * this.Margin);
            }

            IntRect newLayerArea = ChunkHelper.GetChunkAreaFromWorldArea(this.NbCaseSide, newWorldArea);
            this.ChunksMonitor.UpdateChunksArea(newLayerArea);

            if (this.newlyLoadedChunks != null)
            {
                this.InternalPrepareChunks(this.newlyLoadedChunks);

                this.InternalGenerateChunks(this.newlyLoadedChunks);

                this.newlyLoadedChunks = null;
            }

            //this.InternalEndingChunksGeneration();
        }

        protected abstract void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj);

        protected virtual void InternalPrepareChunks(List<ChunkContainer> obj)
        {
            foreach(ChunkContainer chunkContainer in obj)
            {
                (chunkContainer.ContainedChunk as IDataChunk).PrepareChunk(this.DataChunksMonitor, this);
            }
        }

        protected virtual void InternalGenerateChunks(List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainer in obj)
            {
                (chunkContainer.ContainedChunk as IDataChunk).GenerateChunk(this.DataChunksMonitor, this);
            }
        }

        public virtual ICase GetCaseAtWorldCoordinates(int x, int y)
        {
            IntRect casePosition = ChunkHelper.GetChunkPositionFromWorldPosition(this.NbCaseSide, new Vector2i(x, y));

            ChunkContainer chunkContainer = this.ChunksMonitor.GetChunkContainerAt(casePosition.Left, casePosition.Top);
            if(chunkContainer != null && chunkContainer.ContainedChunk != null)
            {
                return ChunkHelper.GetCaseAtLocalCoordinates(chunkContainer.ContainedChunk, casePosition.Width, casePosition.Height);
            }
            return null;
        }

        public virtual void Dispose()
        {
            this.ChunksMonitor.ChunksToLoad -= OnChunksToLoad;
            this.ChunksMonitor.ChunksToUnload -= OnChunksToUnload;
        }
    }
}
