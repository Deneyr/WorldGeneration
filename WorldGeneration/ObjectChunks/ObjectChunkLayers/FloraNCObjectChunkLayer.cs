using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.BiomeManager;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class FloraNCObjectChunkLayer: AObjectChunkLayer
    {
        private AltitudeObjectChunkLayer altitudeObjectChunkLayer;

        private FloraDataAgreggator floraDataAgreggator;

        public FloraNCObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);
            this.floraDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["flora"] as FloraDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            FloraRatioBiomeManager floraRatioManager = objectChunksMonitor.DataChunkMonitor.FloraRatioManager;
            IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            if (zObjectCase.GroundAltitude >= 0)
            {
                ObjectCase objectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;
                LandCase landCase = objectCase.Land;

                if (landCase.LandOverGround == null
                    && landCase.LandWall == null
                    && landCase.IsOnlyWater == false)
                {
                    if(this.floraDataAgreggator.IsThereFlowerAtWorldCoordinate(zObjectCase.Position.X, zObjectCase.Position.Y, floraRatioManager.GetVegetationRatioFromBiomeAltitude(zObjectCase.ObjectBiome, objectCase.Altitude)))
                    {
                        AFloraElementLandObject newFloraElementLandObject = ConstructFloraObjectFromBiomeType(zObjectCase.ObjectBiome, random.Next());

                        newFloraElementLandObject.LandType = this.altitudeObjectChunkLayer.GetAltitudeLandType(zObjectCase.ObjectBiome, objectCase.Altitude);

                        landCase.LandOverGround = newFloraElementLandObject;
                    }
                }
            }
        }

        protected static AFloraElementLandObject ConstructFloraObjectFromBiomeType(BiomeType biomeType, int landElementObjectId)
        {
            switch (biomeType)
            {
                case BiomeType.BOREAL_FOREST:
                    return new BorealForestFloraElementLandObject(landElementObjectId);
                case BiomeType.DESERT:
                    return new DesertFloraElementLandObject(landElementObjectId);
                case BiomeType.RAINFOREST:
                    return new RainForestFloraElementLandObject(landElementObjectId);
                case BiomeType.SAVANNA:
                    return new SavannaFloraElementLandObject(landElementObjectId);
                case BiomeType.SEASONAL_FOREST:
                    return new SeasonalForestFloraElementLandObject(landElementObjectId);
                case BiomeType.TEMPERATE_FOREST:
                    return new TemperateForestFloraElementLandObject(landElementObjectId);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return new TemperateRainForestFloraElementLandObject(landElementObjectId);
                case BiomeType.TROPICAL_WOODLAND:
                    return new TropicalWoodlandFloraElementLandObject(landElementObjectId);
                case BiomeType.TUNDRA:
                    return new TundraFloraElementLandObject(landElementObjectId);
            }
            return new TundraFloraElementLandObject(landElementObjectId);
        }
    }
}
