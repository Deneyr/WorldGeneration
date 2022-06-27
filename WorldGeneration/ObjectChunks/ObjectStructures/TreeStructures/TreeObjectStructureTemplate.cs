using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
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

        protected override void UpdateObjectCase(Random random, IObjectCase objectCase, IObjectStructure parentObjectStructure, int enumValue)
        {
            TreePart treePart = (TreePart)enumValue;
            ObjectCase currentObjectCase = objectCase as ObjectCase;

            if (treePart == TreePart.BOT_LEFT
                || treePart == TreePart.BOT_RIGHT)
            {
                SideTreeElementLandObject sideTreeElement = new SideTreeElementLandObject(random.Next(), treePart);

                sideTreeElement.ParentStructureUID = parentObjectStructure.UID;

                currentObjectCase.Land.LandOverGround = sideTreeElement;
            }
            else
            {
                MainTreeElementLandObject mainTreeElement = new MainTreeElementLandObject(random.Next(), treePart);

                mainTreeElement.ParentStructureUID = parentObjectStructure.UID;

                currentObjectCase.Land.LandWall = mainTreeElement;
            }
        }

        protected override bool ValidateZObjectCase(IZObjectCase zObjectCase, int worldAltitude, int baseLocalI, int baseLocalJ)
        {
            if(base.ValidateZObjectCase(zObjectCase, worldAltitude, baseLocalI, baseLocalJ) == false)
            {
                return false;
            }

            ObjectCase objectCase = zObjectCase[worldAltitude] as ObjectCase;

            if(objectCase == null)
            {
                return false;
            }

            if(objectCase.Land.LandWater != null)
            {
                return false;
            }

            if (baseLocalJ == 0
                || baseLocalJ == 2)
            {
                return objectCase.Land.LandOverGround != null;
            }
            else if(baseLocalJ == 1)
            {
                return objectCase.Land.LandWall != null;
            }

            return true;
        }

        protected override IObjectStructure CreateObjectStructureFrom(Random random, string structureUid, IDataStructure dataStructure, int worldAltitude)
        {
            return new TreeObjectStructure(this.TemplateUID, structureUid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);
        }
    }
}
