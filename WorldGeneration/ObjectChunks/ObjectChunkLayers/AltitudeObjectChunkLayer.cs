using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class AltitudeObjectChunkLayer : A2PassObjectChunkLayer
    {
        //public int[,] InitialAltitudeAreaBuffer
        //{
        //    get;
        //    private set;
        //}

        public override int ObjectChunkMargin
        {
            get
            {
                return 4;
            }
        }

        public int[,] WaterLevelAreaBuffer
        {
            get;
            private set;
        }

        private AltitudeDataAgreggator altitudeDataAgreggator;

        private RiverDataAgreggator riverDataAgreggator;

        public override bool GenerateAllLevels
        {
            get
            {
                return true;
            }
        }

        public AltitudeObjectChunkLayer(string id) 
            : base(id)
        {
            //this.InitialAltitudeAreaBuffer = null;
            this.WaterLevelAreaBuffer = null;
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            //int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            //Random random = new Random(chunkSeed);

            this.altitudeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator);
            this.riverDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["river"] as RiverDataAgreggator);

            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            for (int i = -this.ObjectChunkMargin; i < objectChunk.NbCaseSide + this.ObjectChunkMargin; i++)
            {
                for (int j = -this.ObjectChunkMargin; j < objectChunk.NbCaseSide + this.ObjectChunkMargin; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

                    this.ComputeBufferArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
                }
            }

            int secondObjectChunkMargin = this.ObjectChunkMargin - 1;
            for (int i = -secondObjectChunkMargin; i < objectChunk.NbCaseSide + secondObjectChunkMargin; i++)
            {
                for (int j = -secondObjectChunkMargin; j < objectChunk.NbCaseSide + secondObjectChunkMargin; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

                    this.ComputeSecondBufferArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
                }
            }
            //for (int i = -secondObjectChunkMargin; i < objectChunk.NbCaseSide + secondObjectChunkMargin; i++)
            //{
            //    for (int j = -secondObjectChunkMargin; j < objectChunk.NbCaseSide + secondObjectChunkMargin; j++)
            //    {
            //        Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

            //        this.ComputeSecondBufferArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
            //    }
            //}

            int transitionObjectChunkMargin = this.ObjectChunkMargin - 2;
            for (int i = -transitionObjectChunkMargin; i < objectChunk.NbCaseSide + transitionObjectChunkMargin; i++)
            {
                for (int j = -transitionObjectChunkMargin; j < objectChunk.NbCaseSide + transitionObjectChunkMargin; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

                    this.ComputeTransitionAreaBuffer(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
                }
            }

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    this.ComputeChunkArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), this.GetWorldPosition(objectChunk, j, i));
                }
            }
        }

        protected override void ComputeBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            int altitude = this.altitudeDataAgreggator.GetAltitudeAtWorldCoordinates(worldPosition.X, worldPosition.Y, out bool isUnderSea);

            int riverDepth = 0;
            if (altitude < 22)
            {
                float riverRatio = riverDataAgreggator.GetRiverValueAtWorldCoordinates(worldPosition.X, worldPosition.Y);
                riverDepth = (int)Math.Ceiling(riverRatio * 4);
                riverDepth = Math.Min(Math.Max(altitude - this.altitudeDataAgreggator.SeaLevel + 1, 0), riverDepth);
            }

            int waterAltitude = -1;
            if(riverDepth > 0)
            {
                waterAltitude = altitude - 1;
            }

            int seaDepth = -1;
            if (isUnderSea || riverDepth > 0)
            {
                seaDepth = this.altitudeDataAgreggator.SeaLevel;//waterAltitude = Math.Max(this.altitudeDataAgreggator.SeaLevel, altitude);
            }

            //int realRiverDepth = 0;
            //if(riverDepth > 0)
            //{
            //    realRiverDepth = riverDepth + 1;
            //}

            //this.InitialAltitudeAreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = altitude;
            this.WaterLevelAreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = Math.Max(waterAltitude, seaDepth);

            altitude = altitude - riverDepth;

            this.AreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = altitude;
        }

        protected override void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            int i = localPosition.Y + this.ObjectChunkMargin - 1;
            int j = localPosition.X + this.ObjectChunkMargin - 1;

            this.SecondAreaBuffer[i, j] = LandCreationHelper.NeedToFillAt(this.AreaBuffer, localPosition.Y, localPosition.X, this.ObjectChunkMargin);
        }

        //protected override void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        //{
        //    //base.ComputeSecondBufferArea(objectChunksMonitor, random, objectChunk, localPosition, worldPosition);

        //    //int waterLevel = this.GetWaterLevelAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
        //    //if (waterLevel >= 0)
        //    //{
        //    //    int initialAltitude = this.GetAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
        //    //    int newAltitude = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

        //    //    if (waterLevel > this.altitudeDataAgreggator.SeaLevel)
        //    //    {
        //    //        waterLevel = Math.Max(this.altitudeDataAgreggator.SeaLevel, waterLevel - (newAltitude - initialAltitude));

        //    //        this.WaterLevelAreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = waterLevel;
        //    //    }
        //    //}
        //    int i = localPosition.Y + this.ObjectChunkMargin - 1;
        //    int j = localPosition.X + this.ObjectChunkMargin - 1;

        //    this.SecondAreaBuffer[i, j] = this.GetAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
        //}

        //protected override void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        //{
        //    base.ComputeSecondBufferArea(objectChunksMonitor, random, objectChunk, localPosition, worldPosition);

        //    int waterLevel = this.GetWaterLevelAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
        //    if (waterLevel >= 0)
        //    {
        //        int initialAltitude = this.GetAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
        //        int newAltitude = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

        //        if (waterLevel > this.altitudeDataAgreggator.SeaLevel)
        //        {
        //            waterLevel = Math.Max(this.altitudeDataAgreggator.SeaLevel, waterLevel - (newAltitude - initialAltitude));

        //            this.WaterLevelAreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = waterLevel;
        //        }
        //    }
        //}

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            int computedAltitude = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            ObjectCase objectCase = new ObjectCase(zObjectCase.Position, computedAltitude);

            LandType landType = this.GetAltitudeLandType(zObjectCase.ObjectBiome, objectCase.Altitude);
            GroundLandObject groundLandObject = BiomeObjectChunkLayer.CreateGroundLandObject(random, zObjectCase.ObjectBiome, landType);
            objectCase.Land.AddLandGround(groundLandObject);

            zObjectCase.SetGroundCaseAt(objectCase);
        }

        public override void InitObjectChunkLayer(int nbCaseSide)
        {
            base.InitObjectChunkLayer(nbCaseSide);

            int caseSideExtended = nbCaseSide + this.ObjectChunkMargin * 2;
            //this.InitialAltitudeAreaBuffer = new int[caseSideExtended, caseSideExtended];
            this.WaterLevelAreaBuffer = new int[caseSideExtended, caseSideExtended];
        }

        //public int GetInitialAltitudeAreaBufferValueAtLocal(int x, int y)
        //{
        //    return this.InitialAltitudeAreaBuffer[y + this.ObjectChunkMargin, x + this.ObjectChunkMargin];
        //}

        public int GetWaterLevelAreaBufferValueAtLocal(int x, int y)
        {
            return this.WaterLevelAreaBuffer[y + this.ObjectChunkMargin, x + this.ObjectChunkMargin];
        }

        public LandType GetAltitudeLandType(BiomeType biomeType, int altitude)
        {
            altitude -= this.altitudeDataAgreggator.SeaLevel;
            if (altitude < -1)
            {
                return LandType.SEA_DEPTH;
            }
            else if (altitude < 1)
            {
                return LandType.SAND;
            }
            else if (altitude < 4)
            {
                return LandType.GRASS;
            }
            else if (altitude < 10)
            {
                return LandType.MOUNTAIN;
            }
            else
            {
                return LandType.SNOW;
            }
        }
    }
}
