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
        Vector2i Position
        {
            get;
        }

        int NbCaseSide
        {
            get;
        }

        ICase GetCaseAtLocal(int x, int y);
    }
}
