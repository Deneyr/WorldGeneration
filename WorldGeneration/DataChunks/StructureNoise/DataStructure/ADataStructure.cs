using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal abstract class ADataStructure : IDataStructure
    {
        private IDataStructureCase[,] dataStructureCases;

        public IntRect StructureBoundingBox
        {
            get;
            private set;
        }

        public ADataStructure(int left, int top, int width, int height)
        {
            this.StructureBoundingBox = new IntRect(left, top, width, height);
            this.dataStructureCases = new IDataStructureCase[height, width];
        }

        public abstract void GenerateStructure(IDataStructureTemplate structureTemplate);

        public IDataStructureCase GetStructureCaseAtChunkCoordinate(int x, int y)
        {
            if(this.StructureBoundingBox.Contains(x, y))
            {
                int localX = (x - this.StructureBoundingBox.Left);
                int localY = (y - this.StructureBoundingBox.Top);

                return this.dataStructureCases[localY, localX];
            }
            return null;
        }
    }
}
