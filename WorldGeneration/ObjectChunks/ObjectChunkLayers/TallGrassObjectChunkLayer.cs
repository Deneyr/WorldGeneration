using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.Maths.RandomHelpers;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class TallGrassObjectChunkLayer : AObjectStructureChunkLayer
    {
        private TallGrassDataAgreggator tallGrassDataAgreggator;

        public TallGrassObjectChunkLayer(string id)
            : base(id)
        {
        }

        protected override IObjectStructure ConstructObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk, WGRandom random, IDataStructure dataStructure)
        {
            IObjectStructureTemplate tallGrassStructureTemplate = objectChunksMonitor.ObjectStructureTemplates[dataStructure.ObjectStructureTemplateId];

            return tallGrassStructureTemplate.GenerateStructureAtWorldPosition(objectChunksMonitor, random, dataStructure, 0, objectChunk);
        }

        protected override List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            return this.tallGrassDataAgreggator.GetDataStructuresInWorldArea(worldArea);
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.tallGrassDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["tallGrass"] as TallGrassDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }
    }
}
