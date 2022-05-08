using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise
{
    internal abstract class AStructureDataChunk : IDataChunk
    {
        private IntRect structDimension;

        private int nbMinDataStructure;
        private int nbMaxDataStructure;

        private int nbStructures;
        private int nbStructureCells;
        private int nbCaseStructureCell;

        protected IDataStructure[,] dataStructures;

        //public List<IDataStructure> DataStructureList
        //{
        //    get;
        //    private set;
        //}

        //public List<IDataStructure> BorderDataStructureList
        //{
        //    get;
        //    private set;
        //}

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

        public AStructureDataChunk(Vector2i position, int nbCaseSide, int nbMinDataStructure, int nbMaxDataStructure, IntRect structDimension)
        {
            this.Position = position;
            this.NbCaseSide = nbCaseSide;

            this.nbMinDataStructure = nbMinDataStructure;
            this.nbMaxDataStructure = nbMaxDataStructure;

            this.structDimension = structDimension;

            //this.DataStructureList = new List<IDataStructure>();
            //this.BorderDataStructureList = new List<IDataStructure>();
        }

        private void InitDataStructureArray(Random random)
        {
            this.nbStructures = random.Next(this.nbMinDataStructure, this.nbMaxDataStructure + 1);

            int minSideCells = (int) Math.Ceiling(Math.Sqrt(this.nbStructures));
            int maxSideCells = this.NbCaseSide / Math.Max(this.structDimension.Width, this.structDimension.Height);

            this.nbStructureCells = random.Next(minSideCells, maxSideCells + 1);
            this.nbCaseStructureCell = this.NbCaseSide / this.nbStructureCells;

            this.dataStructures = new IDataStructure[this.nbStructureCells, this.nbStructureCells];
            for(int i = 0; i < this.nbStructureCells; i++)
            {
                for (int j = 0; j < this.nbStructureCells; j++)
                {
                    this.dataStructures[i, j] = null;
                }
            }
        }

        public virtual void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            this.InitDataStructureArray(random);

            List<Vector2i> cellList = new List<Vector2i>();
            for (int i = 0; i < this.nbStructureCells; i++)
            {
                for (int j = 0; j < this.nbStructureCells; j++)
                {
                    cellList.Add(new Vector2i(i, j));
                }
            }
            
            for(int i = 0; i < this.nbStructures; i++)
            {
                int listIndex = random.Next(0, cellList.Count);
                Vector2i cellCoordinate = cellList[listIndex];

                this.dataStructures[cellCoordinate.X, cellCoordinate.Y] = this.GenerateDataStructure(random, dataChunksMonitor, new Vector2i(cellCoordinate.Y * this.nbCaseStructureCell, cellCoordinate.X * this.nbCaseStructureCell));
                cellList.RemoveAt(listIndex);
            }
        }

        private IDataStructure GenerateDataStructure(Random random, DataChunkLayersMonitor dataChunksMonitor, Vector2i cellChunkCoordinate)
        {
            int width = random.Next(this.structDimension.Left, this.structDimension.Width + 1);
            int height = random.Next(this.structDimension.Top, this.structDimension.Height + 1);

            int x = random.Next(cellChunkCoordinate.X, cellChunkCoordinate.X + this.nbCaseStructureCell - width);
            int y = random.Next(cellChunkCoordinate.Y, cellChunkCoordinate.Y + this.nbCaseStructureCell - height);

            IntRect structureDim = new IntRect(x, y, width, height);
            Vector2i structureCenter = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position.X, this.Position.Y, x + width / 2, y + height / 2));

            IDataStructure dataStructure = this.CreateDataStructure(random, dataChunksMonitor, new IntRect(x, y, width, height), structureCenter);

            if (dataStructure != null)
            {
                dataStructure.GenerateStructure(random, null);
            }

            return dataStructure;
        }

        protected abstract IDataStructure CreateDataStructure(Random random, DataChunkLayersMonitor dataChunksMonitor, IntRect boundingBox, Vector2i structureCenter);

        public virtual void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            // Nothing to do
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            int cellCoordinateX = x / this.nbCaseStructureCell;
            int cellCoordinateY = y / this.nbCaseStructureCell;

            if (cellCoordinateX < this.nbStructureCells && cellCoordinateY < this.nbStructureCells)
            {
                IDataStructure dataStructure = this.dataStructures[cellCoordinateY, cellCoordinateX];

                if (dataStructure != null)
                {
                    return dataStructure.GetStructureCaseAtChunkCoordinate(x, y);
                }
            }

            return null;
        }

        protected virtual int GenerateChunkSeed(int seed)
        {
            return this.Position.X * this.Position.Y * seed - seed + this.NbCaseSide - this.Position.X + this.Position.Y * this.Position.Y;
        }
    }
}
