using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ObjectChunks
{
    public class TestCase : ICase
    {
        public Vector2i Position
        {
            get;
            private set;
        }

        public int AltitudeValue
        {
            get;
            set;
        }

        public int BiomeValue
        {
            get;
            set;
        }

        public bool IsUnderSea
        {
            get;
            set;
        }

        public TestCase(int x, int y)
        {
            this.Position = new Vector2i(x, y);
            this.AltitudeValue = 0;
            this.BiomeValue = 0;
            this.IsUnderSea = false;
        }
    }
}
