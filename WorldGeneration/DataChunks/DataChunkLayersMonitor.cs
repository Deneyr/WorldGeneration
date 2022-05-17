using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.BiomeManager;

namespace WorldGeneration.DataChunks
{
    internal class DataChunkLayersMonitor
    {
        internal WeatherMonitor WeatherMonitor
        {
            get;
            private set;
        }

        internal FloraRatioBiomeManager FloraRatioManager
        {
            get;
            private set;
        }

        internal TallGrassBiomeManager TallGrassBiomeManager
        {
            get;
            private set;
        }

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

        internal List<IDataChunkLayer> WorldDataLayers
        {
            get;
            private set;
        }

        internal DataChunkLayersMonitor(int worldSeed)
        {
            this.WorldSeed = worldSeed;

            this.WeatherMonitor = new WeatherMonitor(@"Resources\WorldGenerator\weatherTexture.bmp");

            this.DataChunksLayers = new Dictionary<string, IDataChunkLayer>();
            this.DataAgreggators = new Dictionary<string, IDataAgreggator>();

            this.WorldDataLayers = new List<IDataChunkLayer>();
        }

        internal void AddDataLayerToGenerator(IDataChunkLayer dataChunkLayerToAdd)
        {
            dataChunkLayerToAdd.DataChunksMonitor = this;

            this.DataChunksLayers.Add(dataChunkLayerToAdd.Id, dataChunkLayerToAdd);
            this.WorldDataLayers.Add(dataChunkLayerToAdd);
        }

        internal void AddDataAgreggatorToGenerator(string id, IDataAgreggator dataAgreggator)
        {
            this.DataAgreggators.Add(id, dataAgreggator);
        }

        internal void AddAltitudeAgreggatorToGenerator(string id, AltitudeDataAgreggator altitudeDataAgreggator)
        {
            this.AddDataAgreggatorToGenerator(id, altitudeDataAgreggator);

            this.FloraRatioManager = new FloraRatioBiomeManager(altitudeDataAgreggator.SeaLevel);
            this.TallGrassBiomeManager = new TallGrassBiomeManager(altitudeDataAgreggator.SeaLevel);
        }

        internal void UpdateWorldArea(IntRect newWorldArea)
        {
            foreach (IDataChunkLayer dataChunkLayer in this.WorldDataLayers)
            {
                dataChunkLayer.UpdateLayerArea(newWorldArea);
            }
        }     
    }
}
