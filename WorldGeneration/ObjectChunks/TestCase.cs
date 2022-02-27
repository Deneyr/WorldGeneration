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

        public float Value
        {
            get;
            set;
        }

        public TestCase(int x, int y)
        {
            this.Position = new Vector2i(x, y);
            this.Value = 0;
        }
    }
}
