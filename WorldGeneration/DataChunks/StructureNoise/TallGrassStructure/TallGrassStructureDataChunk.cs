using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.TallGrassStructure
{
    internal class TallGrassStructureDataChunk : AStructureDataChunk
    {
        private WeatherDataAgreggator weatherDataAgreggator;

        public TallGrassStructureDataChunk(Vector2i position, int nbCaseSide, int nbMinDataStructure, int nbMaxDataStructure, IntRect structDimension) 
            : base(position, nbCaseSide, nbMinDataStructure, nbMaxDataStructure, structDimension)
        {
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            this.weatherDataAgreggator = dataChunksMonitor.DataAgreggators["weather"] as WeatherDataAgreggator;

            base.PrepareChunk(dataChunksMonitor, parentLayer);
        }


        protected override IDataStructure CreateDataStructure(Random random, DataChunkLayersMonitor dataChunksMonitor, IntRect boundingBox, Vector2i structureCenter)
        {
            float temperature = weatherDataAgreggator.GetTemperatureAtWorldCoordinates(structureCenter.X, structureCenter.Y);
            float humidity = weatherDataAgreggator.GetHumidityAtWorldCoordinates(structureCenter.X, structureCenter.Y);

            BiomeType biomeValue = dataChunksMonitor.WeatherMonitor.GetBiomeAt(temperature, humidity);

            float ratio = dataChunksMonitor.TallGrassBiomeManager.GetTallGrassRatioFromBiomeAltitude(biomeValue, 0);

            TallGrassStructure tallGrassStructure = null;
            if (random.NextDouble() < ratio)
            {
                tallGrassStructure = new TallGrassStructure(boundingBox.Left, boundingBox.Top, boundingBox.Width, boundingBox.Height);
            }

            return tallGrassStructure;
        }
    }
}
