using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.StructureNoise.TownStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectChunkLayers;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;
using WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TownStructures
{
    internal class TownObjectStructureTemplate : ADataObjectStructureTemplate
    {
        //private AltitudeObjectChunkLayer altitudeObjectChunkLayer;

        public TownObjectStructureTemplate()
            : base("TownStructure")
        {
        }

        protected override void UpdateZObjectCase(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IZObjectCase zObjectCase, IDataStructure dataStructure, int worldAltitude, IObjectStructure parentObjectStructure, int i, int j)
        {
            if (zObjectCase.GroundAltitude >= 0)
            {
                TownDataStructure townDataStructure = dataStructure as TownDataStructure;

                IDataStructureCase dataStructureCase = (dataStructure as ADataStructure).DataStructureCases[i, j];

                ObjectCase currentObjectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;
                if (this.IsCaseValid(townDataStructure, dataStructureCase, currentObjectCase))
                {
                    LandTransition dataCaseLandTransition = LandTransitionHelper.ReverseLandTransition((dataStructureCase as TownDataStructureCase).LandTransition);

                    int index = zObjectCase.GroundAltitude;
                    IObjectCase currentIObjectCase = null;
                    while (index >= 0 && (currentIObjectCase = zObjectCase[index]) != null)
                    {
                        currentObjectCase = currentIObjectCase as ObjectCase;

                        if (this.IsCaseValid(townDataStructure, dataStructureCase, currentObjectCase))
                        {
                            LandCase landCase = currentObjectCase.Land;

                            if (landCase.LandGroundList.Count > 0)
                            {
                                LandTransition newDataCaseLandTransition = dataCaseLandTransition;
                                if (dataCaseLandTransition != LandTransition.NONE && landCase.LandWater != null)
                                {
                                    newDataCaseLandTransition = LandTransitionHelper.IntersectionLandTransition(dataCaseLandTransition, LandTransitionHelper.ReverseLandTransition(landCase.LandWater.LandTransition));
                                }

                                landCase.LandGroundList = this.CreateTownLandGroundFrom(random, parentObjectStructure, townDataStructure, LandTransition.NONE, newDataCaseLandTransition, landCase.LandGroundList);
                            }

                            if (landCase.LandWall != null && landCase.LandGroundOverWallList.Count > 0)
                            {
                                LandTransition landWallTransition = LandTransition.NONE;
                                LandTransition newDataCaseLandTransition = dataCaseLandTransition;

                                if (dataCaseLandTransition != LandTransition.NONE && landCase.LandWall != null)
                                {
                                    newDataCaseLandTransition = LandTransitionHelper.IntersectionLandTransition(dataCaseLandTransition, landCase.LandWall.LandTransition);
                                }

                                landWallTransition = landCase.LandWall.LandTransition;

                                if (newDataCaseLandTransition != landCase.LandWall.LandTransition)
                                {
                                    landCase.LandGroundOverWallList = this.CreateTownLandGroundFrom(random, parentObjectStructure, townDataStructure, landWallTransition, newDataCaseLandTransition, landCase.LandGroundOverWallList);
                                }
                            }
                        }
                        index--;
                    }
                }
            }
        }

        private List<ILandGround> CreateTownLandGroundFrom(Random random, IObjectStructure parentObjectStructure, TownDataStructure townDataStructure, LandTransition landWallTransition, LandTransition townLandTransition, List<ILandGround> initialLandGround)
        {
            GroundLandObject groundLandObject = initialLandGround.First() as GroundLandObject;

            ATownGroundLandObject newTownGroundLandObject = CreateTownGroundLandObject(random, townDataStructure.StructureBiome, groundLandObject.Type);
            newTownGroundLandObject.ParentStructureUID = parentObjectStructure.UID;
            if (landWallTransition != LandTransition.NONE)
            {
                newTownGroundLandObject.LandTransition = landWallTransition;
            }

            return LandTransitionHelper.AddFirstGroundLandObjectTo(initialLandGround, newTownGroundLandObject, townLandTransition);
        }

        private bool IsCaseValid(TownDataStructure parentTownDataStructure, IDataStructureCase dataStructureCase, ObjectCase currentObjectCase)
        {
            if (dataStructureCase == null)
            {
                return false;
            }

            if (currentObjectCase.Land.LandWater != null
                && (currentObjectCase.Land.LandWall == null && currentObjectCase.Land.LandWater.LandTransition == LandTransition.NONE))
            {
                return false;
            }

            return true;
        }

        protected override IObjectStructure CreateObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, Random random, string structureUid, IDataStructure dataStructure, int worldAltitude)
        {
            //this.altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);

            TownObjectStructure newTallGrassStructure = new TownObjectStructure(this.TemplateUID, structureUid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);

            newTallGrassStructure.StructureTypeIndex = dataStructure.StructureTypeIndex;
            newTallGrassStructure.BiomeType = dataStructure.StructureBiome;

            return newTallGrassStructure;
        }


        public static ATownGroundLandObject CreateTownGroundLandObject(Random random, BiomeType biomeType, LandType landType)
        {
            int landObjectId = random.Next();
            switch (biomeType)
            {
                case BiomeType.TUNDRA:
                    return new TundraTownGroundLandObject(landObjectId, landType);
                case BiomeType.BOREAL_FOREST:
                    return new BorealForestTownGroundLandObject(landObjectId, landType);
                case BiomeType.SAVANNA:
                    return new SavannaTownGroundLandObject(landObjectId, landType);
                case BiomeType.TEMPERATE_FOREST:
                    return new TemperateForestTownGroundLandObject(landObjectId, landType);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return new TemperateRainForestTownGroundLandObject(landObjectId, landType);
                case BiomeType.DESERT:
                    return new DesertTownGroundLandObject(landObjectId, landType);
                case BiomeType.TROPICAL_WOODLAND:
                    return new TropicalWoodlandTownGroundLandObject(landObjectId, landType);
                case BiomeType.SEASONAL_FOREST:
                    return new SeasonalForestTownGroundLandObject(landObjectId, landType);
                case BiomeType.RAINFOREST:
                    return new RainForestTownGroundLandObject(landObjectId, landType);
            }
            return null;
        }
    }
}