using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration.DataChunks
{
    internal abstract class ADataChunk : IDataChunk
    {
        public ICase[,] CasesArray
        {
            get;
            private set;
        }

        public Vector2i Position
        {
            get;
            private set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        //protected List<Vector2i> notGeneratedCases;

        public ADataChunk(Vector2i position, int nbCaseSide)
        {
            this.Position = position;
            this.NbCaseSide = nbCaseSide;

            //this.notGeneratedCases = null;

            this.CasesArray = new ICase[this.NbCaseSide, this.NbCaseSide];
            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    this.CasesArray[i, j] = null;
                }
            }
        }

        public abstract void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer);

        public virtual void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed - parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            //this.notGeneratedCases = new List<Vector2i>();

            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    ICase generatedCase = this.GenerateCase(dataChunksMonitor, parentLayer, j, i, random);

                    this.CasesArray[i, j] = generatedCase;
                }
            }
        }

        //public bool EndGenerateChunk(DataChunkLayersMonitor dataChunksMonitor, ADataChunkLayer parentLayer)
        //{
        //    if(this.notGeneratedCases != null && this.notGeneratedCases.Count > 0)
        //    {
        //        int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed - parentLayer.Id.GetHashCode());
        //        Random random = new Random(chunkSeed);

        //        List<Vector2i> remainingNotGeneratedCases = new List<Vector2i>();
        //        foreach(Vector2i coordinateToGenerate in this.notGeneratedCases)
        //        {
        //            ICase generatedCase = this.GenerateCase(dataChunksMonitor, parentLayer, coordinateToGenerate.X, coordinateToGenerate.Y, random);

        //            if (generatedCase != null)
        //            {
        //                this.CasesArray[coordinateToGenerate.Y, coordinateToGenerate.X] = generatedCase;
        //            }
        //            else
        //            {
        //                this.notGeneratedCases.Add(coordinateToGenerate);
        //            }
        //        }
        //        this.notGeneratedCases = remainingNotGeneratedCases;
        //    }

        //    return this.notGeneratedCases.Count == 0;
        //}

        protected abstract ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random);

        protected virtual int GenerateChunkSeed(int seed)
        {
            return this.Position.X * this.Position.Y * seed + seed + this.NbCaseSide + this.Position.X + this.Position.Y;
        }
    }
}
