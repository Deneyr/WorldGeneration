using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.StructureNoise.TallGrassStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectChunkLayers;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TallGrassStructures
{
    internal class TallGrassObjectStructureTemplate : ADataObjectStructureTemplate
    {
        private AltitudeObjectChunkLayer altitudeObjectChunkLayer;

        public TallGrassObjectStructureTemplate() 
            : base("TallGrassStructure")
        {
        }

        protected override void UpdateZObjectCase(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IZObjectCase zObjectCase, IDataStructure dataStructure, int worldAltitude, IObjectStructure parentObjectStructure, int i, int j)
        {
            if (zObjectCase.GroundAltitude >= 0)
            {
                IDataStructureCase dataStructureCase = (dataStructure as ADataStructure).DataStructureCases[i, j];

                ObjectCase currentObjectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;

                if (dataStructureCase != null 
                    && currentObjectCase.Land.LandWater == null
                    && currentObjectCase.Land.LandWall == null
                    && (currentObjectCase.Land.LandOverGround == null || currentObjectCase.Land.LandOverGround is ATallGrassElementLandObject))
                {
                    ATallGrassElementLandObject tallGrassElement = CreateTallGrassElementLandObjectFrom(dataStructure.StructureBiome, dataStructure.StructureTypeIndex);

                    tallGrassElement.LandType = this.altitudeObjectChunkLayer.GetAltitudeLandType(dataStructure.StructureBiome, zObjectCase.GroundAltitude);
                    tallGrassElement.ParentStructureUID = parentObjectStructure.UID;

                    LandTransition dataCaseLandTransition = (dataStructureCase as TallGrassDataStructureCase).LandTransition;

                    if (currentObjectCase.Land.LandOverGround is ATallGrassElementLandObject)
                    {
                        tallGrassElement.LandTransition = this.GetTallGrassLandTransitionMix(currentObjectCase.Land.LandOverGround.LandTransition, dataCaseLandTransition);
                    }
                    else
                    {
                        tallGrassElement.LandTransition = dataCaseLandTransition;
                    }

                    currentObjectCase.Land.LandOverGround = tallGrassElement;
                }
            }
        }

        protected override IObjectStructure CreateObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, Random random, string structureUid, IDataStructure dataStructure, int worldAltitude)
        {
            this.altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);

            TallGrassObjectStructure newTallGrassStructure = new TallGrassObjectStructure(this.TemplateUID, structureUid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);

            newTallGrassStructure.StructureTypeIndex = dataStructure.StructureTypeIndex;
            newTallGrassStructure.BiomeType = dataStructure.StructureBiome;

            return newTallGrassStructure;
        }

        private LandTransition GetTallGrassLandTransitionMix(LandTransition backLandTransition, LandTransition frontLandTransition)
        {
            if(frontLandTransition == LandTransition.NONE
                || backLandTransition == LandTransition.NONE)
            {
                return LandTransition.NONE;
            }

            return LandTransitionHelper.UnionLandTransition(backLandTransition, frontLandTransition);
        }

        internal static ATallGrassElementLandObject CreateTallGrassElementLandObjectFrom(BiomeType biomeType, int landElementObjectId)
        {
            switch (biomeType)
            {
                case BiomeType.BOREAL_FOREST:
                    return new BorealForestTallGrassElementLandObject(landElementObjectId);
                case BiomeType.DESERT:
                    return new DesertTallGrassElementLandObject(landElementObjectId);
                case BiomeType.RAINFOREST:
                    return new RainForestTallGrassElementLandObject(landElementObjectId);
                case BiomeType.SAVANNA:
                    return new SavannaTallGrassElementLandObject(landElementObjectId);
                case BiomeType.SEASONAL_FOREST:
                    return new SeasonalForestTallGrassElementLandObject(landElementObjectId);
                case BiomeType.TEMPERATE_FOREST:
                    return new TemperateForestTallGrassElementLandObject(landElementObjectId);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return new TemperateRainForestTallGrassElementLandObject(landElementObjectId);
                case BiomeType.TROPICAL_WOODLAND:
                    return new TropicalWoodlandTallGrassElementLandObject(landElementObjectId);
                case BiomeType.TUNDRA:
                    return new TundraTallGrassElementLandObject(landElementObjectId);
            }
            return new TundraTallGrassElementLandObject(landElementObjectId);
        }
    }
}
