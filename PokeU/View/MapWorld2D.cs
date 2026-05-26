using MathNet.Numerics.Optimization.TrustRegion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokeU.View.ResourcesManager;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.ObjectChunks;
using WorldGeneration.WorldGenerating;

namespace PokeU.View
{
    public class MapWorld2D
    {
        private static string MAP_DIRECTORY = "Map";

        private ChunksMonitor mapChunksMonitor;

        private LandWorld2D landWorld2D;

        private Dictionary<LandChunk2D, MapChunk2D> mapChunksDictionary;
        private Dictionary<Vector2i, MapChunk2D> coordinateToMapChunk;

        private int currentAltitude;

        private Viewport currentViewport;

        private Camera2D mainCamera;

        public int CurrentZoom
        {
            get
            {
                return this.mainCamera.Zoom;
            }
            set
            {
                if (this.mainCamera.Zoom != value)
                {
                    this.mainCamera.Zoom = value;
                }
            }
        }

        public Viewport CurrentViewport
        {
            get
            {
                return this.currentViewport;
            }
            set
            {
                if (this.currentViewport.Equals(value) == false)
                {
                    this.currentViewport = value;
                    this.mainCamera.ViewSize = new Vector2(this.currentViewport.Width, this.currentViewport.Height);
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.mainCamera.Position;
            }
            set
            {
                this.mainCamera.Position = value;
            }
        }

        public float Rotation
        {
            get
            {
                return this.mainCamera.Rotation;
            }
            set
            {
                if (this.mainCamera.Rotation != value)
                {
                    this.mainCamera.Rotation = value;
                }
            }
        }

        public int CurrentAltitude
        {
            get
            {
                return this.currentAltitude;
            }
            set
            {
                if (this.currentAltitude != value)
                {
                    this.currentAltitude = value;
                }
            }
        }

        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        public Texture2D DefaultTexture
        {
            get;
            private set;
        }

        public float ResolutionScale
        {
            get;
            private set;
        }

        public MapWorld2D(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, LandWorld2D landWorld2D, Viewport viewport, float resolutionScale = 1)
        {
            if(Directory.Exists("Map") == false)
            {
                Directory.CreateDirectory("Map");
            }

            this.mapChunksMonitor = new ChunksMonitor(0);

            this.ResolutionScale = resolutionScale;

            this.GraphicsDevice = graphicsDevice;
            this.SpriteBatch = spriteBatch;

            this.mapChunksDictionary = new Dictionary<LandChunk2D, MapChunk2D>();
            this.coordinateToMapChunk = new Dictionary<Vector2i, MapChunk2D>();

            this.currentAltitude = 16;
            this.landWorld2D = landWorld2D;

            this.mainCamera = new Camera2D();
            this.CurrentViewport = viewport;
            this.Position = landWorld2D.Position;

            this.CurrentZoom = -15;

            this.mainCamera.UpdateTransform();
            this.mainCamera.UpdateViewBound();

            this.DefaultTexture = this.CreateDefaultTexture(graphicsDevice);

            this.landWorld2D.PositionUpdated += LandWorld2D_PositionUpdated;
            this.landWorld2D.LandChunk2DAdded += LandWorld2D_LandChunk2DAdded;
            this.landWorld2D.LandChunk2DRemoved += LandWorld2D_LandChunk2DRemoved;

            this.mapChunksMonitor.ChunksToLoad += MapChunksMonitor_ChunksToLoad;
            this.mapChunksMonitor.ChunksToUnload += MapChunksMonitor_ChunksToUnload;
        }

        private Texture2D CreateDefaultTexture(GraphicsDevice graphicsDevice)
        {
            int chunkSize = MainGame.CHUNK_CASE_SIDE * MainGame.MODEL_TO_VIEW;

            Texture2D texture = new Texture2D(graphicsDevice, chunkSize, chunkSize);
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[chunkSize * chunkSize];

            Array.Fill(colors, Microsoft.Xna.Framework.Color.RoyalBlue);

            texture.SetData(colors);
            return texture;
        }

        private void LandWorld2D_PositionUpdated(Vector2 newPosition)
        {
            this.Position = newPosition;
        }

        public void UpdateWorld2D(GameTime deltaTime)
        {
            this.mainCamera.UpdateTransform();
            this.mainCamera.UpdateViewBound();

            this.UpdateWorldArea();
        }

        private void UpdateWorldArea()
        {
            FloatRect viewBound = this.mainCamera.ViewBound;
            IntRect worldViewArea = LandWorld2D.ViewAreaToWorldArea(viewBound);
            IntRect chunkArea = ChunkHelper.GetChunkAreaFromWorldArea(MainGame.CHUNK_CASE_SIDE, worldViewArea);
            this.mapChunksMonitor.UpdateChunksArea(chunkArea);
        }

        public void DrawIn(Game mainGame, SpriteBatch spriteBatch)
        {
            FloatRect viewBound = this.mainCamera.ViewBound;

            //Texture2D pixelTexture = new Texture2D(mainGame.GraphicsDevice, 1, 1);
            //pixelTexture.SetData(new Microsoft.Xna.Framework.Color[] { Microsoft.Xna.Framework.Color.White });
            //this.mainCamera.Zoom -= 4;
            //this.mainCamera.UpdateTransform();

            mainGame.GraphicsDevice.Viewport = this.CurrentViewport;
            spriteBatch.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp, transformMatrix: this.mainCamera.Transform);

            foreach (List<ChunkContainer> chunkContainerRow in this.mapChunksMonitor.CurrentChunksArea)
            {
                foreach (ChunkContainer chunkContainer in chunkContainerRow)
                {
                    if (chunkContainer.ContainedChunk != null)
                    {
                        MapChunk2D mapChunk2D = (MapChunk2D)chunkContainer.ContainedChunk;
                        //if (mapChunk2D.ViewBound.Intersects(viewBound))
                        //{
                        mapChunk2D.DrawIn(spriteBatch, ref viewBound);
                        //spriteBatch.Draw(
                        //    pixelTexture,
                        //    new Rectangle((int)mapChunk2D.ViewBound.Left, (int)mapChunk2D.ViewBound.Top, (int)mapChunk2D.ViewBound.Width, (int)mapChunk2D.ViewBound.Height),
                        //    new Microsoft.Xna.Framework.Color(0, 255, 0, 100));
                        //}
                        //else
                        //{
                        //    spriteBatch.Draw(
                        //        pixelTexture,
                        //        new Rectangle((int)mapChunk2D.ViewBound.Left, (int)mapChunk2D.ViewBound.Top, (int)mapChunk2D.ViewBound.Width, (int)mapChunk2D.ViewBound.Height),
                        //        new Microsoft.Xna.Framework.Color(0, 0, 255, 100));
                        //}
                    }
                }
            }

            //spriteBatch.Draw(
            //    pixelTexture,
            //    new Rectangle((int)viewBound.Left, (int)viewBound.Top, (int)viewBound.Width, (int)viewBound.Height),
            //    new Microsoft.Xna.Framework.Color(255, 0, 0, 100));

            spriteBatch.End();

            //this.mainCamera.Zoom += 4;
            //this.mainCamera.UpdateTransform();
        }

