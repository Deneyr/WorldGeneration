using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.ObjectChunks
{
    public class ZObjectCase : IZObjectCase
    {
        private IObjectCase[] zModelCases;

        public BiomeType ObjectBiome
        {
            get;
            set;
        }

        public IObjectCase this[int z]
        {
            get
            {
                return this.zModelCases[z];
            }
        }

        public Vector2i Position
        {
            get;
        }

        public int NbAltitudeLevel
        {
            get
            {
                return this.zModelCases.Length;
            }
        }

        public int GroundAltitude
        {
            get;
            private set;
        }

        public ZObjectCase(Vector2i worldPosition, int nbZCases)
        {
            this.Position = worldPosition;
            this.zModelCases = new IObjectCase[nbZCases];

            for(int i = 0; i < nbZCases; i++)
            {
                this.zModelCases[i] = null;
            }

            this.GroundAltitude = -1;
        }

        public void SetGroundCaseAt(IObjectCase objectCase)
        {
            int altitude = objectCase.Altitude;

            if (objectCase == null)
            {
                if (this.zModelCases[altitude] != null && this.GroundAltitude == altitude)
                {
                    int nextAltitude = altitude - 1;
                    while(nextAltitude >= 0 || this.zModelCases[nextAltitude] == null)
                    {
                        nextAltitude--;
                    }

                    this.GroundAltitude = nextAltitude;
                }
                this.zModelCases[altitude] = null;
            }
            else
            {
                if(this.GroundAltitude < altitude)
                {
                    this.GroundAltitude = altitude;
                }
                this.zModelCases[altitude] = objectCase;
            }
        }

        public void SetCaseAt(IObjectCase objectCase)
        {
            this.zModelCases[objectCase.Altitude] = objectCase;
        }
    }
}
