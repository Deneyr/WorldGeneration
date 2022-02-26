﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace WorldGeneration.DataChunks
{
    internal class DataChunkLayersMonitor
    {
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

        internal List<IDataChunkLayer> WorldLayers
        {
            get;
            private set;
        }

        internal DataChunkLayersMonitor(int worldSeed)
        {
            this.WorldSeed = worldSeed;

            this.DataChunksLayers = new Dictionary<string, IDataChunkLayer>();
            this.WorldLayers = new List<IDataChunkLayer>();
        }

        internal void AddDataLayerToGenerator(IDataChunkLayer dataChunkLayerToAdd)
        {
            dataChunkLayerToAdd.DataChunksMonitor = this;

            this.DataChunksLayers.Add(dataChunkLayerToAdd.Id, dataChunkLayerToAdd);
            this.WorldLayers.Add(dataChunkLayerToAdd);
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