﻿using SFML.Graphics;
using PokeU.View.GroundObject;
using PokeU.View.ResourcesManager;
using PokeU.View.WaterObject;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.ObjectChunks;
using WorldGeneration.WorldGenerating;
using WorldGeneration.ObjectChunks.ObjectLands.BiomeGroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;
using WorldGeneration.ObjectChunks.ObjectLands;
using PokeU.View.BiomeGroundObject;
using PokeU.View.ElementLandObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora;
using WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject;
using PokeU.View.BiomeGroundObject.TownGroundObject;

namespace PokeU.View
{
    public class LandWorld2D
    {
        public static readonly int LOADED_ALTITUDE_RANGE = 32;

        public static readonly Dictionary<Type, IObject2DFactory> MappingObjectModelView;

        public static readonly TextureManager TextureManager;

        private Dictionary<IObjectChunk, LandChunk2D> landChunksDictionary;

        private ChunkResourcesLoader chunkResourcesLoader;

        private int currentAltitude;


        // TEST
        private TestAutoDriver testAutoDriver;
        // 

        private WorldMonitor landWorld;

        private Vector2f currentViewSize;

        public int CurrentZoom
        {
            get;
            set;
        }

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
            TextureManager = new TextureManager();

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

            foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            {
                TextureManager.TextureLoaded += factory.OnTextureLoaded;
                TextureManager.TextureUnloaded += factory.OnTextureUnloaded;
            }
        }

        public LandWorld2D(WorldMonitor landWorld)
        {
            this.landChunksDictionary = new Dictionary<IObjectChunk, LandChunk2D>();

            this.chunkResourcesLoader = new ChunkResourcesLoader();

            //this.entity2DManager = new Entity2DManager(this);
            //landWorld.EntityManager.EntityAdded += this.entity2DManager.OnEntityAdded;
            //landWorld.EntityManager.EntityRemoved += this.entity2DManager.OnEntityRemoved;

            this.currentAltitude = 16;

            this.landWorld = landWorld;

            this.currentViewSize = new Vector2f(1920, 1080);
            //this.Position = new Vector2f(-150000, 20000);
            this.Position = new Vector2f(-122259, 55112);
            //this.Position = new Vector2f(-74 * 16 * 32, 337 * 16 * 32);

            // TEST
            this.testAutoDriver = new TestAutoDriver(this.Position, 200);
            //

            this.CurrentZoom = 1;

            landWorld.MainChunksMonitor.ChunksToAdd += OnChunkAdded;
            landWorld.MainChunksMonitor.ChunksRemoved += OnChunkRemoved;
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            // TEST
            //this.Position = this.testAutoDriver.GetNextPosition(this.Position, deltaTime.AsSeconds());
            //

            if (Keyboard.IsKeyPressed(Keyboard.Key.T))
            {
                this.Position = new Vector2f(0, 0);
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
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

            this.CurrentViewSize = new Vector2f(1920, 1080);
            SFML.Graphics.View newView = new SFML.Graphics.View(new Vector2f((((int)this.Position.X) / 2) * 2, (((int)this.Position.Y) / 2) * 2), this.CurrentViewSize);
            newView.Zoom(this.CurrentZoom);

            FloatRect viewBound = new FloatRect(newView.Center.X - newView.Size.X / 2, newView.Center.Y - newView.Size.Y / 2, newView.Size.X, newView.Size.Y);
            IntRect worldViewArea = ViewAreaToWorldArea(viewBound);
            this.landWorld.WorldArea = worldViewArea;

            //viewBound = new FloatRect(newView.Center.X - newView.Size.X / 4, newView.Center.Y - newView.Size.Y / 4, newView.Size.X / 2, newView.Size.Y / 2);
            //newView = new SFML.Graphics.View(new Vector2f((((int)this.Position.X) / 2) * 2, (((int)this.Position.Y) / 2) * 2), new Vector2f(viewBound.Width, viewBound.Height));

            window.SetView(newView);

            foreach (LandChunk2D landChunk2D in this.landChunksDictionary.Values)
            {
                FloatRect bounds = new FloatRect(landChunk2D.Position, new SFML.System.Vector2f(landChunk2D.Width, landChunk2D.Height));

                if (bounds.Intersects(viewBound))
                {
                    landChunk2D.DrawIn(window, ref viewBound);
                }
            }

            //this.entity2DManager.DrawIn(window, ref boundsView);

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        private void OnChunkAdded(List<ChunkContainer> objs)
        {
            foreach (ChunkContainer chunkContainer in objs)
            {
                IObjectChunk objectChunk = chunkContainer.ContainedChunk as IObjectChunk;

                this.chunkResourcesLoader.LoadChunkResources(objectChunk);

                IObject2DFactory landChunk2DFactory = LandWorld2D.MappingObjectModelView[objectChunk.GetType()];

                this.landChunksDictionary.Add(objectChunk, landChunk2DFactory.CreateObject2D(this, objectChunk, objectChunk.Position) as LandChunk2D);
            }
        }

        public static IntRect ViewAreaToWorldArea(FloatRect viewArea)
        {
            IntRect area = new IntRect((int)(viewArea.Left), (int)(viewArea.Top), (int)(viewArea.Width), (int)(viewArea.Height));
            area.Left /= MainWindow.MODEL_TO_VIEW;
            area.Top /= MainWindow.MODEL_TO_VIEW;
            area.Width /= MainWindow.MODEL_TO_VIEW;
            area.Height /= MainWindow.MODEL_TO_VIEW;

            return area;
        }

        public static FloatRect ViewAreaToWorldArea(IntRect viewArea)
        {
            FloatRect area = new FloatRect((int)(viewArea.Left), (int)(viewArea.Top), (int)(viewArea.Width), (int)(viewArea.Height));
            area.Left *= MainWindow.MODEL_TO_VIEW;
            area.Top *= MainWindow.MODEL_TO_VIEW;
            area.Width *= MainWindow.MODEL_TO_VIEW;
            area.Height *= MainWindow.MODEL_TO_VIEW;

            return area;
        }

        private void OnChunkRemoved(List<ChunkContainer> objs)
        {
            foreach (ChunkContainer chunkContainer in objs)
            {
                IObjectChunk objectChunk = chunkContainer.ContainedChunk as IObjectChunk;

                this.chunkResourcesLoader.UnloadChunkResources(objectChunk);

                this.landChunksDictionary[objectChunk].Dispose();

                this.landChunksDictionary.Remove(objectChunk);
            }
        }

        public void Dispose(WorldMonitor landWorld)
        {
            landWorld.MainChunksMonitor.ChunksToAdd -= OnChunkAdded;
            landWorld.MainChunksMonitor.ChunksRemoved -= OnChunkRemoved;
        }
    }
}
