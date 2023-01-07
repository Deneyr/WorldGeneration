using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace WorldGeneration.DataChunks.StructureNoise.TownStructure
{
    internal class TownDataStructure : ADataStructure
    {
        public TownDataStructure(Vector2i structureWorldPosition, IntRect structureBoundingBox, IntRect structureBaseBoundingBox) 
            : base(structureWorldPosition, structureBoundingBox, structureBaseBoundingBox)
        {
            this.ObjectStructureTemplateId = "TownStructure";
        }

        public override void GenerateStructure(Random random, IDataStructureTemplate structureTemplate)
        {
            int heightMax = this.DataStructureCases.GetLength(0);
            int widthMax = this.DataStructureCases.GetLength(1);

            this.InitializeBorderArrays(out int[] leftBorderIndexes, out int[] rightBorderIndexes, out int[] topBorderIndexes, out int[] botBorderIndexes);

            this.InitializeDataStructureCases();

            this.GenerateStructureBoundaries(random, Math.Min(heightMax, widthMax) / 3, 1,
                leftBorderIndexes, rightBorderIndexes, topBorderIndexes, botBorderIndexes);

            this.GenerateStructureBoundariesLimit(random,
                leftBorderIndexes, rightBorderIndexes, topBorderIndexes, botBorderIndexes);

            this.GenerateStructureCases(random);
        }

        protected override void GenerateStructureFillCase(Random random, int i, int j)
        {
            this.DataStructureCases[i, j] = new TownDataStructureCase(this, this.StructureBoundingBox.Left + j, this.StructureBoundingBox.Top + i);
        }

        protected override void GenerateStructureBorderCase(Random random, int i, int j, LandTransition landTransition)
        {
            TownDataStructureCase newTallGrassStructureCase = new TownDataStructureCase(this, this.StructureBoundingBox.Left + j, this.StructureBoundingBox.Top + i);

            newTallGrassStructureCase.LandTransition = landTransition;

            this.DataStructureCases[i, j] = newTallGrassStructureCase;
        }
    }
}
