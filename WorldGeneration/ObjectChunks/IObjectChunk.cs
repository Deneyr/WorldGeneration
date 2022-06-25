using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks
{
    public interface IObjectChunk : IChunk
    {
        IZObjectCase[,] ZCasesArray
        {
            get;
        }

        HashSet<Type> TypesInChunk
        {
            get;
        }

        void RegisterObjectStructure(IObjectStructure objectStructureToRegister);

        IObjectStructure GetObjectStructure(string uid);
    }
}
