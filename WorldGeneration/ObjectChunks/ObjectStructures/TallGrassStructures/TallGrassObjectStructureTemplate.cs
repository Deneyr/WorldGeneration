using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TallGrassStructures
{
    internal class TallGrassObjectStructureTemplate : ADataObjectStructureTemplate
    {
        public TallGrassObjectStructureTemplate() 
            : base("TallGrassStructure")
        {
        }

        protected override void UpdateZObjectCase(Random random, IZObjectCase zObjectCase, IDataStructure dataStructure, int worldAltitude, IObjectStructure parentObjectStructure, int i, int j)
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
                    ATallGrassElementLandObject tallGrassElement = this.CreateTallGrassElementLandObjectFrom(dataStructure.StructureBiome, random.Next());

                    tallGrassElement.ParentStructureUID = parentObjectStructure.UID;

                    currentObjectCase.Land.LandOverGround = tallGrassElement;
                }
            }
        }

        //protected override bool ValidateZObjectCase(IZObjectCase zObjectCase, int worldAltitude, int baseLocalI, int baseLocalJ)
        //{
        //    if(zObjectCase.GroundAltitude < 0)
        //    {
        //        return false;
        //    }

        //    ObjectCase currentObjectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;

        //    if (currentObjectCase.Land.LandWater != null)
        //    {
        //        return false;
        //    }

        //    if(currentObjectCase.Land.LandWall != null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        protected override IObjectStructure CreateObjectStructureFrom(Random random, string structureUid, IDataStructure dataStructure, int worldAltitude)
        {
            TallGrassObjectStructure newTallGrassStructure = new TallGrassObjectStructure(this.TemplateUID, structureUid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);

            newTallGrassStructure.BiomeType = dataStructure.StructureBiome;

            return newTallGrassStructure;
        }

        private ATallGrassElementLandObject CreateTallGrassElementLandObjectFrom(BiomeType biomeType, int landElementObjectId)
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
