using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.WorldGenerating
{
    public class WorldMonitor: IDisposable
    {
        private readonly object mainLock = new object();

        private IntRect worldArea;

        private Dictionary<Vector2i, ChunkContainer> pendingChunkContainersToOrder;
        private Dictionary<Vector2i, IChunk> pendingChunkContainersGenerated;

        public int NbChunkCaseSide
        {
            get;
            private set;
        }

        public IntRect WorldArea
        {
            get
            {
                return this.worldArea;
            }
            set
            {
                if(this.worldArea != value)
                {
                    this.worldArea = value;
                }
            }
        }

        public IntRect ChunkArea
        {
            get
            {
                IntRect layerArea = ChunkHelper.GetChunkAreaFromWorldArea(this.NbChunkCaseSide, this.WorldArea);
                //return layerArea;
                return new IntRect(
                    layerArea.Left - 1,
                    layerArea.Top - 1,
                    layerArea.Width + 2,
                    layerArea.Height + 2);
            }
        }

        public ChunksMonitor MainChunksMonitor
        {
            get;
            private set;
        }

        internal WorldGenerator WorldGenerator
        {
            get;
            private set;
        }

        public WorldMonitor(int nbChunkCaseSide, int poolLimit, int seed)
        {
            this.NbChunkCaseSide = nbChunkCaseSide;

            this.WorldArea = new IntRect(0, 0, 0, 0);

            this.WorldGenerator = new WorldGenerator(nbChunkCaseSide, seed);

            this.WorldGenerator.ChunkGenerated += OnChunkGenerated;

            this.MainChunksMonitor = new ChunksMonitor(poolLimit);

            this.pendingChunkContainersGenerated = new Dictionary<Vector2i, IChunk>();
            this.pendingChunkContainersToOrder = new Dictionary<Vector2i, ChunkContainer>();

            this.MainChunksMonitor.ChunksToLoad += OnChunksToLoad;
            this.MainChunksMonitor.ChunksToAdd += OnChunksAdded;

            this.MainChunksMonitor.ChunksRemoved += OnChunksRemoved;
            this.MainChunksMonitor.ChunksToUnload += OnChunksToUnload;
        }

        private void OnChunkGenerated(IChunk chunkGenerated)
        {
            Vector2i chunkGeneratedPosition = new Vector2i(chunkGenerated.Position.X, chunkGenerated.Position.Y);

            lock (this.mainLock)
            {
                this.pendingChunkContainersGenerated.Add(chunkGeneratedPosition, chunkGenerated);
            }
        }

        private void OnChunksToLoad(List<ChunkContainer> obj)
        {
            foreach(ChunkContainer chunkContainer in obj)
            {
                this.pendingChunkContainersToOrder.Add(chunkContainer.Position, chunkContainer);
            }
            //chunkLoaded += obj.Count;
        }

        private void OnChunksAdded(List<ChunkContainer> obj)
        {
            //chunkAdded += obj.Count;
        }

        private void OnChunksRemoved(List<ChunkContainer> obj)
        {
            //chunkAdded -= obj.Count;
        }

        //private int chunkAdded = 0;
        //private int chunkLoaded = 0;

        private void OnChunksToUnload(List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainer in obj)
            {
                this.pendingChunkContainersToOrder.Remove(chunkContainer.Position);
            }
            //chunkLoaded -= obj.Count;
        }

        public void UpdateWorld(Time deltaTime)
        {
            this.MainChunksMonitor.UpdateChunksArea(this.ChunkArea);
            IntRect chunkWorldArea = ChunkHelper.GetWorldAreaFromChunkArea(this.NbChunkCaseSide, this.MainChunksMonitor.CurrentArea);

            if(this.pendingChunkContainersToOrder.Count > 0)
            { 
                KeyValuePair<Vector2i, ChunkContainer> chunkContainerToGenerate = this.pendingChunkContainersToOrder.First();

                bool canRemoveFromPendingList = false;
                lock (this.mainLock)
                {
                    canRemoveFromPendingList = this.pendingChunkContainersGenerated.ContainsKey(chunkContainerToGenerate.Key)
                        || this.WorldGenerator.OrderChunkGeneration(chunkWorldArea, chunkContainerToGenerate.Value);
                }

                if (canRemoveFromPendingList)
                {
                    this.pendingChunkContainersToOrder.Remove(chunkContainerToGenerate.Key);
                }
            }

            List<KeyValuePair<Vector2i, IChunk>> chunkContainersToAdd = null;
            lock (this.mainLock)
            {
                //foreach(Tuple<ChunkContainer, IChunk> chunkContainerGenerated in this.pendingChunkContainersGenerated)
                //{
                //    if (this.pendingChunkContainersToOrder.Contains(chunkContainerGenerated.Item1))
                //    {
                //        chunkContainersToAdd.Add(chunkContainerGenerated);
                //    }
                //}
                chunkContainersToAdd = this.pendingChunkContainersGenerated.ToList();

                this.pendingChunkContainersGenerated.Clear();
            }

            foreach (KeyValuePair<Vector2i, IChunk> chunkToAdd in chunkContainersToAdd)
            {
                this.pendingChunkContainersToOrder.Remove(chunkToAdd.Key);
                this.MainChunksMonitor.AddChunkToMonitor(chunkToAdd.Value);
            }
        }

        public void Dispose()
        {
            this.WorldGenerator.ChunkGenerated -= OnChunkGenerated;

            this.MainChunksMonitor.ChunksToLoad -= OnChunksToLoad;
            this.MainChunksMonitor.ChunksToAdd -= OnChunksAdded;

            this.MainChunksMonitor.ChunksRemoved -= OnChunksRemoved;
            this.MainChunksMonitor.ChunksToUnload -= OnChunksToUnload;
        }
    }
}
