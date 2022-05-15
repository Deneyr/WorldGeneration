using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks;
using WorldGeneration.DataChunks.DataAgreggator;

namespace WorldGeneration.ObjectChunks
{
    public class ObjectChunkLayersMonitor
    {
        internal DataChunkLayersMonitor DataChunkMonitor
        {
            get;
            private set;
        }

        internal List<IObjectChunkLayer> ObjectChunksLayers
        {
            get;
            private set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        internal int WorldSeed
        {
            get;
            private set;
        }

        internal ObjectChunkLayersMonitor(DataChunkLayersMonitor dataChunkLayersMonitor, int nbCaseSide, int worldSeed)
        {
            this.WorldSeed = worldSeed;
            this.NbCaseSide = nbCaseSide;

            this.ObjectChunksLayers = new List<IObjectChunkLayer>();

            this.DataChunkMonitor = dataChunkLayersMonitor;
        }

        internal void AddObjectLayerToGenerator(IObjectChunkLayer objectChunkLayerToAdd)
        {
            this.ObjectChunksLayers.Add(objectChunkLayerToAdd);
        }

        //private float timeMean = 0;
        //private int nb = 0;

        public IObjectChunk GenerateChunkAt(Vector2i position)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            //TestChunk newChunk = new TestChunk(position, this.NbCaseSide, (this.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator).SeaLevel);

            //newChunk.GenerateChunk(this, null);

            //sw.Stop();

            //timeMean = timeMean * nb + sw.ElapsedMilliseconds;
            //nb++;
            //timeMean = timeMean / nb;

            //return newChunk;

            IObjectChunk newChunk = this.CreateObjectChunkAt(position);

            foreach (IObjectChunkLayer objectChunkLayer in this.ObjectChunksLayers)
            {
                objectChunkLayer.ComputeObjectChunk(this, newChunk);
            }

            //sw.Stop();

            //timeMean = timeMean * nb + sw.ElapsedMilliseconds;
            //nb++;
            //timeMean = timeMean / nb;

            return newChunk;
        }

        private IObjectChunk CreateObjectChunkAt(Vector2i position)
        {
            int nbAltitudeLevel = (this.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator).NbAltitudeLevel;

            return new ObjectChunk(position, this.NbCaseSide, nbAltitudeLevel);
        }
    }
}
