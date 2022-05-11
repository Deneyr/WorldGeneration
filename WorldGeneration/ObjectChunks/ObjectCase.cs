using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace WorldGeneration.ObjectChunks
{
    public class ObjectCase : IObjectCase
    {
        public int Altitude
        {
            get;
            private set;
        }

        public Vector2i Position
        {
            get;
            private set;
        }

        // Test properties

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

        public bool IsThereTallGrass
        {
            get;
            set;
        }

        // End test properties

        public ObjectCase(Vector2i worldPosition, int altitude)
        {
            this.Position = worldPosition;
            this.Altitude = altitude;
        }
    }
}
