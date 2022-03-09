using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks
{
    internal class DataChunkLayersMonitor
    {
        private WeatherMonitor weatherMonitor;

        internal int WorldSeed
        {
            get;
            private set;
        }

        internal Dictionary<string, IDataChunkLayer> DataChunksLayers
        {
            get;
            private set;
        }

        internal Dictionary<string, IDataAgreggator> DataAgreggators
        {
            get;
            private set;
        }

        internal List<IDataChunkLayer> WorldLayers
        {
            get;
            private set;
        }

        internal DataChunkLayersMonitor(int worldSeed)
        {
            this.WorldSeed = worldSeed;

            this.weatherMonitor = new WeatherMonitor(@"Resources\WorldGenerator\weatherTexture.bmp");

            this.DataChunksLayers = new Dictionary<string, IDataChunkLayer>();
            this.DataAgreggators = new Dictionary<string, IDataAgreggator>();

            this.WorldLayers = new List<IDataChunkLayer>();
        }

        internal void AddDataLayerToGenerator(IDataChunkLayer dataChunkLayerToAdd)
        {
            dataChunkLayerToAdd.DataChunksMonitor = this;

            this.DataChunksLayers.Add(dataChunkLayerToAdd.Id, dataChunkLayerToAdd);
            this.WorldLayers.Add(dataChunkLayerToAdd);
        }

        internal void AddDataAgreggatorToGenerator(string id, IDataAgreggator dataAgreggator)
        {
            this.DataAgreggators.Add(id, dataAgreggator);
        }

        internal void UpdateWorldArea(IntRect newWorldArea)
        {
            foreach (IDataChunkLayer dataChunkLayer in this.WorldLayers)
            {
                dataChunkLayer.UpdateLayerArea(newWorldArea);
            }
        }     
    }
}
