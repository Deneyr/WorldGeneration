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
using WorldGeneration.ObjectChunks.ObjectLands.BiomeGroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class BiomeObjectChunkLayer : A2PassObjectChunkLayer
    {
        private BiomeDataAgreggator biomeDataAgreggator;

        public override bool GenerateAllLevels
        {
            get
            {
                return false;
            }
        }

        public BiomeObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {          
            this.biomeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["biome"] as BiomeDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }

        protected override void ComputeBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            this.AreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = (int)this.biomeDataAgreggator.GetBiomeAtWorldCoordinates(worldPosition.X, worldPosition.Y, out float borderValue);
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            int newBiomeValue = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            zObjectCase.ObjectBiome = (BiomeType)newBiomeValue;//this.areaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin];
        }

        public static GroundLandObject CreateGroundLandObject(Random random, BiomeType biomeType, LandType landType)
        {
            int landObjectId = random.Next();
            switch (biomeType)
            {
                case BiomeType.TUNDRA:
                    return new TundraGroundLandObject(landObjectId, landType);
                case BiomeType.BOREAL_FOREST:
                    return new BorealForestGroundLandObject(landObjectId, landType);
                case BiomeType.SAVANNA:
                    return new SavannaGroundLandObject(landObjectId, landType);
                case BiomeType.TEMPERATE_FOREST:
                    return new TemperateForestGroundLandObject(landObjectId, landType);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return new TemperateRainForestGroundLandObject(landObjectId, landType);
                case BiomeType.DESERT:
                    return new DesertGroundLandObject(landObjectId, landType);
                case BiomeType.TROPICAL_WOODLAND:
                    return new TropicalWoodlandGroundLandObject(landObjectId, landType);
                case BiomeType.SEASONAL_FOREST:
                    return new SeasonalForestGroundLandObject(landObjectId, landType);
                case BiomeType.RAINFOREST:
                    return new RainForestGroundLandObject(landObjectId, landType);
            }
            return null;
        }
    }
}
