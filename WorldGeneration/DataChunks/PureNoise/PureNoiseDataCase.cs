using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.PureNoise
{
    internal class PureNoiseDataCase : ICase
    {
        public Vector2i Position
        {
            get;
            private set;
        }

        public float Value
        {
            get;
            set;
        }

        public PureNoiseDataCase(int x, int y)
        {
            this.Position = new Vector2i(x, y);
            this.Value = 0;
        }
    }
}
