using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ObjectChunks
{
    public class TestChunk: IObjectChunk
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

        public TestChunk(Vector2i position, int nbCaseSide)
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

        public bool GenerateChunk(ObjectChunkLayersMonitor dataChunksMonitor, IObjectChunkLayer parentLayer)
        {
            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position.X, this.Position.Y, j, i));
                    this.CasesArray[i, j] = this.GenerateCaseAtWorldCoordinates(dataChunksMonitor, worldPosition);
                }
            }

            return true;
        }

        public ICase GenerateCaseAtWorldCoordinates(ObjectChunkLayersMonitor dataChunksMonitor, Vector2i position)
        {
            return dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscape"].GetCaseAtWorldCoordinates(position.X, position.Y);
        }
    }
}
