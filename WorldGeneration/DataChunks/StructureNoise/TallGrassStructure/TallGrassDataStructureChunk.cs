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
    internal class TallGrassDataStructureChunk : APointDataStructureChunk
    {
        private WeatherDataAgreggator weatherDataAgreggator;

        public TallGrassDataStructureChunk(Vector2i position, int nbCaseSide, int nbMinDataStructure, int nbMaxDataStructure, IntRect structDimension) 
            : base(position, nbCaseSide, nbMinDataStructure, nbMaxDataStructure, structDimension)
        {
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            this.weatherDataAgreggator = dataChunksMonitor.DataAgreggators["weather"] as WeatherDataAgreggator;

            base.PrepareChunk(dataChunksMonitor, parentLayer);
        }


        protected override IDataStructure CreateDataStructure(Random random, DataChunkLayersMonitor dataChunksMonitor, IntRect boundingBox, IntRect baseBoundingBox, Vector2i structureWorldPosition)
        {
            //TallGrassDataStructure newTallGrassDataStructure = new TallGrassDataStructure(structureWorldPosition, boundingBox, baseBoundingBox);

            //newTallGrassDataStructure.StructureTypeIndex = random.Next();

            return new TallGrassDataStructure(structureWorldPosition, boundingBox, baseBoundingBox); ;
        }

        protected override bool IsDataStructureValid(Random random, DataChunkLayersMonitor dataChunksMonitor, IDataStructure dataStructure)
        {
            Vector2i structureWorldCenter = dataStructure.StructureWorldPosition;

            float temperature = weatherDataAgreggator.GetTemperatureAtWorldCoordinates(structureWorldCenter.X, structureWorldCenter.Y);
            float humidity = weatherDataAgreggator.GetHumidityAtWorldCoordinates(structureWorldCenter.X, structureWorldCenter.Y);

            BiomeType biomeValue = dataChunksMonitor.WeatherMonitor.GetBiomeAt(temperature, humidity);

            TallGrassDataStructure tallGrassDataStructure = dataStructure as TallGrassDataStructure;
            tallGrassDataStructure.StructureTypeIndex = random.Next();
            tallGrassDataStructure.StructureBiome = biomeValue;

            float ratio = dataChunksMonitor.TallGrassBiomeManager.GetTallGrassRatioFromBiomeAltitude(biomeValue, 0);

            return random.NextDouble() < ratio;
        }
    }
}
