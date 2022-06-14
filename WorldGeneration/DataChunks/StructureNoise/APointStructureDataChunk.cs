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
    internal abstract class APointStructureDataChunk : IDataChunk
    {
        private IntRect structDimension;

        private int nbMinDataStructure;
        private int nbMaxDataStructure;

        private int nbStructures;
        private int nbCaseCell;
        private int nbCellSide;

        internal List<IDataStructure>[,] PreviousDataStructures
        {
            get;
            private set;
        }

        internal List<IDataStructure>[,] DataStructures
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

        public APointStructureDataChunk(Vector2i position, int nbCaseSide, int nbMinDataStructure, int nbMaxDataStructure, IntRect structDimension)
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

            //int minSideCells = (int) Math.Ceiling(Math.Sqrt(this.nbStructures));
            //int maxSideCells = this.NbCaseSide / Math.Max(this.structDimension.Width, this.structDimension.Height);

            //this.nbStructureCells = random.Next(minSideCells, maxSideCells + 1);
            //this.nbCaseStructureCell = this.NbCaseSide / this.nbStructureCells;

            this.nbCellSide = this.NbCaseSide / Math.Max(this.structDimension.Width, this.structDimension.Height);
            this.nbCaseCell = this.NbCaseSide / nbCellSide;

            this.PreviousDataStructures = new List<IDataStructure>[this.nbCellSide, this.nbCellSide];
            this.DataStructures = new List<IDataStructure>[this.nbCellSide, this.nbCellSide];
            for (int i = 0; i < this.nbCellSide; i++)
            {
                for (int j = 0; j < this.nbCellSide; j++)
                {
                    this.PreviousDataStructures[i, j] = null;
                    this.DataStructures[i, j] = null;
                }
            }
        }

        private static Dictionary<string, int> testDico = new Dictionary<string, int>();

        [Obsolete]
        private static void SetTestDicoValue(APointStructureDataChunk chunk, int value)
        {
            string key = chunk.Position.X + ":" + chunk.Position.Y;

            if(testDico.TryGetValue(key, out int ancientValue))
            {
                if(ancientValue != value)
                {
                    Console.WriteLine("Casse !!");
                }
            }
            else
            {
                testDico[key] = value;
            }
        }

        public virtual void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            this.InitDataStructureArray(random);

            for (int i = 0; i < this.nbStructures; i++)
            {
                int x = random.Next(0, this.NbCaseSide);
                int y = random.Next(0, this.NbCaseSide);

                int xCase = Math.Min(this.nbCellSide - 1, x / this.nbCaseCell);
                int yCase = Math.Min(this.nbCellSide - 1, y / this.nbCaseCell);

                IDataStructure newDataStructure = this.GenerateDataStructure(random, dataChunksMonitor, parentLayer, new Vector2i(x, y));

                List<IDataStructure> listDataStructures = this.PreviousDataStructures[yCase, xCase];
                if (listDataStructures == null && newDataStructure != null)
                {
                    listDataStructures = new List<IDataStructure>();
                    this.PreviousDataStructures[yCase, xCase] = listDataStructures;
                }

                if(listDataStructures != null)
                {
                    listDataStructures.Add(newDataStructure);
                }
            }

            //List<Vector2i> cellList = new List<Vector2i>();
            //for (int i = 0; i < this.nbStructureCells; i++)
            //{
            //    for (int j = 0; j < this.nbStructureCells; j++)
            //    {
            //        cellList.Add(new Vector2i(i, j));
            //    }
            //}

            //for(int i = 0; i < this.nbStructures; i++)
            //{
            //    int listIndex = random.Next(0, cellList.Count);
            //    Vector2i cellCoordinate = cellList[listIndex];

            //    this.dataStructures[cellCoordinate.X, cellCoordinate.Y] = this.GenerateDataStructure(random, dataChunksMonitor, new Vector2i(cellCoordinate.Y * this.nbCaseStructureCell, cellCoordinate.X * this.nbCaseStructureCell));
            //    cellList.RemoveAt(listIndex);
            //}
        }

        private IDataStructure GenerateDataStructure(Random random, DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, Vector2i structureCoordinates)
        {
            int width = random.Next(this.structDimension.Left, this.structDimension.Width + 1);
            int height = random.Next(this.structDimension.Top, this.structDimension.Height + 1);

            IntRect structureDim = new IntRect(structureCoordinates.X, structureCoordinates.Y, width, height);
            Vector2i structureWorldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position.X, this.Position.Y, structureCoordinates.X, structureCoordinates.Y));

            IDataStructure dataStructure = this.CreateDataStructure(random, dataChunksMonitor, structureDim, new IntRect(0, 0, width, height), structureWorldPosition);

            return dataStructure;
        }

        protected abstract IDataStructure CreateDataStructure(Random random, DataChunkLayersMonitor dataChunksMonitor, IntRect boundingBox, IntRect baseBoundingBox, Vector2i structureWorldPosition);

        public virtual void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed - parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            List<IDataStructure> nearbyDataStructures = new List<IDataStructure>();
            for (int i = 0; i < this.nbCellSide; i++)
            {
                for (int j = 0; j < this.nbCellSide; j++)
                {
                    List<IDataStructure> dataStructuresHere = this.PreviousDataStructures[i, j];

                    if (dataStructuresHere != null)
                    {
                        List<IDataStructure> remainingDataStructure = new List<IDataStructure>();

                        this.AddNearbyDataStructuresAt(parentLayer, j - 1, i, nearbyDataStructures);
                        this.AddNearbyDataStructuresAt(parentLayer, j, i - 1, nearbyDataStructures);
                        this.AddNearbyDataStructuresAt(parentLayer, j - 1, i - 1, nearbyDataStructures);

                        List<IDataStructure>.Enumerator dataStructuresHereEnum = dataStructuresHere.GetEnumerator();

                        while (dataStructuresHereEnum.MoveNext())
                        {
                            IDataStructure currentDataStructure = dataStructuresHereEnum.Current;
                            IntRect currentWorldBoundingBox = currentDataStructure.StructureWorldBaseBoundingBox;

                            bool isColliding = false;
                            foreach (IDataStructure nearbyDataStructure in nearbyDataStructures)
                            {
                                if (currentWorldBoundingBox.Intersects(nearbyDataStructure.StructureWorldBaseBoundingBox))
                                {
                                    isColliding = true;
                                    break;
                                }
                            }

                            if (isColliding == false)
                            {
                                List<IDataStructure>.Enumerator nextDataStructureEnum = dataStructuresHereEnum;
                                while (nextDataStructureEnum.MoveNext() && isColliding == false)
                                {
                                    IDataStructure nextDataStructure = nextDataStructureEnum.Current;

                                    isColliding = currentWorldBoundingBox.Intersects(nextDataStructure.StructureWorldBaseBoundingBox);
                                }
                            }

                            if (isColliding == false && this.IsDataStructureValid(random, dataChunksMonitor, currentDataStructure))
                            {
                                remainingDataStructure.Add(currentDataStructure);
                            }
                        }

                        this.DataStructures[i, j] = remainingDataStructure;

                        foreach(IDataStructure dataStructure in remainingDataStructure)
                        {
                            dataStructure.GenerateStructure(random, null);
                        }

                        nearbyDataStructures.Clear();
                    }
                }
            }

            if (parentLayer.Id.Contains("second") == false)
            {
                SetTestDicoValue(this, random.Next());
            }
        }

        protected virtual bool IsDataStructureValid(Random random, DataChunkLayersMonitor dataChunksMonitor, IDataStructure dataStructure)
        {
            return true;
        }

        protected void AddNearbyDataStructuresAt(IDataChunkLayer parentLayer, int x, int y, List<IDataStructure> nearbyDataStructures)
        {
            int xChunk = this.Position.X;
            int yChunk = this.Position.Y;

            bool isOtherChunk = false;
            if(x < 0)
            {
                xChunk--;
                x = this.nbCellSide - 1;
                isOtherChunk = true;
            }

            if (y < 0)
            {
                yChunk--;
                y = this.nbCellSide - 1;
                isOtherChunk = true;
            }

            ChunkContainer nearbyChunkContainer = (parentLayer as AExtendedDataChunkLayer).ExtendedChunksMonitor.GetChunkContainerAt(xChunk, yChunk);

            if (nearbyChunkContainer != null && nearbyChunkContainer.ContainedChunk != null)
            {
                APointStructureDataChunk pointStructureDataChunk = nearbyChunkContainer.ContainedChunk as APointStructureDataChunk;

                List<IDataStructure> currentDataStructures;
                if (isOtherChunk)
                {
                    currentDataStructures = pointStructureDataChunk.PreviousDataStructures[y, x];
                }
                else
                {
                    currentDataStructures = pointStructureDataChunk.DataStructures[y, x];
                }

                //currentDataStructures = pointStructureDataChunk.PreviousDataStructures[y, x];

                if (currentDataStructures != null && currentDataStructures.Any())
                {
                    nearbyDataStructures.AddRange(currentDataStructures);
                }
            }

        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            int cellCoordinateX = Math.Min(this.nbCellSide - 1, x / this.nbCaseCell);
            int cellCoordinateY = Math.Min(this.nbCellSide - 1, y / this.nbCaseCell);

            if (cellCoordinateX < this.nbCellSide && cellCoordinateY < this.nbCellSide)
            {
                List<IDataStructure> dataStructures = this.DataStructures[cellCoordinateY, cellCoordinateX];
                return this.GetCaseFromDataStructuresList(dataStructures, x, y);
            }

            return null;
        }

        public void AddDataStructuresFromWorldArea(IntRect worldArea, List<IDataStructure> dataStructures)
        {
            IntRect chunkWorldArea = ChunkHelper.GetWorldAreaFromChunkArea(this.NbCaseSide, new IntRect(this.Position.X, this.Position.Y, 1, 1));

            if (worldArea.Intersects(chunkWorldArea, out IntRect overlapingArea))
            {
                IntRect startChunkPosition = ChunkHelper.GetChunkPositionFromWorldPosition(this.NbCaseSide, new Vector2i(overlapingArea.Left, overlapingArea.Top));
                IntRect endChunkPosition = ChunkHelper.GetChunkPositionFromWorldPosition(this.NbCaseSide, new Vector2i(overlapingArea.Left + overlapingArea.Width - 1, overlapingArea.Top + overlapingArea.Height - 1));

                Vector2i startCellCoordinate = new Vector2i(Math.Min(startChunkPosition.Width / this.nbCaseCell, this.nbCellSide - 1), Math.Min(startChunkPosition.Height / this.nbCaseCell, this.nbCellSide - 1));
                Vector2i endCellCoordinate = new Vector2i(Math.Min(endChunkPosition.Width / this.nbCaseCell, this.nbCellSide - 1), Math.Min(endChunkPosition.Height / this.nbCaseCell, this.nbCellSide - 1));

                //if(startChunkPosition.Left != this.Position.X
                //    || startChunkPosition.Top != this.Position.Y)
                //{
                //    Console.WriteLine();
                //}
                //if (endChunkPosition.Left != this.Position.X
                //    || endChunkPosition.Top != this.Position.Y)
                //{
                //    Console.WriteLine();
                //}

                for (int i = startCellCoordinate.Y; i <= endCellCoordinate.Y; i++)
                {
                    for (int j = startCellCoordinate.X; j <= endCellCoordinate.X; j++)
                    {
                        List<IDataStructure> currentDataStructures = this.DataStructures[i, j];
                        if (currentDataStructures != null && currentDataStructures.Any())
                        {
                            dataStructures.AddRange(currentDataStructures);
                        }
                    }
                }
            }
        }

        //private static bool GetIntersectionBetween(IntRect intRect1, IntRect intRect2, out IntRect overlapingArea)
        //{
        //    intRect1.Width = intRect1.Left + intRect1.Width;
        //    intRect1.Height = intRect1.Top + intRect1.Height;

        //    intRect2.Width = intRect2.Left + intRect2.Width;
        //    intRect2.Height = intRect2.Top + intRect2.Height;

        //    overlapingArea = new IntRect();
        //    if (intRect1.Intersects(intRect2))
        //    {
        //        overlapingArea.Left
        //    }
        //}

        [Obsolete]
        public ICase GetCaseAtLocal(IDataChunkLayer parentLayer, int x, int y)
        {
            int cellCoordinateX = Math.Min(this.nbCellSide - 1, x / this.nbCaseCell);
            int cellCoordinateY = Math.Min(this.nbCellSide - 1, y / this.nbCaseCell);

            if (cellCoordinateX < this.nbCellSide && cellCoordinateY < this.nbCellSide)
            {
                List<IDataStructure> dataStructures = this.DataStructures[cellCoordinateY, cellCoordinateX];

                ICase resultCase = this.GetCaseFromDataStructuresList(dataStructures, x, y);

                if(resultCase == null)
                {
                    resultCase = this.GetCaseAtLocalNearbyCell(parentLayer, cellCoordinateX - 1, cellCoordinateY, x, y);
                }
                if (resultCase == null)
                {
                    resultCase = this.GetCaseAtLocalNearbyCell(parentLayer, cellCoordinateX, cellCoordinateY - 1, x, y);
                }
                if (resultCase == null)
                {
                    resultCase = this.GetCaseAtLocalNearbyCell(parentLayer, cellCoordinateX - 1, cellCoordinateY - 1, x, y);
                }
                return resultCase;
            }

            return null;
        }

        private ICase GetCaseFromDataStructuresList(List<IDataStructure> dataStructures, int x, int y)
        {
            if (dataStructures == null || dataStructures.Any() == false)
            {
                return null;
            }

            ICase resultCase = null;
            foreach (IDataStructure dataStructure in dataStructures)
            {
                resultCase = dataStructure.GetStructureCaseAtChunkCoordinate(x, y);
                if (resultCase != null)
                {
                    return resultCase;
                }
            }
            return resultCase;
        }

        private ICase GetCaseAtLocalNearbyCell(IDataChunkLayer parentLayer, int cellCoordinateX, int cellCoordinateY, int x, int y)
        {
            int xChunk = this.Position.X;
            int yChunk = this.Position.Y;

            if (cellCoordinateX < 0)
            {
                xChunk--;
                cellCoordinateX = this.nbCellSide - 1;
                x += this.NbCaseSide;
            }

            if (cellCoordinateY < 0)
            {
                yChunk--;
                cellCoordinateY = this.nbCellSide - 1;
                y += this.NbCaseSide;
            }

            ChunkContainer nearbyChunkContainer = (parentLayer as AExtendedDataChunkLayer).ExtendedChunksMonitor.GetChunkContainerAt(xChunk, yChunk);

            if (nearbyChunkContainer != null && nearbyChunkContainer.ContainedChunk != null)
            {
                APointStructureDataChunk pointStructureDataChunk = nearbyChunkContainer.ContainedChunk as APointStructureDataChunk;

                List<IDataStructure> currentDataStructures = pointStructureDataChunk.DataStructures[cellCoordinateY, cellCoordinateX];
                return this.GetCaseFromDataStructuresList(currentDataStructures, x, y);
            }

            return null;
        }

        protected virtual int GenerateChunkSeed(int seed)
        {
            return this.Position.X * this.Position.Y * seed - seed + this.NbCaseSide - this.Position.X + this.Position.Y * this.Position.Y;
        }
    }
}
