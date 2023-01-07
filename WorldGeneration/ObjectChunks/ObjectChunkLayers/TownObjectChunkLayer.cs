using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class TownObjectChunkLayer: AObjectStructureChunkLayer
    {
        private TownDataAgreggator townDataAgreggator;

        public TownObjectChunkLayer(string id)
            : base(id)
        {
        }

        protected override IObjectStructure ConstructObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk, Random random, IDataStructure dataStructure)
        {
            IObjectStructureTemplate townStructureTemplate = objectChunksMonitor.ObjectStructureTemplates[dataStructure.ObjectStructureTemplateId];

            return townStructureTemplate.GenerateStructureAtWorldPosition(objectChunksMonitor, random, dataStructure, 0, objectChunk);
        }

        protected override List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            return this.townDataAgreggator.GetDataStructuresInWorldArea(worldArea);
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.townDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["town"] as TownDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }
    }
}
