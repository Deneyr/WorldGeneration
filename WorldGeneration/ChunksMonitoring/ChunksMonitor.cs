using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ChunksMonitoring
{
    public class ChunksMonitor
    {
        protected Dictionary<Vector2i, ChunkContainer> chunksPoolDictionary;
        protected List<ChunkContainer> chunksPoolQueue;

        public int ChunksPoolLimit
        {
            get;
            private set;
        }

        public Dictionary<Vector2i, ChunkContainer> CurrentChunksLoaded
        {
            get;
            private set;
        }

        public List<List<ChunkContainer>> CurrentChunksArea
        {
            get;
            private set;
        }

        public IntRect CurrentArea
        {
            get;
            private set;
        }

        public event Action<List<ChunkContainer>> ChunksToLoad;
        public event Action<List<ChunkContainer>> ChunksToUnload;

        public event Action<List<ChunkContainer>> ChunksToAdd;
        public event Action<List<ChunkContainer>> ChunksRemoved;

        public ChunksMonitor(int chunksPoolLimit)
        {
            this.chunksPoolDictionary = new Dictionary<Vector2i, ChunkContainer>();
            this.chunksPoolQueue = new List<ChunkContainer>();
            this.ChunksPoolLimit = chunksPoolLimit;

            this.CurrentArea = new IntRect(0, 0, 0, 0);

            this.CurrentChunksLoaded = new Dictionary<Vector2i, ChunkContainer>();
            this.CurrentChunksArea = new List<List<ChunkContainer>>();
        }

        public void Reinitialize()
        {
            this.chunksPoolDictionary.Clear();
            this.chunksPoolQueue.Clear();

            this.CurrentArea = new IntRect(0, 0, 0, 0);

            this.CurrentChunksLoaded.Clear();
            this.CurrentChunksArea.Clear();
        }

        public void UpdateChunksArea(IntRect newArea)
        {
            if(newArea == this.CurrentArea)
            {
                return;
            }

            List<ChunkContainer> chunksToRemove = new List<ChunkContainer>();
            List<ChunkContainer> chunksToAdd = new List<ChunkContainer>();

            // Part Remove
            int topHeightToRemove = Math.Min(Math.Max(newArea.Top - this.CurrentArea.Top, 0), this.CurrentArea.Height);
            int botHeightToRemove = Math.Min(Math.Max((this.CurrentArea.Top + this.CurrentArea.Height) - (newArea.Top + newArea.Height), 0), this.CurrentArea.Height);

            for(int i = 0; i < topHeightToRemove; i++)
            {
                chunksToRemove.InsertRange(0, this.CurrentChunksArea.First());
                this.CurrentChunksArea.RemoveAt(0);
            }
            for (int i = 0; i < botHeightToRemove; i++)
            {
                chunksToRemove.InsertRange(0, this.CurrentChunksArea.Last());
                this.CurrentChunksArea.RemoveAt(this.CurrentChunksArea.Count - 1);
            }

            int columnCount = 0;
            if(this.CurrentChunksArea.Count > 0)
            {
                int leftWidthToRemove = Math.Min(Math.Max(newArea.Left - this.CurrentArea.Left, 0), this.CurrentArea.Width);
                int rightWidthToRemove = Math.Min(Math.Max((this.CurrentArea.Left + this.CurrentArea.Width) - (newArea.Left + newArea.Width), 0), this.CurrentArea.Width);

                if (leftWidthToRemove > 0)
                {
                    for (int i = 0; i < this.CurrentChunksArea.Count; i++)
                    {
                        for (int j = 0; j < leftWidthToRemove; j++)
                        {
                            chunksToRemove.Insert(0, this.CurrentChunksArea[i][j]);
                        }
                        this.CurrentChunksArea[i].RemoveRange(0, leftWidthToRemove);
                    }
                }

                columnCount = this.CurrentChunksArea[0].Count;
                if (rightWidthToRemove > 0)
                {
                    for (int i = 0; i < this.CurrentChunksArea.Count; i++)
                    {
                        for (int j = 0; j < rightWidthToRemove; j++)
                        {
                            chunksToRemove.Insert(0, this.CurrentChunksArea[i][columnCount - j - 1]);
                        }
                        this.CurrentChunksArea[i].RemoveRange(columnCount - rightWidthToRemove, rightWidthToRemove);
                    }
                }
            }

            // Part Add

            if (this.CurrentChunksArea.Count > 0)
            {
                int startTop = this.CurrentArea.Top + topHeightToRemove;
                int startRight = newArea.Left + newArea.Width - 1;

                int leftWidthToAdd = Math.Min(Math.Max(this.CurrentArea.Left - newArea.Left, 0), newArea.Width);
                int rightWidthToAdd = Math.Min(Math.Max((newArea.Left + newArea.Width) - (this.CurrentArea.Left + this.CurrentArea.Width), 0), newArea.Width);

                if (leftWidthToAdd > 0)
                {
                    for (int i = 0; i < this.CurrentChunksArea.Count; i++)
                    {
                        for (int j = leftWidthToAdd - 1; j >= 0; j--)
                        {
                            ChunkContainer newChunkContainer = new ChunkContainer(newArea.Left + j, startTop + i);
                            chunksToAdd.Add(newChunkContainer);
                            this.CurrentChunksArea[i].Insert(0, newChunkContainer);
                        }
                    }
                }

                columnCount = this.CurrentChunksArea[0].Count;
                if (rightWidthToAdd > 0)
                {
                    for (int i = 0; i < this.CurrentChunksArea.Count; i++)
                    {
                        for (int j = 0; j < rightWidthToAdd; j++)
                        {
                            ChunkContainer newChunkContainer = new ChunkContainer(startRight - j, startTop + i);
                            chunksToAdd.Add(newChunkContainer);
                            this.CurrentChunksArea[i].Insert(columnCount, newChunkContainer);
                        }
                    }
                }
            }

            int topHeightToAdd = Math.Min(Math.Max(this.CurrentArea.Top - newArea.Top, 0), newArea.Height);
            int botHeightToAdd = Math.Min(Math.Max((newArea.Top + newArea.Height) - (this.CurrentArea.Top + this.CurrentArea.Height), 0), newArea.Height);
            int startBot = newArea.Top + newArea.Height - botHeightToAdd;

            for (int i = topHeightToAdd - 1; i >= 0; i--)
            {
                List<ChunkContainer> newRowChunkContainer = new List<ChunkContainer>();
                for (int j = 0; j < newArea.Width; j++)
                {
                    ChunkContainer newChunkContainer = new ChunkContainer(newArea.Left + j, newArea.Top + i);
                    chunksToAdd.Add(newChunkContainer);
                    newRowChunkContainer.Add(newChunkContainer);
                }
                this.CurrentChunksArea.Insert(0, newRowChunkContainer);
            }

            for (int i = 0; i < botHeightToAdd; i++)
            {
                List<ChunkContainer> newRowChunkContainer = new List<ChunkContainer>();
                for (int j = 0; j < newArea.Width; j++)
                {
                    ChunkContainer newChunkContainer = new ChunkContainer(newArea.Left + j, startBot + i);
                    chunksToAdd.Add(newChunkContainer);
                    newRowChunkContainer.Add(newChunkContainer);
                }
                this.CurrentChunksArea.Add(newRowChunkContainer);
            }

            // Update Add from Pool
            List<ChunkContainer> chunksToLoad = new List<ChunkContainer>();
            List<ChunkContainer> realChunksToAdd = new List<ChunkContainer>();
            foreach (ChunkContainer chunkContainer in chunksToAdd)
            {
                if(this.CurrentChunksLoaded.TryGetValue(chunkContainer.Position, out ChunkContainer chunkContainerFromPool))
                {
                    if(chunkContainerFromPool.ContainedChunk != null)
                    {
                        chunkContainer.ContainedChunk = chunkContainerFromPool.ContainedChunk;
                        realChunksToAdd.Add(chunkContainer);
                    }
                    else
                    {
                        chunksToLoad.Add(chunkContainer);
                    }

                    this.CurrentChunksLoaded[chunkContainer.Position] = chunkContainer;
                    this.chunksPoolQueue.Remove(chunkContainerFromPool);
                }
                else
                {
                    this.CurrentChunksLoaded.Add(chunkContainer.Position, chunkContainer);
                    chunksToLoad.Add(chunkContainer);
                }
            }

            this.CurrentArea = newArea;

            // Notify Remove
            this.ChunksRemoved?.Invoke(chunksToRemove);

            // Notify Unload
            foreach(ChunkContainer chunkContainerToRemove in chunksToRemove)
            {
                if(chunkContainerToRemove.ContainedChunk != null)
                {
                    this.chunksPoolQueue.Add(chunkContainerToRemove);
                }
                else
                {
                    this.CurrentChunksLoaded.Remove(chunkContainerToRemove.Position);
                }
            }
            //this.chunksPoolQueue.AddRange(chunksToRemove);
            int nbChunksToUnload = Math.Max(this.chunksPoolQueue.Count - this.ChunksPoolLimit, 0);
            List<ChunkContainer> chunksToUnload = new List<ChunkContainer>();
            for(int i = 0; i < nbChunksToUnload; i++)
            {
                ChunkContainer chunkContainerToUnload = this.chunksPoolQueue.First();
                chunksToUnload.Add(chunkContainerToUnload);
                this.CurrentChunksLoaded.Remove(chunkContainerToUnload.Position);
                this.chunksPoolQueue.RemoveAt(0);
            }

            this.ChunksToUnload?.Invoke(chunksToUnload);

            // Notify Load
            this.ChunksToLoad?.Invoke(chunksToLoad);

            // Notify Add
            this.ChunksToAdd?.Invoke(realChunksToAdd);
        }

        public bool AddChunkToMonitor(IChunk chunkGenerated)
        {
            if (this.CurrentChunksLoaded.TryGetValue(chunkGenerated.Position, out ChunkContainer chunkContainer))
            {
                chunkContainer.ContainedChunk = chunkGenerated;
                //IntRect chunkArea = new IntRect(chunkContainer.Position.X, chunkContainer.Position.Y, 1, 1);
                //if (this.CurrentArea.Intersects(chunkArea))
                //{
                //    this.ChunksToAdd?.Invoke(new List<ChunkContainer>() { chunkContainer });
                //}
                this.ChunksToAdd?.Invoke(new List<ChunkContainer>() { chunkContainer });

                return true;
            }
            return false;
        }

        public ChunkContainer GetChunkContainerAt(int x, int y)
        {
            int localX = x - this.CurrentArea.Left;
            int localY = y - this.CurrentArea.Top;

            if (localX >= 0
                    && localX < this.CurrentArea.Width
                    && localY >= 0
                    && localY < this.CurrentArea.Height)
            {
                return this.CurrentChunksArea[localY][localX];
            }
            return null;
        }
    }
}