        private void MapChunksMonitor_ChunksToLoad(List<ChunkContainer> chunkContainers)
        {
            foreach (ChunkContainer chunkContainerToLoad in chunkContainers)
            {
                MapChunk2D mapChunk2D = null;
                if (this.TryGetMapChunk2DFromAdded(chunkContainerToLoad.Position, out mapChunk2D) == false)
                {
                    int chunkSize = MainGame.CHUNK_CASE_SIDE * MainGame.MODEL_TO_VIEW;
                    mapChunk2D = new MapChunk2D(this, chunkContainerToLoad.Position, chunkSize, chunkSize);
                }

                if(mapChunk2D.Texture.Item1 == null)
                {
                    _ = mapChunk2D.LoadTextureAsync(MAP_DIRECTORY);
                }

                chunkContainerToLoad.ContainedChunk = mapChunk2D;
            }
        }

        private void MapChunksMonitor_ChunksToUnload(List<ChunkContainer> chunkContainers)
        {
            foreach (ChunkContainer chunkContainerToUnload in chunkContainers)
            {
                if (chunkContainerToUnload.ContainedChunk != null)
                {
                    if (this.TryGetMapChunk2DFromAdded(chunkContainerToUnload.Position, out _) == false)
                    {
                        MapChunk2D mapChunk2DToUnload = (MapChunk2D)chunkContainerToUnload.ContainedChunk;
                        mapChunk2DToUnload.Dispose();
                    }
                }
            }
        }

