using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ChunksMonitoring
{
    public interface IChunk
    {
        ICase[,] CasesArray
        {
            get;
        }

        Vector2i Position
        {
            get;
        }

        int NbCaseSide
        {
            get;
        }
    }
}
