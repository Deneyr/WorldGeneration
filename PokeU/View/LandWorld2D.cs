using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokeU.View.BiomeGroundObject;
using PokeU.View.BiomeGroundObject.TownGroundObject;
using PokeU.View.ElementLandObject;
using PokeU.View.ResourcesManager;
using PokeU.View.WaterObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.BiomeGroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree;
using WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;
using WorldGeneration.WorldGenerating;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace PokeU.View
{
    public class LandWorld2D
    {
        public static readonly int LOADED_ALTITUDE_RANGE = 32;

        public static readonly Dictionary<Type, IObject2DFactory> MappingObjectModelView;

        public static TextureManager TextureManager;

        private Dictionary<IObjectChunk, LandChunk2D> landChunksDictionary;

        private ChunkResourcesLoader chunkResourcesLoader;

        private int currentAltitude;

        // TEST
        private TestAutoDriver testAutoDriver;
        //

        private WorldMonitor landWorld;

        private Viewport currentViewport;

        private Camera2D mainCamera;

        public event Action<Vector2> PositionUpdated;

        public event Action<IObjectChunk, LandChunk2D> LandChunk2DAdded;

        public event Action<IObjectChunk, LandChunk2D> LandChunk2DRemoved;

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
                Vector2 snappedPosition = this.SnapPosition(value);
                if (this.mainCamera.Position != snappedPosition)
                {
                    this.mainCamera.Position = snappedPosition;

                    this.PositionUpdated?.Invoke(this.mainCamera.Position);
                }
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

        public ChunkResourcesLoader ResourcesLoader
        {
            get
            {
                return this.chunkResourcesLoader;
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

                    foreach (KeyValuePair<IObjectChunk, LandChunk2D> landChunkPair in this.landChunksDictionary)
                    {
                        landChunkPair.Value.CurrentAltitude = this.currentAltitude;
                    }
                }
            }
        }

        static LandWorld2D()
        {
            MappingObjectModelView = new Dictionary<Type, IObject2DFactory>();

            // Land Objects (ground objects and town ground objects)

            MappingObjectModelView.Add(typeof(BorealForestGroundLandObject), new BorealForestGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(DesertGroundLandObject), new DesertGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(RainForestGroundLandObject), new RainGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(SavannaGroundLandObject), new DryGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(SeasonalForestGroundLandObject), new SeasonalGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TemperateForestGroundLandObject), new TemperateGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TemperateRainForestGroundLandObject), new RainGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TropicalWoodlandGroundLandObject), new SeasonalGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TundraGroundLandObject), new SnowGroundObject2DFactory());

            MappingObjectModelView.Add(typeof(BorealForestTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(DesertTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(RainForestTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(SavannaTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(SeasonalForestTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TemperateForestTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TemperateRainForestTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TropicalWoodlandTownGroundLandObject), new TownGroundObject2DFactory());
            MappingObjectModelView.Add(typeof(TundraTownGroundLandObject), new TownGroundObject2DFactory());

            // Water Objects
            MappingObjectModelView.Add(typeof(WaterLandObject), new WaterObject2DFactory());

            // Element Land Objects
            // Tall Grass
            MappingObjectModelView.Add(typeof(BorealForestTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.BOREAL_FOREST));
            MappingObjectModelView.Add(typeof(DesertTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.DESERT));
            MappingObjectModelView.Add(typeof(RainForestTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.RAINFOREST));
            MappingObjectModelView.Add(typeof(SavannaTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.SAVANNA));
            MappingObjectModelView.Add(typeof(SeasonalForestTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.SEASONAL_FOREST));
            MappingObjectModelView.Add(typeof(TemperateForestTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.TEMPERATE_FOREST));
            MappingObjectModelView.Add(typeof(TemperateRainForestTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.TEMPERATE_RAINFOREST));
            MappingObjectModelView.Add(typeof(TropicalWoodlandTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.TROPICAL_WOODLAND));
            MappingObjectModelView.Add(typeof(TundraTallGrassElementLandObject), new TallGrassObject2DFactory(BiomeType.TUNDRA));

            // Tree
            MappingObjectModelView.Add(typeof(BorealForestMainTreeElementLandObject), new TreeObject2DFactory(BiomeType.BOREAL_FOREST));
            MappingObjectModelView.Add(typeof(BorealForestSideTreeElementLandObject), new TreeObject2DFactory(BiomeType.BOREAL_FOREST));
            MappingObjectModelView.Add(typeof(DesertMainTreeElementLandObject), new TreeObject2DFactory(BiomeType.DESERT));
            MappingObjectModelView.Add(typeof(DesertSideTreeElementLandObject), new TreeObject2DFactory(BiomeType.DESERT));
            MappingObjectModelView.Add(typeof(RainForestMainTreeElementLandObject), new TreeObject2DFactory(BiomeType.RAINFOREST));
            MappingObjectModelView.Add(typeof(RainForestSideTreeElementLandObject), new TreeObject2DFactory(BiomeType.RAINFOREST));
            MappingObjectModelView.Add(typeof(SavannaMainTreeElementLandObject), new TreeObject2DFactory(BiomeType.SAVANNA));
            MappingObjectModelView.Add(typeof(SavannaSideTreeElementLandObject), new TreeObject2DFactory(BiomeType.SAVANNA));
            MappingObjectModelView.Add(typeof(SeasonalMainTreeElementLandObject), new TreeObject2DFactory(BiomeType.SEASONAL_FOREST));
            MappingObjectModelView.Add(typeof(SeasonalSideTreeElementLandObject), new TreeObject2DFactory(BiomeType.SEASONAL_FOREST));
            MappingObjectModelView.Add(typeof(TemperateForestMainTreeElementObject), new TreeObject2DFactory(BiomeType.TEMPERATE_FOREST));
            MappingObjectModelView.Add(typeof(TemperateForestSideTreeElementObject), new TreeObject2DFactory(BiomeType.TEMPERATE_FOREST));
            MappingObjectModelView.Add(typeof(TemperateRainForestMainTreeElementObject), new TreeObject2DFactory(BiomeType.TEMPERATE_RAINFOREST));
            MappingObjectModelView.Add(typeof(TemperateRainForestSideTreeElementObject), new TreeObject2DFactory(BiomeType.TEMPERATE_RAINFOREST));
            MappingObjectModelView.Add(typeof(TropicalWoodlandMainTreeElementObject), new TreeObject2DFactory(BiomeType.TROPICAL_WOODLAND));
            MappingObjectModelView.Add(typeof(TropicalWoodlandSideTreeElementObject), new TreeObject2DFactory(BiomeType.TROPICAL_WOODLAND));
            MappingObjectModelView.Add(typeof(TundraMainTreeElementObject), new TreeObject2DFactory(BiomeType.TUNDRA));
            MappingObjectModelView.Add(typeof(TundraSideTreeElementObject), new TreeObject2DFactory(BiomeType.TUNDRA));

            // Flora
            MappingObjectModelView.Add(typeof(BorealForestFloraElementLandObject), new FloraObject2DFactory(BiomeType.BOREAL_FOREST));
            MappingObjectModelView.Add(typeof(DesertFloraElementLandObject), new FloraObject2DFactory(BiomeType.DESERT));
            MappingObjectModelView.Add(typeof(RainForestFloraElementLandObject), new FloraObject2DFactory(BiomeType.RAINFOREST));
            MappingObjectModelView.Add(typeof(SavannaFloraElementLandObject), new FloraObject2DFactory(BiomeType.SAVANNA));
            MappingObjectModelView.Add(typeof(SeasonalForestFloraElementLandObject), new FloraObject2DFactory(BiomeType.SEASONAL_FOREST));
            MappingObjectModelView.Add(typeof(TemperateForestFloraElementLandObject), new FloraObject2DFactory(BiomeType.TEMPERATE_FOREST));
            MappingObjectModelView.Add(typeof(TemperateRainForestFloraElementLandObject), new FloraObject2DFactory(BiomeType.TEMPERATE_RAINFOREST));
            MappingObjectModelView.Add(typeof(TropicalWoodlandFloraElementLandObject), new FloraObject2DFactory(BiomeType.TROPICAL_WOODLAND));
            MappingObjectModelView.Add(typeof(TundraFloraElementLandObject), new FloraObject2DFactory(BiomeType.TUNDRA));

            // Entity objects

            //MappingObjectModelView.Add(typeof(PlayerEntity), new PlayerEntity2DFactory());

            MappingObjectModelView.Add(typeof(ObjectChunk), new LandChunk2DFactory());
            MappingObjectModelView.Add(typeof(LandCase), new LandCase2DFactory());
        }

        public LandWorld2D(WorldMonitor landWorld, Viewport viewPort)
        {
            this.landChunksDictionary = new Dictionary<IObjectChunk, LandChunk2D>();
            this.chunkResourcesLoader = new ChunkResourcesLoader();

            this.currentAltitude = 16;
            this.landWorld = landWorld;

            this.mainCamera = new Camera2D();
            this.CurrentViewport = viewPort;
            //this.Position = new Vector2f(-150000, 20000);
            this.Position = new Vector2(-14754, 14077);
            //this.Position = new Vector2f(-74 * 16 * 32, 337 * 16 * 32);

            this.CurrentZoom = 0;

            this.mainCamera.UpdateTransform();
            this.mainCamera.UpdateViewBound();

            this.UpdateWorldArea();

            // TEST
            this.testAutoDriver = new TestAutoDriver(new Vector2f(this.Position.X, this.Position.Y), 200);
            //

            this.RegisterFactoryEvents();

            this.landWorld.MainChunksMonitor.ChunksToAdd += OnChunkAdded;
            this.landWorld.MainChunksMonitor.ChunksRemoved += OnChunkRemoved;

            //this.entity2DManager = new Entity2DManager(this);
            //landWorld.EntityManager.EntityAdded += this.entity2DManager.OnEntityAdded;
            //landWorld.EntityManager.EntityRemoved += this.entity2DManager.OnEntityRemoved;
        }

        public void UpdateWorld2D(GameTime deltaTime)
        {
            this.UpdatePlayerPosition(deltaTime);

            this.mainCamera.UpdateTransform();
            this.mainCamera.UpdateViewBound();

            this.UpdateWorldArea();
        }

        public void DrawIn(Game mainGame, SpriteBatch spriteBatch)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            // TEST
            //this.Position = this.testAutoDriver.GetNextPosition(this.Position, deltaTime.AsSeconds());
            //

            FloatRect viewBound = this.mainCamera.ViewBound;

            //Texture2D pixelTexture = new Texture2D(mainGame.GraphicsDevice, 1, 1);
            //pixelTexture.SetData(new Microsoft.Xna.Framework.Color[] { Microsoft.Xna.Framework.Color.White });
            //this.mainCamera.Zoom -= 2;
            //this.mainCamera.UpdateTransform();

            mainGame.GraphicsDevice.Viewport = this.CurrentViewport;
            spriteBatch.Begin(blendState:BlendState.NonPremultiplied, samplerState:SamplerState.PointClamp, transformMatrix:this.mainCamera.Transform);

            foreach (LandChunk2D landChunk2D in this.landChunksDictionary.Values)
            {
                if (landChunk2D.ViewBound.Intersects(viewBound))
                {
                    landChunk2D.DrawIn(spriteBatch, ref viewBound);
                    //spriteBatch.Draw(
                    //    pixelTexture,
                    //    new Rectangle((int)landChunk2D.ViewBound.Left, (int)landChunk2D.ViewBound.Top, (int)landChunk2D.ViewBound.Width, (int)landChunk2D.ViewBound.Height),
                    //    new Microsoft.Xna.Framework.Color(0, 255, 0, 100));
                }
                //else
                //{
                //    spriteBatch.Draw(
                //        pixelTexture,
                //        new Rectangle((int)landChunk2D.ViewBound.Left, (int)landChunk2D.ViewBound.Top, (int)landChunk2D.ViewBound.Width, (int)landChunk2D.ViewBound.Height),
                //        new Microsoft.Xna.Framework.Color(0, 0, 255, 100));
                //}
            }

            //spriteBatch.Draw(
            //    pixelTexture,
            //    new Rectangle((int)viewBound.Left, (int)viewBound.Top, (int)viewBound.Width, (int)viewBound.Height),
            //    new Microsoft.Xna.Framework.Color(255, 0, 0, 100));

            spriteBatch.End();

            //this.mainCamera.Zoom += 2;
            //this.mainCamera.UpdateTransform();

            //this.entity2DManager.DrawIn(window, ref boundsView);

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        private void UpdatePlayerPosition(GameTime deltaTime)
        {
            float elapsedSeconds = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                this.Position = new Vector2(0, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                Vector2 position = this.Position;

                position.Y -= elapsedSeconds * 320;

                this.Position = position;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Vector2 position = this.Position;

                position.Y += elapsedSeconds * 320;

                this.Position = position;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                Vector2 position = this.Position;

                position.X -= elapsedSeconds * 320;

                this.Position = position;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Vector2 position = this.Position;

                position.X += elapsedSeconds * 320;

                this.Position = position;
            }
        }

        private Vector2 SnapPosition(Vector2 position)
        {
            //float snappedX = (float)Math.Floor(position.X / 2) * 2;
            //float snappedY = (float)Math.Floor(position.Y / 2) * 2;
            //this.mainCamera.Position = new Vector2((((int)this.Position.X) / 2) * 2, (((int)this.Position.Y) / 2) * 2);
            //float snappedX = (float)Math.Floor(position.X);
            //float snappedY = (float)Math.Floor(position.Y);
            return position;
        }

        private void UpdateWorldArea()
        {
            FloatRect viewBound = this.mainCamera.ViewBound;
            IntRect worldViewArea = ViewAreaToWorldArea(viewBound);
            this.landWorld.WorldArea = worldViewArea;
        }

        public static IntRect ViewAreaToWorldArea(FloatRect viewArea)
        {
            IntRect area = new IntRect((int)(viewArea.Left), (int)(viewArea.Top), (int)(viewArea.Width), (int)(viewArea.Height));
            area.Left /= MainGame.MODEL_TO_VIEW;
            area.Top /= MainGame.MODEL_TO_VIEW;
            area.Width /= MainGame.MODEL_TO_VIEW;
            area.Height /= MainGame.MODEL_TO_VIEW;

            return area;
        }

        public static FloatRect ViewAreaToWorldArea(IntRect viewArea)
        {
            FloatRect area = new FloatRect((int)(viewArea.Left), (int)(viewArea.Top), (int)(viewArea.Width), (int)(viewArea.Height));
            area.Left *= MainGame.MODEL_TO_VIEW;
            area.Top *= MainGame.MODEL_TO_VIEW;
            area.Width *= MainGame.MODEL_TO_VIEW;
            area.Height *= MainGame.MODEL_TO_VIEW;

            return area;
        }

        private void OnChunkAdded(List<ChunkContainer> objs)
        {
            foreach (ChunkContainer chunkContainer in objs)
            {
                IObjectChunk objectChunk = chunkContainer.ContainedChunk as IObjectChunk;

                this.chunkResourcesLoader.LoadChunkResources(objectChunk);

                IObject2DFactory landChunk2DFactory = LandWorld2D.MappingObjectModelView[objectChunk.GetType()];
                LandChunk2D landChunk2DToAdd = landChunk2DFactory.CreateObject2D(this, objectChunk, new Point(objectChunk.Position.X * MainGame.MODEL_TO_VIEW, objectChunk.Position.Y * MainGame.MODEL_TO_VIEW)) as LandChunk2D;
                this.landChunksDictionary.Add(objectChunk, landChunk2DToAdd);

                this.LandChunk2DAdded?.Invoke(objectChunk, landChunk2DToAdd);
            }
        }

        private void OnChunkRemoved(List<ChunkContainer> objs)
        {
            foreach (ChunkContainer chunkContainer in objs)
            {
                IObjectChunk objectChunk = chunkContainer.ContainedChunk as IObjectChunk;

                this.chunkResourcesLoader.UnloadChunkResources(objectChunk);

                LandChunk2D landChunk2DToRemove = this.landChunksDictionary[objectChunk];
                landChunk2DToRemove.Dispose();

                this.landChunksDictionary.Remove(objectChunk);

                this.LandChunk2DRemoved?.Invoke(objectChunk, landChunk2DToRemove);
            }
        }

        private void RegisterFactoryEvents()
        {
            foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            {
                TextureManager.TextureLoaded += factory.OnTextureLoaded;
                TextureManager.TextureUnloaded += factory.OnTextureUnloaded;
            }
        }

        private void UnregisterFactoryEvents()
        {
            foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            {
                TextureManager.TextureLoaded -= factory.OnTextureLoaded;
                TextureManager.TextureUnloaded -= factory.OnTextureUnloaded;
            }
        }

        public void Dispose()
        {
            this.landWorld.MainChunksMonitor.ChunksToAdd -= OnChunkAdded;
            this.landWorld.MainChunksMonitor.ChunksRemoved -= OnChunkRemoved;

            this.UnregisterFactoryEvents();
        }
    }
}
