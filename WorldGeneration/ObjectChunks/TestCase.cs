using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.WeatherMonitoring;

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

        public BiomeType BiomeValue
        {
            get;
            set;
        }

        public bool IsUnderSea
        {
            get;
            set;
        }

        public float RiverValue
        {
            get;
            set;
        }

        public bool IsThereTree
        {
            get;
            set;
        }

        public bool IsThereRock
        {
            get;
            set;
        }

        public bool IsThereFlower
        {
            get;
            set;
        }

        public int IsThereTallGrass
        {
            get;
            set;
        }

        public TestCase(int x, int y)
        {
            this.Position = new Vector2i(x, y);
            this.AltitudeValue = 0;
            this.BiomeValue = BiomeType.TEMPERATE_FOREST;
            this.IsUnderSea = false;
            this.RiverValue = 0;
            this.IsThereTree = false;
            this.IsThereFlower = false;
            this.IsThereTallGrass = -1;
        }
    }
}
