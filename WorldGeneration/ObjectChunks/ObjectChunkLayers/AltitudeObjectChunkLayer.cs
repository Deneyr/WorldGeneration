﻿using SFML.Graphics;
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
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            //int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            //Random random = new Random(chunkSeed);

            this.altitudeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator);
            this.riverDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["river"] as RiverDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);

            //int seaAltitude = altitudeDataAgreggator.SeaLevel;

            //for (int i = 0; i < objectChunk.NbCaseSide; i++)
            //{
            //    for (int j = 0; j < objectChunk.NbCaseSide; j++)
            //    {
            //        IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(j, i) as IZObjectCase;

            //        int altitude = altitudeDataAgreggator.GetAltitudeAtWorldCoordinates(zObjectCase.Position.X, zObjectCase.Position.Y, out bool isUnderSea);

            //        int riverDepth = 0;
            //        if (altitude < 22)
            //        {
            //            float riverRatio = riverDataAgreggator.GetRiverValueAtWorldCoordinates(zObjectCase.Position.X, zObjectCase.Position.Y);
            //            riverDepth = (int)Math.Ceiling(riverRatio * 4);
            //            riverDepth = Math.Min(Math.Max(altitude - seaAltitude + 1, 0), riverDepth);
            //        }

            //        altitude = altitude - riverDepth;

            //        ObjectCase objectCase = new ObjectCase(zObjectCase.Position, altitude);
            //        zObjectCase.SetCaseAt(objectCase, altitude);

            //        if (isUnderSea)
            //        {
            //            for (int a = altitude; a <= seaAltitude; a++)
            //            {
            //                if (zObjectCase[a] == null)
            //                {
            //                    objectCase = new ObjectCase(zObjectCase.Position, a);
            //                    zObjectCase.SetCaseAt(objectCase, a);
            //                }

            //                (zObjectCase[a] as ObjectCase).IsUnderSea = true;
            //            }
            //        }
            //        else if (riverDepth > 0)
            //        {
            //            if (altitude == seaAltitude)
            //            {
            //                riverDepth = 1;
            //            }

            //            for (int a = 0; a <= riverDepth; a++)
            //            {
            //                int newAltitude = altitude + a;
            //                if (zObjectCase[newAltitude] == null)
            //                {
            //                    objectCase = new ObjectCase(zObjectCase.Position, newAltitude);
            //                    zObjectCase.SetCaseAt(objectCase, newAltitude);
            //                }

            //                (zObjectCase[newAltitude] as ObjectCase).IsUnderSea = true;
            //            }
            //        }
            //    }
            //}
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

            altitude = altitude - riverDepth;

            this.AreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = altitude;
        }

        //protected override void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        //{

        //}

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            int computedAltitude = LandCreationHelper.NeedToFillAt(this.AreaBuffer, localPosition.Y, localPosition.X, this.ObjectChunkMargin);

            ObjectCase objectCase = new ObjectCase(zObjectCase.Position, computedAltitude);

            LandType landType = this.GetAltitudeLandType(zObjectCase.ObjectBiome, objectCase.Altitude);
            GroundLandObject groundLandObject = BiomeObjectChunkLayer.CreateGroundLandObject(zObjectCase.ObjectBiome, landType);
            objectCase.Land.AddLandGround(groundLandObject);

            zObjectCase.SetCaseAt(objectCase);
        }

        private LandType GetAltitudeLandType(BiomeType biomeType, int altitude)
        {
            altitude -= 16;
            if (altitude < -2)
            {
                return LandType.SEA_DEPTH;
            }
            else if (altitude < 1)
            {
                return LandType.SAND;
            }
            else if (altitude < 3)
            {
                return LandType.GRASS;
            }
            else if (altitude < 9)
            {
                return LandType.MONTAIN;
            }
            else
            {
                return LandType.SNOW;
            }
        }
    }
}
