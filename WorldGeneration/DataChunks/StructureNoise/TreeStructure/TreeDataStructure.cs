using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.DataChunks.StructureNoise.TreeStructure
{
    internal class TreeDataStructure : AAreaDataStructure
    {
        public TreeDataStructure(Vector2i structureWorldPosition, IntRect structureBoundingBox, IntRect structureBaseBoundingBox)
            : base(structureWorldPosition, structureBoundingBox, structureBaseBoundingBox)
        {
            this.ObjectStructureTemplateId = "TreeStructure";
        }

        public override void GenerateStructure(Random random, IDataStructureTemplate structureTemplate)
        {
            //int heightMax = this.DataStructureCases.GetLength(0);
            //int widthMax = this.DataStructureCases.GetLength(1);

            //for (int i = 0; i < heightMax; i++)
            //{
            //    for (int j = 0; j < widthMax; j++)
            //    {
            //        if (i == heightMax - 1
            //            && (j == 0 || j == widthMax - 1))
            //        {
            //            this.DataStructureCases[i, j] = new TreeDataStructureCase(this, this.StructureBoundingBox.Left + j, this.StructureBoundingBox.Height + i, TreePart.SIDE);
            //        }
            //        else
            //        {
            //            this.DataStructureCases[i, j] = new TreeDataStructureCase(this, this.StructureBoundingBox.Left + j, this.StructureBoundingBox.Height + i, TreePart.MAIN);
            //        }
            //    }
            //}
        }
    }
}
