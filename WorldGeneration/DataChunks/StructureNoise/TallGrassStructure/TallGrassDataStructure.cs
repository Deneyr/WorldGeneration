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
    internal class TallGrassDataStructure : ADataStructure
    {
        public TallGrassDataStructure(Vector2i structureWorldPosition, IntRect structureBoundingBox, IntRect structureBaseBoundingBox) 
            : base(structureWorldPosition, structureBoundingBox, structureBaseBoundingBox)
        {
            this.ObjectStructureTemplateId = "TallGrassStructure";
        }

        public override void GenerateStructure(Random random, IDataStructureTemplate structureTemplate)
        {
            int heightMax = this.DataStructureCases.GetLength(0);
            int widthMax = this.DataStructureCases.GetLength(1);

            //for (int i = 0; i < heightMax; i++)
            //{
            //    for (int j = 0; j < widthMax; j++)
            //    {
            //        if (random.NextDouble() < 0.95)
            //        {
            //            this.DataStructureCases[i, j] = new TallGrassDataStructureCase(this, this.StructureBoundingBox.Left + j, this.StructureBoundingBox.Height + i);
            //        }
            //    }
            //}

            this.InitializeDataStructureCases();

            this.GenerateStructureBoundaries(random, Math.Min(heightMax, widthMax) / 3, 1);

            this.GenerateStructureBoundariesLimit(random);
        }
    }
}
