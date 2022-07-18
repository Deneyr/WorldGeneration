using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal abstract class AStructureDataAgreggator: IDataAgreggator
    {
        public List<APointDataStructureChunkLayer> DataStructureChunkLayers
        {
            get;
            set;
        }

        public AStructureDataAgreggator()
        {
            this.DataStructureChunkLayers = new List<APointDataStructureChunkLayer>();
        }

        public virtual List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea)
        {
            List<IDataStructure> resultDataStructures = new List<IDataStructure>();

            foreach (APointDataStructureChunkLayer dataStructureChunkLayer in this.DataStructureChunkLayers)
            {
                resultDataStructures.AddRange(dataStructureChunkLayer.GetDataStructuresInWorldArea(worldArea));
            }

            return resultDataStructures;
        }

        public void AddDataStructureChunkLayer(APointDataStructureChunkLayer dataStructureChunkLayer)
        {
            this.DataStructureChunkLayers.Add(dataStructureChunkLayer);
        }
    }
}
