using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class WeatherDataAgreggator : IDataAgreggator
    {
        public int NbBiomeLevel
        {
            get;
            private set;
        }

        internal IDataChunkLayer TemperatureLayer
        {
            get;
            set;
        }

        internal IDataChunkLayer HumidityLayer
        {
            get;
            set;
        }

        public WeatherDataAgreggator()
        {
        }

        public float GetTemperatureAtWorldCoordinates(int x, int y)
        {
            return (this.TemperatureLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;
        }

        public float GetHumidityAtWorldCoordinates(int x, int y)
        {
            return (this.HumidityLayer.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;
        }
    }
}