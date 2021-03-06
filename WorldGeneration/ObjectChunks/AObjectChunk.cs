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

            typeof(TallGrassElementLandObject),
            typeof(MainTreeElementLandObject),
            typeof(SideTreeElementLandObject)
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
