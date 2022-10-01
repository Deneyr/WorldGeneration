using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands;

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

            this.InitializeBorderArrays(out int[] leftBorderIndexes, out int[] rightBorderIndexes, out int[] topBorderIndexes, out int[] botBorderIndexes);

            this.InitializeDataStructureCases();

            this.GenerateStructureBoundaries(random, Math.Min(heightMax, widthMax) / 3, 1,
                leftBorderIndexes, rightBorderIndexes, topBorderIndexes, botBorderIndexes);

            if (this.StructureBiome == BiomeType.SAVANNA)
            {
                this.GenerateStructureBoundariesLimit(random,
                    leftBorderIndexes, rightBorderIndexes, topBorderIndexes, botBorderIndexes);
            }

            this.GenerateStructureCases(random);
        }

        protected override void GenerateStructureFillCase(Random random, int i, int j)
        {
            if (this.StructureBiome == BiomeType.SAVANNA
                || random.NextDouble() < 0.95)
            {
                this.DataStructureCases[i, j] = new TallGrassDataStructureCase(this, this.StructureBoundingBox.Left + j, this.StructureBoundingBox.Top + i);
            }
        }

        protected override void GenerateStructureBorderCase(Random random, int i, int j, LandTransition landTransition)
        {
            TallGrassDataStructureCase newTallGrassStructureCase = new TallGrassDataStructureCase(this, this.StructureBoundingBox.Left + j, this.StructureBoundingBox.Top + i);

            newTallGrassStructureCase.LandTransition = landTransition;

            this.DataStructureCases[i, j] = newTallGrassStructureCase;
        }
    }
}