        private void LandWorld2D_LandChunk2DAdded(IObjectChunk objectChunkAdded, LandChunk2D landChunk2DAdded)
        {
            MapChunk2D mapChunk2D = null;
            bool needRenderChunk = true;
            if (this.TryGetMapChunk2DFromMonitor(objectChunkAdded.Position, out mapChunk2D) == false)
            {
                int chunkSize = MainGame.CHUNK_CASE_SIDE * MainGame.MODEL_TO_VIEW;
                mapChunk2D = new MapChunk2D(this, objectChunkAdded.Position, chunkSize, chunkSize);
            }
            else
            {
                needRenderChunk = mapChunk2D.DoesTextureFileExist(MAP_DIRECTORY) == false;
            }

            mapChunk2D.LandChunk2D = landChunk2DAdded;

            if (mapChunk2D.Texture.Item1 == null)
            {
                if (needRenderChunk)
                {
                    mapChunk2D.Render();
                    _ = mapChunk2D.SaveTextureAsync(MAP_DIRECTORY);
                }
                else
                {
                    _ = mapChunk2D.LoadTextureAsync(MAP_DIRECTORY);
                }
            }

            this.mapChunksDictionary.Add(landChunk2DAdded, mapChunk2D);
            this.coordinateToMapChunk.Add(objectChunkAdded.Position, mapChunk2D);
        }

        private void LandWorld2D_LandChunk2DRemoved(IObjectChunk objectChunkRemoved, LandChunk2D landChunk2DRemoved)
        {
            if(this.mapChunksDictionary.TryGetValue(landChunk2DRemoved, out MapChunk2D mapChunk2DToRemove))
            {
                this.mapChunksDictionary.Remove(landChunk2DRemoved);
                this.coordinateToMapChunk.Remove(objectChunkRemoved.Position);

                mapChunk2DToRemove.LandChunk2D = null;
                if (this.TryGetMapChunk2DFromMonitor(objectChunkRemoved.Position, out _) == false)
                {
                    mapChunk2DToRemove.Dispose();
                }
            }
        }

        private bool TryGetMapChunk2DFromMonitor(Vector2i chunkCoordinate, out MapChunk2D mapChunk2D)
        {
            mapChunk2D = null;
            if (this.mapChunksMonitor.CurrentChunksLoaded.TryGetValue(chunkCoordinate, out ChunkContainer chunkContainer))
            {
                mapChunk2D = (MapChunk2D)chunkContainer.ContainedChunk;
                return true;
            }
            return false;
        }

        private bool TryGetMapChunk2DFromAdded(Vector2i chunkCoordinate, out MapChunk2D mapChunk2D)
        {
            return this.coordinateToMapChunk.TryGetValue(chunkCoordinate, out mapChunk2D);
        }

        public void Dispose()
        {
            this.mapChunksMonitor.ChunksToLoad -= MapChunksMonitor_ChunksToLoad;
            this.mapChunksMonitor.ChunksToUnload -= MapChunksMonitor_ChunksToUnload;

            this.landWorld2D.PositionUpdated -= LandWorld2D_PositionUpdated;
            this.landWorld2D.LandChunk2DAdded -= LandWorld2D_LandChunk2DAdded;
            this.landWorld2D.LandChunk2DRemoved -= LandWorld2D_LandChunk2DRemoved;
        }
    }
}
