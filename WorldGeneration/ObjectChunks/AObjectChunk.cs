using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ObjectChunks
{
    public class AObjectChunk : IObjectChunk
    {
        public IZObjectCase[,] ZCasesArray
        {
            get;
            protected set;
        }

        public Vector2i Position
        {
            get;
            private set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        public AObjectChunk(Vector2i position, int nbCaseSide)
        {
            this.Position = position;
            this.NbCaseSide = nbCaseSide;
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            return this.ZCasesArray[y, x];
        }
    }
}
