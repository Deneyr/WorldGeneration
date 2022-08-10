using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    internal interface IObjectStructureTemplate
    {
        string TemplateUID
        {
            get;
        }

        IObjectStructure GenerateStructureAtWorldPosition(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IDataStructure dataStructure, int worldAltitude, IObjectChunk objectChunk);

        bool IsGenerationValidAtWorldPosition(ObjectChunkLayersMonitor objectChunksMonitor, IDataStructure dataStructure, int worldAltitude, IObjectChunk objectChunk);
    }
}
