using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ChunksMonitoring
{
    public class ChunkContainer: IChunk
    {
        public Vector2i Position
        {
            get;
            private set;
        }

        public ICase[,] CasesArray
        {
            get
            {
                if (this.ContainedChunk != null)
                {
                    return this.ContainedChunk.CasesArray;
                }
                return null;
            }
        }

        public int NbCaseSide
        {
            get
            {
                if (this.ContainedChunk != null)
                {
                    return this.ContainedChunk.NbCaseSide;
                }
                return 0;
            }
        }

        public IChunk ContainedChunk
        {
            get;
            set;
        }

        public ChunkContainer(int left, int top)
        {
            this.Position = new Vector2i(left, top);
        }
    }
}
