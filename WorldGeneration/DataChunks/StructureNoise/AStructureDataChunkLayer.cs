using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.DataChunks.StructureNoise
{
    internal abstract class AStructureDataChunkLayer : ADataChunkLayer
    {
        public IntRect StructDimension
        {
            get;
            set;
        }

        public int NbMinDataStructure
        {
            get;
            set;
        }

        public int nbMaxDataStructure
        {
            get;
            set;
        }

        public AStructureDataChunkLayer(string id, int nbCaseSide) 
            : base(id, nbCaseSide)
        {
            this.StructDimension = new IntRect();
            this.NbMinDataStructure = 0;
            this.nbMaxDataStructure = 0;
        }
    }
}
