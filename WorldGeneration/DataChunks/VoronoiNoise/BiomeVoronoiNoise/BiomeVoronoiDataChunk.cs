using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;

namespace WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise
{
    internal class BiomeVoronoiDataChunk : VoronoiDataChunk
    {
        public BiomeVoronoiDataChunk(Vector2i position, int nbCaseSide, int nbPointsInside, int sampleLevel)
            : base(position, nbCaseSide, nbPointsInside, sampleLevel)
        {

        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            base.PrepareChunk(dataChunksMonitor, parentLayer);

            WeatherDataAgreggator weatherDataAgreggator = dataChunksMonitor.DataAgreggators["weather"] as WeatherDataAgreggator;

            foreach(VoronoiDataPoint point in this.Points)
            {
                float temperature = weatherDataAgreggator.GetTemperatureAtWorldCoordinates(point.PointPosition.X, point.PointPosition.Y);
                float humidity = weatherDataAgreggator.GetHumidityAtWorldCoordinates(point.PointPosition.X, point.PointPosition.Y);

                int biomeValue = (int) dataChunksMonitor.WeatherMonitor.GetBiomeAt(temperature, humidity);

                point.PointValue = biomeValue;
            }
        }
    }
}
