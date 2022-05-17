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

        public override int ObjectChunkMargin
        {
            get
            {
                return 2;
            }
        }

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

            int newBiomeValue = LandCreationHelper.NeedToFillAt(this.AreaBuffer, localPosition.Y, localPosition.X, this.ObjectChunkMargin);

            zObjectCase.ObjectBiome = (BiomeType)newBiomeValue;//this.areaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin];
        }

        public static GroundLandObject CreateGroundLandObject(BiomeType biomeType, LandType landType)
        {
            switch (biomeType)
            {
                case BiomeType.TUNDRA:
                    return new TundraGroundLandObject(landType);
                case BiomeType.BOREAL_FOREST:
                    return new BorealForestGroundLandObject(landType);
                case BiomeType.SAVANNA:
                    return new SavannaGroundLandObject(landType);
                case BiomeType.TEMPERATE_FOREST:
                    return new TemperateForestGroundLandObject(landType);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return new TemperateRainForestGroundLandObject(landType);
                case BiomeType.DESERT:
                    return new DesertGroundLandObject(landType);
                case BiomeType.TROPICAL_WOODLAND:
                    return new TropicalWoodlandGroundLandObject(landType);
                case BiomeType.SEASONAL_FOREST:
                    return new SeasonalForestGroundLandObject(landType);
                case BiomeType.RAINFOREST:
                    return new RainForestGroundLandObject(landType);
            }
            return null;
        }
    }
}
