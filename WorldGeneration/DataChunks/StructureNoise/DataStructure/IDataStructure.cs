using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal interface IDataStructure
    {
        IntRect StructureBoundingBox
        {
            get;
        }

        void GenerateStructure(IDataStructureTemplate structureTemplate);

        IDataStructureCase GetStructureCaseAtChunkCoordinate(int x, int y);
    }
}
