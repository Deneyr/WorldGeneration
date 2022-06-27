using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks
{
    internal class ObjectChunkLayersMonitor
    {
        internal DataChunkLayersMonitor DataChunkMonitor
        {
            get;
            private set;
        }

        internal List<IObjectChunkLayer> WorldObjectLayers
        {
            get;
            private set;
        }

        //internal ObjectStructureManager ObjectStructureManager
        //{
        //    get;
        //    private set;
        //}

        internal Dictionary<string, IObjectChunkLayer> ObjectChunksLayers
        {
            get;
            private set;
        }

        internal Dictionary<string, IObjectStructureTemplate> ObjectStructureTemplates
        {
            get;
            private set;
        }

        internal int NbCaseSide
        {
            get;
            private set;
        }

        internal int WorldSeed
        {
            get;
            private set;
        }

        internal int MaxObjectChunkLayerMargin
        {
            get;
            private set;
        }

        internal ObjectChunkLayersMonitor(DataChunkLayersMonitor dataChunkLayersMonitor, int nbCaseSide, int worldSeed)
        {
            this.WorldSeed = worldSeed;
            this.NbCaseSide = nbCaseSide;

            this.MaxObjectChunkLayerMargin = 0;

            this.WorldObjectLayers = new List<IObjectChunkLayer>();
            this.ObjectChunksLayers = new Dictionary<string, IObjectChunkLayer>();

            this.ObjectStructureTemplates = new Dictionary<string, IObjectStructureTemplate>();

            this.DataChunkMonitor = dataChunkLayersMonitor;

            //this.ObjectStructureManager = new ObjectStructureManager();
        }

        internal void AddObjectLayerToGenerator(IObjectChunkLayer objectChunkLayerToAdd)
        {
            this.WorldObjectLayers.Add(objectChunkLayerToAdd);
            this.ObjectChunksLayers.Add(objectChunkLayerToAdd.Id, objectChunkLayerToAdd);

            objectChunkLayerToAdd.InitObjectChunkLayer(this.NbCaseSide);

            this.MaxObjectChunkLayerMargin = Math.Max(this.MaxObjectChunkLayerMargin, objectChunkLayerToAdd.ObjectChunkMargin);
        }

        internal void AddObjectStructureTemplatesToGenerator(IObjectStructureTemplate structureTemplateToAdd)
        {
            this.ObjectStructureTemplates.Add(structureTemplateToAdd.TemplateUID, structureTemplateToAdd);
        }

        //private float timeMean = 0;
        //private int nb = 0;

        internal IObjectChunk GenerateChunkAt(Vector2i position)
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

            foreach (IObjectChunkLayer objectChunkLayer in this.WorldObjectLayers)
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

            ObjectChunk objectChunk = new ObjectChunk(position, this.NbCaseSide, nbAltitudeLevel);
            objectChunk.ParentMonitor = this;

            return objectChunk;
        }
    }
}
