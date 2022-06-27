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
    internal class TreeDataStructure : ADataStructure
    {
        public TreeDataStructure(Vector2i structureWorldPosition, IntRect structureBoundingBox, IntRect structureBaseBoundingBox)
            : base(structureWorldPosition, structureBoundingBox, structureBaseBoundingBox)
        {
            this.ObjectStructureTemplateId = "TreeStructure";
        }

        public override void GenerateStructure(Random random, IDataStructureTemplate structureTemplate)
        {
            int heightMax = this.DataStructureCases.GetLength(0);
            int widthMax = this.DataStructureCases.GetLength(1);

            for (int i = 0; i < widthMax; i++)
            {
                for (int j = 0; j < heightMax; j++)
                {
                    this.DataStructureCases[j, i] = new TreeDataStructureCase(this, this.StructureBoundingBox.Left + i, this.StructureBoundingBox.Height + j);
                }
            }

            this.GenerateStructureBoundaries2(random, Math.Min(heightMax, widthMax) / 3, 1);
        }
    }
}
