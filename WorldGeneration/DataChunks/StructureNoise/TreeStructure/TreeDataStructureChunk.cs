using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.TreeStructure
{
    internal class TreeDataStructureChunk : APointDataStructureChunk
    {
        private WeatherDataAgreggator weatherDataAgreggator;

        public TreeDataStructureChunk(Vector2i position, int nbCaseSide, int nbMinDataStructure, int nbMaxDataStructure, IntRect structDimension)
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
            //TreeDataStructure newTreeDataStructure = new TreeDataStructure(structureWorldPosition, boundingBox, new IntRect(0, boundingBox.Height - 1, boundingBox.Width, 1));

            //newTreeDataStructure.StructureTypeIndex = random.Next();

            return new TreeDataStructure(structureWorldPosition, boundingBox, new IntRect(0, boundingBox.Height - 1, boundingBox.Width, 1));
        }

        protected override bool IsDataStructureValid(Random random, DataChunkLayersMonitor dataChunksMonitor, IDataStructure dataStructure)
        {
            Vector2i structureWorldPosition = dataStructure.StructureWorldPosition;

            float temperature = weatherDataAgreggator.GetTemperatureAtWorldCoordinates(structureWorldPosition.X, structureWorldPosition.Y);
            float humidity = weatherDataAgreggator.GetHumidityAtWorldCoordinates(structureWorldPosition.X, structureWorldPosition.Y);

            BiomeType biomeValue = dataChunksMonitor.WeatherMonitor.GetBiomeAt(temperature, humidity);

            TreeDataStructure treeDataStructure = dataStructure as TreeDataStructure;
            treeDataStructure.UpdateStructureTypeIndexFrom(random, biomeValue);

            float ratio = dataChunksMonitor.FloraRatioManager.GetTreeRatioFromBiomeAltitude(biomeValue, 0);

            return random.NextDouble() < ratio;
            //return true;
        }
    }
}