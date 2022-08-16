using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectChunkLayers;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures
{
    internal class NarrowTreeObjectStructureTemplate : ACaseObjectStructureTemplate
    {
        public NarrowTreeObjectStructureTemplate()
            : base("NarrowTreeStructure", new Vector2i(2, 3), 3, new IntRect(0, 2, 2, 1))
        {
            this.enumValueStructure = new int[3, 3, 2]
            {
                { {-1, -1 },
                {-1, -1 },
                {(int)TreeObjectStructure.TreePart.BOT_LEFT, (int)TreeObjectStructure.TreePart.BOT_RIGHT } },

                { {-1, -1},
                {(int)TreeObjectStructure.TreePart.MID_LEFT, (int)TreeObjectStructure.TreePart.MID_RIGHT },
                {-1, -1} },

                { {(int)TreeObjectStructure.TreePart.TOP_LEFT, (int)TreeObjectStructure.TreePart.TOP_RIGHT },
                {-1, -1},
                {-1, -1} }
            };
        }

        protected override void UpdateObjectCase(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectCase objectCase, IDataStructure dataStructure, IObjectStructure parentObjectStructure, int enumValue)
        {
            TreePart treePart = (TreePart)enumValue;
            ObjectCase currentObjectCase = objectCase as ObjectCase;

            //if (treePart == TreePart.BOT_LEFT
            //    || treePart == TreePart.BOT_RIGHT)
            //{
            //    ASideTreeElementLandObject sideTreeElement = TreeObjectStructureTemplate.CreateSideTreeElementLandObjectFrom(dataStructure.StructureBiome, dataStructure.StructureTypeIndex, treePart);

            //    sideTreeElement.ParentStructureUID = parentObjectStructure.UID;

            //    currentObjectCase.Land.LandOverGround = sideTreeElement;
            //}
            //else
            //{
            //    AMainTreeElementLandObject mainTreeElement = TreeObjectStructureTemplate.CreateMainTreeElementLandObjectFrom(dataStructure.StructureBiome, dataStructure.StructureTypeIndex, treePart);

            //    mainTreeElement.ParentStructureUID = parentObjectStructure.UID;

            //    currentObjectCase.Land.LandWall = mainTreeElement;
            //}
            AMainTreeElementLandObject mainTreeElement = TreeObjectStructureTemplate.CreateMainTreeElementLandObjectFrom(dataStructure.StructureBiome, dataStructure.StructureTypeIndex, treePart);

            mainTreeElement.ParentStructureUID = parentObjectStructure.UID;

            currentObjectCase.Land.LandWall = mainTreeElement;
        }

        protected override IObjectStructure CreateObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, Random random, string structureUid, IDataStructure dataStructure, int worldAltitude)
        {
            AltitudeObjectChunkLayer altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);

            TreeObjectStructure newTreeObjectStructure = new TreeObjectStructure(this.TemplateUID, structureUid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);

            newTreeObjectStructure.StructureTypeIndex = dataStructure.StructureTypeIndex;
            newTreeObjectStructure.BiomeType = dataStructure.StructureBiome;
            newTreeObjectStructure.LandType = altitudeObjectChunkLayer.GetAltitudeLandType(dataStructure.StructureBiome, worldAltitude);

            return newTreeObjectStructure;
        }
    }
}
