using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures
{
    internal class TreeObjectStructureTemplate : ACaseObjectStructureTemplate
    {
        public TreeObjectStructureTemplate() 
            : base("TreeStructure", new Vector2i(3, 3), 3, new IntRect(0, 2, 3, 1))
        {
            this.enumValueStructure = new int[3, 3, 3]
            {
                { {-1, -1, -1 },
                {-1, -1, -1 },
                {(int)TreeObjectStructure.TreePart.BOT_LEFT, (int)TreeObjectStructure.TreePart.BOT_MID, (int)TreeObjectStructure.TreePart.BOT_RIGHT } },

                { {-1, -1, -1 },
                {(int)TreeObjectStructure.TreePart.MID_LEFT, (int)TreeObjectStructure.TreePart.MID_MID, (int)TreeObjectStructure.TreePart.MID_RIGHT },
                {-1, -1, -1 } },

                { {(int)TreeObjectStructure.TreePart.TOP_LEFT, (int)TreeObjectStructure.TreePart.TOP_MID, (int)TreeObjectStructure.TreePart.TOP_RIGHT },
                {-1, -1, -1 },
                {-1, -1, -1 } }
            };
        }

        protected override void UpdateObjectCase(Random random, IObjectCase objectCase, IDataStructure dataStructure, IObjectStructure parentObjectStructure, int enumValue)
        {
            TreePart treePart = (TreePart)enumValue;
            ObjectCase currentObjectCase = objectCase as ObjectCase;

            if (treePart == TreePart.BOT_LEFT
                || treePart == TreePart.BOT_RIGHT)
            {
                ASideTreeElementLandObject sideTreeElement = this.CreateSideTreeElementLandObjectFrom(dataStructure.StructureBiome, random.Next(), treePart);

                sideTreeElement.ParentStructureUID = parentObjectStructure.UID;

                currentObjectCase.Land.LandOverGround = sideTreeElement;
            }
            else
            {
                AMainTreeElementLandObject mainTreeElement = this.CreateMainTreeElementLandObjectFrom(dataStructure.StructureBiome, random.Next(), treePart);

                mainTreeElement.ParentStructureUID = parentObjectStructure.UID;

                currentObjectCase.Land.LandWall = mainTreeElement;
            }
        }

        private AMainTreeElementLandObject CreateMainTreeElementLandObjectFrom(BiomeType biomeType, int landElementObjectId, TreePart treePart)
        {
            switch (biomeType)
            {
                case BiomeType.BOREAL_FOREST:
                    return new BorealForestMainTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.DESERT:
                    return new DesertMainTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.RAINFOREST:
                    return new RainForestMainTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.SAVANNA:
                    return new SavannaMainTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.SEASONAL_FOREST:
                    return new SeasonalMainTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.TEMPERATE_FOREST:
                    return new TemperateForestMainTreeElementObject(landElementObjectId, treePart);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return new TemperateRainForestMainTreeElementObject(landElementObjectId, treePart);
                case BiomeType.TROPICAL_WOODLAND:
                    return new TropicalWoodlandMainTreeElementObject(landElementObjectId, treePart);
                case BiomeType.TUNDRA:
                    return new TundraMainTreeElementObject(landElementObjectId, treePart);
            }
            return new TundraMainTreeElementObject(landElementObjectId, treePart);
        }

        private ASideTreeElementLandObject CreateSideTreeElementLandObjectFrom(BiomeType biomeType, int landElementObjectId, TreePart treePart)
        {
            switch (biomeType)
            {
                case BiomeType.BOREAL_FOREST:
                    return new BorealForestSideTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.DESERT:
                    return new DesertSideTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.RAINFOREST:
                    return new RainForestSideTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.SAVANNA:
                    return new SavannaSideTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.SEASONAL_FOREST:
                    return new SeasonalSideTreeElementLandObject(landElementObjectId, treePart);
                case BiomeType.TEMPERATE_FOREST:
                    return new TemperateForestSideTreeElementObject(landElementObjectId, treePart);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return new TemperateRainForestSideTreeElementObject(landElementObjectId, treePart);
                case BiomeType.TROPICAL_WOODLAND:
                    return new TropicalWoodlandSideTreeElementObject(landElementObjectId, treePart);
                case BiomeType.TUNDRA:
                    return new TundraSideTreeElementObject(landElementObjectId, treePart);
            }
            return new TundraSideTreeElementObject(landElementObjectId, treePart);
        }

        protected override IObjectStructure CreateObjectStructureFrom(Random random, string structureUid, IDataStructure dataStructure, int worldAltitude)
        {
            return new TreeObjectStructure(this.TemplateUID, structureUid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);
        }
    }
}
