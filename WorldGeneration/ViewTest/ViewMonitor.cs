using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration.ViewTest
{
    public class ViewMonitor: IDisposable
    {
        internal static readonly float MODEL_TO_VIEW = 16f;

        private WorldMonitor worldMonitor;

        Dictionary<Vector2i, ViewChunk> viewChunkDisplayed;

        private Vector2f currentViewSize;

        public Vector2f CurrentViewSize
        {
            get
            {
                return this.currentViewSize;
            }
            set
            {
                if (this.currentViewSize != value)
                {
                    this.currentViewSize = value;
                }
            }
        }

        public ViewMonitor(View defaultView, WorldMonitor worldMonitor)
        {
            this.worldMonitor = worldMonitor;

            this.currentViewSize = defaultView.Size;

            this.worldMonitor.MainChunksMonitor.ChunksToAdd += OnChunksToAdd;
            this.worldMonitor.MainChunksMonitor.ChunksToUnload += ChunksToUnload;

            this.viewChunkDisplayed = new Dictionary<Vector2i, ViewChunk>();
        }

        private void ChunksToUnload(List<ChunkContainer> obj)
        {
            foreach(ChunkContainer chunkContainer in obj)
            {
                this.viewChunkDisplayed.Remove(chunkContainer.Position);
            }
        }

        private void OnChunksToAdd(List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainer in obj)
            {
                this.viewChunkDisplayed.Add(chunkContainer.Position, new ViewChunk(chunkContainer.ContainedChunk));
            }
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            this.CurrentViewSize = window.DefaultView.Size;

            View defaultView = window.DefaultView;

            FloatRect viewBound = new FloatRect(defaultView.Center.X - defaultView.Size.X / 2, defaultView.Center.Y - defaultView.Size.Y / 2, defaultView.Size.X, defaultView.Size.Y);

            foreach (ViewChunk viewChunk in this.viewChunkDisplayed.Values)
            {
                //IntRect viewChunkBound = ChunkHelper.GetWorldAreaFromChunkArea(viewChunk.NbCaseSide, new IntRect(viewChunk.Position, new Vector2i(viewChunk.NbCaseSide, viewChunk.NbCaseSide)));
                //FloatRect realViewChunkBound = new FloatRect(
                //    viewChunkBound.Left * ViewMonitor.MODEL_TO_VIEW,
                //    viewChunkBound.Top * ViewMonitor.MODEL_TO_VIEW,
                //    viewChunkBound.Width * ViewMonitor.MODEL_TO_VIEW,
                //    viewChunkBound.Height * ViewMonitor.MODEL_TO_VIEW);
                //if (viewBound.Intersects(realViewChunkBound))
                //{
                    viewChunk.DrawIn(window, deltaTime);
                //}
            }

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        public void Dispose()
        {
            this.worldMonitor.MainChunksMonitor.ChunksToAdd -= OnChunksToAdd;
            this.worldMonitor.MainChunksMonitor.ChunksToUnload -= ChunksToUnload;

            this.worldMonitor = null;
        }
    }
}
