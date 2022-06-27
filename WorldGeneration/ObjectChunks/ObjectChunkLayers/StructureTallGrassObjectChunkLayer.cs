using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class StructureTallGrassObjectChunkLayer : AObjectStructureChunkLayer
    {
        private TallGrassDataAgreggator tallGrassDataAgreggator;

        public StructureTallGrassObjectChunkLayer(string id)
            : base(id)
        {
        }

        protected override IObjectStructure ConstructObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk, Random random, IDataStructure dataStructure)
        {
            IObjectStructureTemplate tallGrassStructureTemplate = objectChunksMonitor.ObjectStructureTemplates[dataStructure.ObjectStructureTemplateId];

            return tallGrassStructureTemplate.GenerateStructureAtWorldPosition(random, dataStructure, 0, objectChunk);
        }

        protected override List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            return this.tallGrassDataAgreggator.GetDataStructuresInWorldArea(worldArea);
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            //int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            //Random random = new Random(chunkSeed);

            this.tallGrassDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["tallGrass"] as TallGrassDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);

            //for (int i = 0; i < objectChunk.NbCaseSide; i++)
            //{
            //    for (int j = 0; j < objectChunk.NbCaseSide; j++)
            //    {
            //        IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(j, i) as IZObjectCase;

            //        if (zObjectCase.GroundAltitude >= 0)
            //        {
            //            ObjectCase objectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;

            //            if (objectCase.IsUnderSea == false)
            //            {
            //                if (objectCase.IsThereTree == false
            //                    && objectCase.IsThereRock == false)
            //                {
            //                    objectCase.IsThereTallGrass = tallGrassDataAgreggator.IsThereTallGrassAtWorldCoordinates(zObjectCase.Position.X, zObjectCase.Position.Y);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        //protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        //{
        //    //IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

        //    //zObjectCase.ObjectBiome = (BiomeType)this.areaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin];
        //}
    }
}
