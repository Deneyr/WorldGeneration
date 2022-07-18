using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class TreeObjectChunkLayer : AObjectStructureChunkLayer
    {
        private TreeDataAgreggator treeDataAgreggator;

        public override int ObjectChunkMargin
        {
            get
            {
                return 2;
            }
        }

        public TreeObjectChunkLayer(string id)
            : base(id)
        {
        }

        protected override IObjectStructure ConstructObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk, Random random, IDataStructure dataStructure)
        {
            if (this.IsStructureBaseValid(objectChunksMonitor, objectChunk, random, dataStructure, out int structureAltitude))
            {
                IObjectStructureTemplate treeStructureTemplate = objectChunksMonitor.ObjectStructureTemplates[dataStructure.ObjectStructureTemplateId];

                return treeStructureTemplate.GenerateStructureAtWorldPosition(random, dataStructure, structureAltitude, objectChunk);
            }
            return null;
        }

        protected override List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            return this.treeDataAgreggator.GetDataStructuresInWorldArea(worldArea);
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.treeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["tree"] as TreeDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }
    }
}