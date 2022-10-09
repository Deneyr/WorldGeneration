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
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

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

                TallGrassDataStructure tallGrassDataStructure = dataStructure as TallGrassDataStructure;

                if (this.IsCaseValid(tallGrassDataStructure, dataStructureCase, currentObjectCase))
                {
                    ATallGrassElementLandObject tallGrassElement = CreateTallGrassElementLandObjectFrom(dataStructure.StructureBiome, dataStructure.StructureTypeIndex);

                    tallGrassElement.LandType = this.altitudeObjectChunkLayer.GetAltitudeLandType(dataStructure.StructureBiome, zObjectCase.GroundAltitude);
                    tallGrassElement.ParentStructureUID = parentObjectStructure.UID;

                    LandTransition dataCaseLandTransition = (dataStructureCase as TallGrassDataStructureCase).LandTransition;

                    if (currentObjectCase.Land.LandOverGround is ATallGrassElementLandObject)
                    {
                        tallGrassElement.LandTransition = this.GetTallGrassLandTransitionMix(currentObjectCase.Land.LandOverGround.LandTransition, dataCaseLandTransition);
                    }
                    else if (currentObjectCase.Land.LandOverWall is ATallGrassElementLandObject)
                    {
                        tallGrassElement.LandTransition = this.GetTallGrassLandTransitionMix(currentObjectCase.Land.LandOverWall.LandTransition, dataCaseLandTransition);
                    }
                    else
                    {
                        tallGrassElement.LandTransition = dataCaseLandTransition;
                    }

                    bool isLandOverWallValid = true;
                    if ((currentObjectCase.Land.LandWall != null && currentObjectCase.Land.LandWall is GroundLandObject)
                        || currentObjectCase.Land.LandWater != null)
                    {
                        LandTransition wallWaterTransition = this.CreateWallWaterTransition(currentObjectCase.Land.LandWall, currentObjectCase.Land.LandWater);

                        tallGrassElement.LandTransition = LandTransitionHelper.IntersectionLandTransition(tallGrassElement.LandTransition, wallWaterTransition);

                        isLandOverWallValid = tallGrassElement.LandTransition != LandTransition.NONE;
                    }

                    if (isLandOverWallValid)
                    {
                        if (currentObjectCase.Land.LandWall == null)
                        {
                            currentObjectCase.Land.LandOverGround = tallGrassElement;
                        }
                        else
                        {
                            currentObjectCase.Land.LandOverWall = tallGrassElement;
                        }
                    }
                }
            }
        }

        protected override IObjectStructure CreateObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, Random random, string structureUid, IDataStructure dataStructure, int worldAltitude)
        {
            this.altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);

            TallGrassObjectStructure newTallGrassStructure = new TallGrassObjectStructure(this.TemplateUID, structureUid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);

            newTallGrassStructure.StructureTypeIndex = dataStructure.StructureTypeIndex;
            newTallGrassStructure.BiomeType = dataStructure.StructureBiome;

            newTallGrassStructure.IsFullPatch = (dataStructure as TallGrassDataStructure).IsFullPatch;

            return newTallGrassStructure;
        }

        private bool IsCaseValid(TallGrassDataStructure parentTallGrassDataStructure, IDataStructureCase dataStructureCase, ObjectCase currentObjectCase)
        {
            if(dataStructureCase == null)
            {
                return false;
            }

            if (parentTallGrassDataStructure.IsFullPatch)
            {
                if (currentObjectCase.Land.LandWater != null
                    && (currentObjectCase.Land.LandWall == null && currentObjectCase.Land.LandWater.LandTransition == LandTransition.NONE))
                {
                    return false;
                }
            }
            else
            {
                if (currentObjectCase.Land.LandWater != null)
                {
                    return false;
                }

                if (currentObjectCase.Land.LandWall != null)
                {
                    return false;
                }

                if(currentObjectCase.Land.LandOverGround != null 
                    && currentObjectCase.Land.LandOverGround is ATallGrassElementLandObject == false)
                {
                    return false;
                }
            }

            return true;
        }

        private LandTransition CreateWallWaterTransition(ILandWall landWall, ILandWater landWater)
        {
            if(landWall != null
                && landWater == null)
            {
                return landWall.LandTransition;
            }

            if(landWater != null
                && landWall == null)
            {
                return LandTransitionHelper.ReverseLandTransition(landWater.LandTransition);
            }

            return LandTransitionHelper.IntersectionLandTransition(landWall.LandTransition, LandTransitionHelper.ReverseLandTransition(landWater.LandTransition));
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
