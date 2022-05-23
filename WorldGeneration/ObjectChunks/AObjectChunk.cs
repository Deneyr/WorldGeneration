using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.BiomeGroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;

namespace WorldGeneration.ObjectChunks
{
    public class AObjectChunk : IObjectChunk
    {
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
        };

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
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            return this.ZCasesArray[y, x];
        }
    }
}
