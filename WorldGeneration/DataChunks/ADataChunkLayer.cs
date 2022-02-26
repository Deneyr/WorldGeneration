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

        public DataChunkLayersMonitor DataChunksMonitor
        {
            protected get;
            set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        public ADataChunkLayer(string id, int nbCaseSide)
        {
            this.Id = id;
            this.NbCaseSide = nbCaseSide;

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
                (chunkContainer.ContainedChunk as ADataChunk).PrepareChunk(this.DataChunksMonitor, this);
            }
        }

        protected virtual void InternalGenerateChunks(List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainer in obj)
            {
                (chunkContainer.ContainedChunk as ADataChunk).GenerateChunk(this.DataChunksMonitor, this);
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

        //protected virtual void InternalEndingChunksGeneration()
        //{
        //    if (this.notGeneratedChunkContainers.Count > 0)
        //    {
        //        IntRect trueArea = this.GetTrueEffectiveArea(this.ChunksMonitor.CurrentArea);

        //        HashSet<ChunkContainer> notGeneratedContainerClone = new HashSet<ChunkContainer>(this.notGeneratedChunkContainers);
        //        foreach (ChunkContainer chunkContainer in notGeneratedContainerClone)
        //        {
        //            IntRect chunkArea = new IntRect(chunkContainer.Position.X, chunkContainer.Position.Y, 1, 1);
        //            if (trueArea.Intersects(chunkArea))
        //            {
        //                if ((chunkContainer.ContainedChunk as ADataChunk).EndGenerateChunk(this.DataChunksMonitor, this))
        //                {
        //                    this.notGeneratedChunkContainers.Remove(chunkContainer);
        //                }
        //            }
        //        }
        //    }
        //}

        //protected virtual IntRect GetLayerFromWorldArea(IntRect newWorldArea)
        //{
        //    IntRect layerArea = ChunkHelper.GetChunkAreaFromWorldArea(this.NbCaseSide, newWorldArea);

        //    return new IntRect(
        //        layerArea.Left - this.NbCaseBorder,
        //        layerArea.Top - this.NbCaseBorder,
        //        layerArea.Width + this.NbCaseBorder * 2,
        //        layerArea.Height + this.NbCaseBorder * 2);

        //}

        //protected virtual IntRect GetTrueEffectiveArea(IntRect newWorldArea)
        //{
        //    return new IntRect(
        //        newWorldArea.Left + this.NbCaseBorder,
        //        newWorldArea.Top + this.NbCaseBorder,
        //        newWorldArea.Width - this.NbCaseBorder * 2,
        //        newWorldArea.Height - this.NbCaseBorder * 2);

        //}

        public virtual void Dispose()
        {
            this.ChunksMonitor.ChunksToLoad -= OnChunksToLoad;
            this.ChunksMonitor.ChunksToUnload -= OnChunksToUnload;
        }

        //public bool LoadDataChunk(Vector2i chunkPosition)
        //{
        //    if (this.LoadedChunks.TryGetValue(chunkPosition, out IDataChunk dataChunk))
        //    {
        //        throw new Exception("Already loaded data chunk : " + chunkPosition);
        //    }

        //    return this.InternalLoadDataChunk(chunkPosition);
        //}

        //protected abstract bool InternalLoadDataChunk(Vector2i chunkPosition);

        //public bool UnLoadDataChunk(Vector2i chunkPosition)
        //{
        //    return this.LoadedChunks.Remove(chunkPosition);
        //}
    }
}
