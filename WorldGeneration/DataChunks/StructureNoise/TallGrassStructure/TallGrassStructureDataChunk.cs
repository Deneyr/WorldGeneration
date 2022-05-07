using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.DataChunks.StructureNoise.TallGrassStructure
{
    internal class TallGrassStructureDataChunk : AStructureDataChunk
    {
        public TallGrassStructureDataChunk(Vector2i position, int nbCaseSide, int nbMinDataStructure, int nbMaxDataStructure, IntRect structDimension) 
            : base(position, nbCaseSide, nbMinDataStructure, nbMaxDataStructure, structDimension)
        {
        }

        protected override IDataStructure CreateDataStructure(Random random, IntRect boundingBox)
        {
            return new TallGrassStructure(boundingBox.Left, boundingBox.Top, boundingBox.Width, boundingBox.Height);
        }
    }
}
