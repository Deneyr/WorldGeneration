using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.TallGrassStructure
{
    internal class TallGrassStructure : ADataStructure
    {
        public TallGrassStructure(Vector2i structureWorldPosition, IntRect structureBoundingBox, IntRect structureBaseBoundingBox) 
            : base(structureWorldPosition, structureBoundingBox, structureBaseBoundingBox)
        {
            this.ObjectStructureTemplateId = "TallGrassStructure";
        }

        public override void GenerateStructure(Random random, IDataStructureTemplate structureTemplate)
        {
            int heightMax = this.DataStructureCases.GetLength(0);
            int widthMax = this.DataStructureCases.GetLength(1);

            for (int i = 0; i < widthMax; i++)
            {
                for (int j = 0; j < heightMax; j++)
                {
                    this.DataStructureCases[j, i] = new TallGrassStructureCase(this, this.StructureBoundingBox.Left + i, this.StructureBoundingBox.Height + j);
                }
            }

            this.GenerateStructureBoundaries2(random, Math.Min(heightMax, widthMax) / 3, 1);
        }
    }
}
