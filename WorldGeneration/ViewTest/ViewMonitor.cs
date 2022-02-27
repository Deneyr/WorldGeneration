using SFML.Graphics;
using SFML.System;
using SFML.Window;
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

        private int currentZoom;

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

        public Vector2f Position
        {
            get;
            private set;
        }

        public ViewMonitor(View defaultView, WorldMonitor worldMonitor)
        {
            this.worldMonitor = worldMonitor;

            this.currentViewSize = defaultView.Size;
            this.Position = new Vector2f(0, 0);
            this.currentZoom = 1;

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
                if (this.viewChunkDisplayed.ContainsKey(chunkContainer.Position) == false)
                {
                    this.viewChunkDisplayed.Add(chunkContainer.Position, new ViewChunk(chunkContainer.ContainedChunk));
                }
                else
                {

                }
            }
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
            {
                Vector2f position = this.Position;

                position.Y -= deltaTime.AsSeconds() * 320;

                this.Position = position;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                Vector2f position = this.Position;

                position.Y += deltaTime.AsSeconds() * 320;

                this.Position = position;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
            {
                Vector2f position = this.Position;

                position.X -= deltaTime.AsSeconds() * 320;

                this.Position = position;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                Vector2f position = this.Position;

                position.X += deltaTime.AsSeconds() * 320;

                this.Position = position;
            }

            View newView = new View(this.Position, this.CurrentViewSize);
            newView.Zoom(this.currentZoom);

            FloatRect viewBound = new FloatRect(newView.Center.X - newView.Size.X / 2, newView.Center.Y - newView.Size.Y / 2, newView.Size.X, newView.Size.Y);
            IntRect worldViewArea = ViewAreaToWorldArea(viewBound);
            worldMonitor.WorldArea = worldViewArea;

            RectangleShape rectangleShape = new RectangleShape(newView.Size);
            rectangleShape.Origin = new Vector2f(newView.Size.X / 2, newView.Size.Y / 2);
            rectangleShape.Position = this.Position;
            rectangleShape.OutlineThickness = 5;
            rectangleShape.OutlineColor = Color.Red;
            rectangleShape.FillColor = Color.Transparent;

            //newView.Zoom(this.currentZoom + 1);

            window.SetView(newView);

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

            window.Draw(rectangleShape);

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Up)
            {
                this.currentZoom += 1;
            }
            else if(e.Code == Keyboard.Key.Down)
            {
                this.currentZoom -= 1;
            }
        }

        public static IntRect ViewAreaToWorldArea(FloatRect viewArea)
        {
            IntRect area = new IntRect((int)(viewArea.Left), (int)(viewArea.Top), (int)(viewArea.Width), (int)(viewArea.Height));
            area.Left /= 16;
            area.Top /= 16;
            area.Width /= 16;
            area.Height /= 16;

            return area;
        }

        public void Dispose()
        {
            this.worldMonitor.MainChunksMonitor.ChunksToAdd -= OnChunksToAdd;
            this.worldMonitor.MainChunksMonitor.ChunksToUnload -= ChunksToUnload;

            this.worldMonitor = null;
        }
    }
}
