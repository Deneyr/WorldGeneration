using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeStructure;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures
{
    internal class TreeStructureTemplate : ACaseObjectStructureTemplate
    {
        public TreeStructureTemplate() 
            : base("TreeStructure", new Vector2i(3, 3), 3, new IntRect(0, 2, 3, 1))
        {
            this.enumValueStructure = new int[3, 3, 3]
            {
                { {-1, -1, -1 },
                {-1, -1, -1 },
                {(int)TreeStructure.TreePart.BOT_LEFT, (int)TreeStructure.TreePart.BOT_MID, (int)TreeStructure.TreePart.BOT_RIGHT } },

                { {-1, -1, -1 },
                {(int)TreeStructure.TreePart.MID_LEFT, (int)TreeStructure.TreePart.MID_MID, (int)TreeStructure.TreePart.MID_RIGHT },
                {-1, -1, -1 } },

                { {(int)TreeStructure.TreePart.TOP_LEFT, (int)TreeStructure.TreePart.TOP_MID, (int)TreeStructure.TreePart.TOP_RIGHT },
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
                currentObjectCase.Land.LandOverGround = new SideTreeElementLandObject(random.Next(), treePart);
            }
            else
            {
                currentObjectCase.Land.LandWall = new MainTreeElementLandObject(random.Next(), treePart);
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

        protected override IObjectStructure CreateObjectStructureFrom(Random random, Vector2i worldPosition, int worldAltitude)
        {
            return new TreeStructure(this.TemplateUID, random.Next(), worldPosition, worldAltitude);
        }
    }
}
