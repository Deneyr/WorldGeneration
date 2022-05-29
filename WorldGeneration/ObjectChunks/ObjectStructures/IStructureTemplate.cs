using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    internal interface IStructureTemplate
    {
        string TemplateUID
        {
            get;
        }

        IObjectStructure GenerateStructureAtWorldPosition(Random random, Vector2i worldPosition, int worldAltitude, IObjectChunk objectChunk);

        bool IsGenerationValidAtWorldPosition(Vector2i worldPosition, int worldAltitude, IObjectChunk objectChunk);
    }
}
