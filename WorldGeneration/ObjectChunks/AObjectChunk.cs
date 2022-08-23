using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.BiomeGroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks
{
    public class AObjectChunk : IObjectChunk
    {
        private Dictionary<string, IObjectStructure> idToDataStructures;

        private static readonly HashSet<Type> ALL_TYPES_IN_CHUNK = new HashSet<Type>()
        {
            typeof(WaterLandObject),

            typeof(BorealForestGroundLandObject),
            typeof(DesertGroundLandObject),
            typeof(RainForestGroundLandObject),
            typeof(SavannaGroundLandObject),
            typeof(SeasonalForestGroundLandObject),
            typeof(TemperateForestGroundLandObject),
            typeof(TemperateRainForestGroundLandObject),
            typeof(TropicalWoodlandGroundLandObject),
            typeof(TundraGroundLandObject),

            // Tall Grass
            typeof(BorealForestTallGrassElementLandObject),
            typeof(DesertTallGrassElementLandObject),
            typeof(RainForestTallGrassElementLandObject),
            typeof(SavannaTallGrassElementLandObject),
            typeof(SeasonalForestTallGrassElementLandObject),
            typeof(TemperateForestTallGrassElementLandObject),
            typeof(TemperateRainForestTallGrassElementLandObject),
            typeof(TropicalWoodlandTallGrassElementLandObject),
            typeof(TundraTallGrassElementLandObject),

            // Tree
            typeof(BorealForestMainTreeElementLandObject),
            typeof(BorealForestSideTreeElementLandObject),
            typeof(DesertMainTreeElementLandObject),
            typeof(DesertSideTreeElementLandObject),
            typeof(RainForestMainTreeElementLandObject),
            typeof(RainForestSideTreeElementLandObject),
            typeof(SavannaMainTreeElementLandObject),
            typeof(SavannaSideTreeElementLandObject),
            typeof(SeasonalMainTreeElementLandObject),
            typeof(SeasonalSideTreeElementLandObject),
            typeof(TemperateForestMainTreeElementObject),
            typeof(TemperateForestSideTreeElementObject),
            typeof(TemperateRainForestMainTreeElementObject),
            typeof(TemperateRainForestSideTreeElementObject),
            typeof(TropicalWoodlandMainTreeElementObject),
            typeof(TropicalWoodlandSideTreeElementObject),
            typeof(TundraMainTreeElementObject),
            typeof(TundraSideTreeElementObject),

            // Flora
            typeof(BorealForestFloraElementLandObject),
            typeof(DesertFloraElementLandObject),
            typeof(RainForestFloraElementLandObject),
            typeof(SavannaFloraElementLandObject),
            typeof(SeasonalForestFloraElementLandObject),
            typeof(TemperateForestFloraElementLandObject),
            typeof(TemperateRainForestFloraElementLandObject),
            typeof(TropicalWoodlandFloraElementLandObject),
            typeof(TundraFloraElementLandObject),
        };

        internal ObjectChunkLayersMonitor ParentMonitor
        {
            get;
            set;
        }

        public IZObjectCase[,] ZCasesArray
        {
            get;
            protected set;
        }

        public Vector2i Position
        {
            get;
            private set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        public HashSet<Type> TypesInChunk
        {
            get
            {
                return ALL_TYPES_IN_CHUNK;
            }
        }

        public AObjectChunk(Vector2i position, int nbCaseSide)
        {
            this.Position = position;
            this.NbCaseSide = nbCaseSide;
            this.idToDataStructures = new Dictionary<string, IObjectStructure>();
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            return this.ZCasesArray[y, x];
        }

        public void RegisterObjectStructure(IObjectStructure objectStructureToRegister)
        {
            this.idToDataStructures[objectStructureToRegister.UID] = objectStructureToRegister;
        }

        public IObjectStructure GetObjectStructure(string uid)
        {
            if(this.idToDataStructures.TryGetValue(uid, out IObjectStructure objectStructure))
            {
                return objectStructure;
            }
            return null;
        }
    }
}
