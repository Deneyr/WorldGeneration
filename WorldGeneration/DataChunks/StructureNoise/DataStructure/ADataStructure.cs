﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.DataChunks.StructureNoise.TallGrassStructure;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal abstract class ADataStructure : IDataStructure
    {
        protected DataStructureCaseType[,] dataStructureCasesType;

        protected int[] leftBorderIndexes;
        protected int[] rightBorderIndexes;
        protected int[] topBorderIndexes;
        protected int[] botBorderIndexes;

        public int StructureTypeIndex
        {
            get;
            internal set;
        }

        public string ObjectStructureTemplateId
        {
            get;
            internal set;
        }

        public IDataStructureCase[,] DataStructureCases
        {
            get;
            private set;
        }

        public Vector2i StructureWorldPosition
        {
            get;
            private set;
        }

        public Vector2i StructureWorldCenter
        {
            get
            {
                return new Vector2i(this.StructureWorldPosition.X + this.StructureBoundingBox.Width / 2, this.StructureWorldPosition.Y + this.StructureBoundingBox.Height / 2);
            }
        }

        public BiomeType StructureBiome
        {
            get;
            internal set;
        }

        public IntRect StructureBoundingBox
        {
            get;
            private set;
        }

        public IntRect StructureBaseBoundingBox
        {
            get;
            private set;
        }

        public IntRect StructureWorldBoundingBox
        {
            get
            {
                return new IntRect(
                    this.StructureWorldPosition.X,
                    this.StructureWorldPosition.Y,
                    this.StructureBoundingBox.Width,
                    this.StructureBoundingBox.Height);
            }
        }

        public IntRect StructureWorldBaseBoundingBox
        {
            get
            {
                return new IntRect(
                    this.StructureWorldPosition.X + this.StructureBaseBoundingBox.Left,
                    this.StructureWorldPosition.Y + this.StructureBaseBoundingBox.Top,
                    this.StructureBaseBoundingBox.Width,
                    this.StructureBaseBoundingBox.Height);
            }
        }

        public ADataStructure(Vector2i structureWorldPosition, IntRect structureBoundingBox, IntRect structureBaseBoundingBox)
        {
            this.StructureWorldPosition = structureWorldPosition;
            this.StructureBiome = BiomeType.BOREAL_FOREST;

            this.StructureBoundingBox = structureBoundingBox;

            this.DataStructureCases = new IDataStructureCase[structureBoundingBox.Height, structureBoundingBox.Width];
            this.dataStructureCasesType = new DataStructureCaseType[structureBoundingBox.Height, structureBoundingBox.Width];
            this.leftBorderIndexes = new int[structureBoundingBox.Height];
            this.rightBorderIndexes = new int[structureBoundingBox.Height];
            this.topBorderIndexes = new int[structureBoundingBox.Width];
            this.botBorderIndexes = new int[structureBoundingBox.Width];

            this.StructureBaseBoundingBox = structureBaseBoundingBox;
        }

        public abstract void GenerateStructure(Random random, IDataStructureTemplate structureTemplate);

        protected virtual void InitializeDataStructureCases()
        {
            int height = this.DataStructureCases.GetLength(0);
            int width = this.DataStructureCases.GetLength(1);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    this.dataStructureCasesType[i, j] = DataStructureCaseType.FILLED_CASE;
                }
            }
        }

        public IDataStructureCase GetStructureCaseAtChunkCoordinate(int x, int y)
        {
            if (this.StructureBoundingBox.Contains(x, y))
            {
                int localX = (x - this.StructureBoundingBox.Left);
                int localY = (y - this.StructureBoundingBox.Top);

                return this.DataStructureCases[localY, localX];
            }
            return null;
        }

        protected virtual void GenerateStructureBoundariesPerlin(Random random, float ratioMargin)
        {
            int height = this.DataStructureCases.GetLength(0);
            int width = this.DataStructureCases.GetLength(1);
            int heightMargin = (int)(height * ratioMargin);
            int widthMargin = (int)(width * ratioMargin);

            PerlinStructureNoise perlinStructureNoise = new PerlinStructureNoise();
            perlinStructureNoise.GenerateStructureNoise(random);

            float ratioX;
            float ratioY;

            // Side Left-Right
            for (int i = 0; i < height; i++)
            {
                ratioY = ((float)i) / height;
                for (int j = 0; j < widthMargin * 2; j++)
                {
                    if (j <= i
                        && j <= (height - i - 1))
                    {
                        ratioX = ((float)j) / width;

                        float ratioMarginJ = (j - widthMargin) / ((float)widthMargin);

                        ratioMarginJ = perlinStructureNoise.GetValueAt(ratioX, ratioY) + ratioMarginJ;

                        if (ratioMarginJ < 0)
                        {
                            this.DataStructureCases[i, j] = null;
                        }
                    }
                }
            }

            for (int i = 0; i < height; i++)
            {
                ratioY = ((float)i) / height;
                for (int j = 0; j < widthMargin * 2; j++)
                {
                    if (j <= i
                        && j <= (height - i - 1))
                    {
                        ratioX = ((float)(width - j - 1)) / width;

                        float ratioMarginJ = (j - widthMargin) / ((float)widthMargin);

                        ratioMarginJ = perlinStructureNoise.GetValueAt(ratioX, ratioY) + ratioMarginJ;

                        if (ratioMarginJ < 0)
                        {
                            this.DataStructureCases[i, width - j - 1] = null;
                        }
                    }
                }
            }

            // Side Top-Bot
            for (int j = 0; j < width; j++)
            {
                ratioX = ((float)j) / width;
                for (int i = 0; i < heightMargin * 2; i++)
                {
                    if (i < j
                        && i < (width - j - 1))
                    {
                        ratioY = ((float)i) / height;

                        float ratioMarginI = (i - heightMargin) / ((float)heightMargin);

                        ratioMarginI = perlinStructureNoise.GetValueAt(ratioX, ratioY) + ratioMarginI;

                        if (ratioMarginI < 0)
                        {
                            this.DataStructureCases[i, j] = null;
                        }
                    }
                }
            }

            for (int j = 0; j < width; j++)
            {
                ratioX = ((float)j) / width;
                for (int i = 0; i < heightMargin * 2; i++)
                {
                    if (i < j
                        && i < (width - j - 1))
                    {
                        ratioY = ((float)(height - i - 1)) / height;

                        float ratioMarginI = (i - heightMargin) / ((float)heightMargin);

                        ratioMarginI = perlinStructureNoise.GetValueAt(ratioX, ratioY) + ratioMarginI;

                        if (ratioMarginI < 0)
                        {
                            this.DataStructureCases[height - i - 1, j] = null;
                        }
                    }
                }
            }

        }

        protected virtual void GenerateStructureBoundaries(Random random, int margin, int space)
        {
            int height = this.DataStructureCases.GetLength(0);
            int width = this.DataStructureCases.GetLength(1);
            //int heightMargin = (int)(height * ratioMargin);
            //int widthMargin = (int)(width * ratioMargin);

            //int margin = Math.Min(heightMargin, widthMargin);

            int topLeftValue = random.Next(1, margin + 1);
            int botLeftValue = random.Next(1, margin + 1);
            int botRightValue = random.Next(1, margin + 1);
            int topRightValue = random.Next(1, margin + 1);

            // Side Left-Right
            int midHeight = height / 2;
            int topIndex = 0;
            int botIndex = 0;
            for (int i = 0; i < midHeight; i++)
            {
                float ratio = i / ((float)midHeight);

                if (i <= topLeftValue)
                {
                    topIndex = topLeftValue;
                }
                else
                {
                    topIndex = this.GetNextIndex(random, space, topIndex, botIndex, ratio);

                    topIndex = Math.Min(i - 1, Math.Min(margin, Math.Max(0, topIndex)));
                }

                if (i <= botLeftValue)
                {
                    botIndex = botLeftValue;
                }
                else
                {
                    botIndex = this.GetNextIndex(random, space, botIndex, topIndex, ratio);

                    botIndex = Math.Min(i - 1, Math.Min(margin, Math.Max(0, botIndex)));
                }

                for (int j = 0; j < topIndex; j++)
                {
                    this.dataStructureCasesType[i, j] = DataStructureCaseType.EMPTY_CASE;
                }
                for (int j = 0; j < botIndex; j++)
                {
                    this.dataStructureCasesType[height - i - 1, j] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                if(i >= topLeftValue)
                {
                    this.leftBorderIndexes[i] = Math.Max(0, topIndex - 1);
                }
                else
                {
                    this.leftBorderIndexes[i] = -1;
                }
                if (i >= botLeftValue)
                {
                    this.leftBorderIndexes[height - i - 1] = Math.Max(0, botIndex - 1);
                }
                else
                {
                    this.leftBorderIndexes[height - i - 1] = -1;
                }
            }
            if(height % 2 == 1)
            {      
                int middleIndex = this.GetNextIndex(random, space, topIndex, botIndex, 0f);

                middleIndex = Math.Min(midHeight - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int j = 0; j < middleIndex; j++)
                {
                    this.dataStructureCasesType[midHeight, j] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                this.leftBorderIndexes[midHeight] = Math.Max(0, middleIndex - 1);
            }

            topIndex = 0;
            botIndex = 0;
            for (int i = 0; i < midHeight; i++)
            {
                float ratio = i / ((float)midHeight);

                if (i <= topRightValue)
                {
                    topIndex = topRightValue;
                }
                else
                {
                    topIndex = this.GetNextIndex(random, space, topIndex, botIndex, ratio);

                    topIndex = Math.Min(i - 1, Math.Min(margin, Math.Max(0, topIndex)));
                }

                if (i <= botRightValue)
                {
                    botIndex = botRightValue;
                }
                else
                {
                    botIndex = this.GetNextIndex(random, space, botIndex, topIndex, ratio);

                    botIndex = Math.Min(i - 1, Math.Min(margin, Math.Max(0, botIndex)));
                }

                for (int j = 0; j < topIndex; j++)
                {
                    this.dataStructureCasesType[i, width - j - 1] = DataStructureCaseType.EMPTY_CASE;
                }
                for (int j = 0; j < botIndex; j++)
                {
                    this.dataStructureCasesType[height - i - 1, width - j - 1] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                if (i >= topRightValue)
                {
                    this.rightBorderIndexes[i] = Math.Max(0, topIndex - 1);
                }
                else
                {
                    this.rightBorderIndexes[i] = -1;
                }
                if (i >= botRightValue)
                {
                    this.rightBorderIndexes[height - i - 1] = Math.Max(0, botIndex - 1);
                }
                else
                {
                    this.rightBorderIndexes[height - i - 1] = -1;
                }
            }
            if (height % 2 == 1)
            {
                int middleIndex = this.GetNextIndex(random, space, topIndex, botIndex, 0f);

                middleIndex = Math.Min(midHeight - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int j = 0; j < middleIndex; j++)
                {
                    this.dataStructureCasesType[midHeight, width - j - 1] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                this.rightBorderIndexes[midHeight] = Math.Max(0, middleIndex - 1);
            }

            // Side Top-Bot
            int midWidth = width / 2;
            int leftIndex = 0;
            int rightIndex = 0;
            for (int j = 0; j < midWidth; j++)
            {
                float ratio = j / ((float)midWidth);

                if (j <= topLeftValue)
                {
                    leftIndex = topLeftValue;
                }
                else
                {
                    leftIndex = this.GetNextIndex(random, space, leftIndex, rightIndex, ratio);

                    leftIndex = Math.Min(j - 1, Math.Min(margin, Math.Max(0, leftIndex)));
                }

                if (j <= topRightValue)
                {
                    rightIndex = topRightValue;
                }
                else
                {
                    rightIndex = this.GetNextIndex(random, space, rightIndex, leftIndex, ratio);

                    rightIndex = Math.Min(j - 1, Math.Min(margin, Math.Max(0, rightIndex)));
                }

                for (int i = 0; i < leftIndex; i++)
                {
                    this.dataStructureCasesType[i, j] = DataStructureCaseType.EMPTY_CASE;
                }
                for (int i = 0; i < rightIndex; i++)
                {
                    this.dataStructureCasesType[i, width - j - 1] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                if (j >= topLeftValue)
                {
                    this.topBorderIndexes[j] = Math.Max(0, leftIndex - 1);
                }
                else
                {
                    this.topBorderIndexes[j] = -1;
                }
                if (j >= topRightValue)
                {
                    this.topBorderIndexes[width - j - 1] = Math.Max(0, rightIndex - 1);
                }
                else
                {
                    this.topBorderIndexes[width - j - 1] = -1;
                }
            }
            if (width % 2 == 1)
            {
                int middleIndex = this.GetNextIndex(random, space, leftIndex, rightIndex, 0f);

                middleIndex = Math.Min(midWidth - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int i = 0; i < middleIndex; i++)
                {
                    this.dataStructureCasesType[i, midWidth] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                this.topBorderIndexes[midWidth] = Math.Max(0, middleIndex - 1);
            }

            leftIndex = 0;
            rightIndex = 0;
            for (int j = 0; j < midWidth; j++)
            {
                float ratio = j / ((float)midWidth);

                if (j <= botLeftValue)
                {
                    leftIndex = botLeftValue;
                }
                else
                {
                    leftIndex = this.GetNextIndex(random, space, leftIndex, rightIndex, ratio);

                    leftIndex = Math.Min(j - 1, Math.Min(margin, Math.Max(0, leftIndex)));
                }

                if (j <= botRightValue)
                {
                    rightIndex = botRightValue;
                }
                else
                {
                    rightIndex = this.GetNextIndex(random, space, rightIndex, leftIndex, ratio);

                    rightIndex = Math.Min(j - 1, Math.Min(margin, Math.Max(0, rightIndex)));
                }

                for (int i = 0; i < leftIndex; i++)
                {
                    this.dataStructureCasesType[height - i - 1, j] = DataStructureCaseType.EMPTY_CASE;
                }
                for (int i = 0; i < rightIndex; i++)
                {
                    this.dataStructureCasesType[height - i - 1, width - j - 1] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                if (j >= botLeftValue)
                {
                    this.botBorderIndexes[j] = Math.Max(0, leftIndex - 1);
                }
                else
                {
                    this.botBorderIndexes[j] = -1;
                }
                if (j >= botRightValue)
                {
                    this.botBorderIndexes[width - j - 1] = Math.Max(0, rightIndex - 1);
                }
                else
                {
                    this.botBorderIndexes[width - j - 1] = -1;
                }
            }
            if (width % 2 == 1)
            {
                int middleIndex = this.GetNextIndex(random, space, leftIndex, rightIndex, 0f);

                middleIndex = Math.Min(midWidth - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int i = 0; i < middleIndex; i++)
                {
                    this.dataStructureCasesType[height - i - 1, midWidth] = DataStructureCaseType.EMPTY_CASE;
                }

                // Boundaries
                this.botBorderIndexes[midWidth] = Math.Max(0, middleIndex - 1);
            }

            this.leftBorderIndexes[topLeftValue - 1] = topLeftValue - 1;
            this.topBorderIndexes[topLeftValue - 1] = topLeftValue - 1;
            this.leftBorderIndexes[height - botLeftValue] = botLeftValue - 1;
            this.botBorderIndexes[botLeftValue - 1] = botLeftValue - 1;

            this.rightBorderIndexes[topRightValue - 1] = topRightValue - 1;
            this.topBorderIndexes[width - topRightValue] = topRightValue - 1;
            this.rightBorderIndexes[height - botRightValue] = botRightValue - 1;
            this.botBorderIndexes[width - botRightValue] = botRightValue - 1;
        }

        protected virtual void GenerateStructureBoundariesLimit(Random random)
        {
            int height = this.DataStructureCases.GetLength(0);
            int width = this.DataStructureCases.GetLength(1);

            int fillIndex = -1;
            int borderIndex = -1;

            int counter = -1;

            for (int i = 0; i < height; i++)
            {
                if (this.leftBorderIndexes[i] >= 0)
                {
                    this.GetFillBorderIndexes(this.leftBorderIndexes, i, out fillIndex, out borderIndex);

                    int currentIndex = this.leftBorderIndexes[i];
                    counter = currentIndex - fillIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[i, currentIndex] = DataStructureCaseType.FILLED_CASE;
                        currentIndex--;
                    }
                    counter = currentIndex - borderIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[i, currentIndex] = DataStructureCaseType.BORDER_CASE;
                        currentIndex--;
                    }
                }
                if (this.rightBorderIndexes[i] >= 0)
                {
                    this.GetFillBorderIndexes(this.rightBorderIndexes, i, out fillIndex, out borderIndex);

                    int currentIndex = this.rightBorderIndexes[i];
                    counter = currentIndex - fillIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[i, width - currentIndex - 1] = DataStructureCaseType.FILLED_CASE;
                        currentIndex--;
                    }
                    counter = currentIndex - borderIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[i, width - currentIndex - 1] = DataStructureCaseType.BORDER_CASE;
                        currentIndex--;
                    }
                }
            }

            for (int j = 0; j < width; j++)
            {
                if (this.topBorderIndexes[j] >= 0)
                {
                    this.GetFillBorderIndexes(this.topBorderIndexes, j, out fillIndex, out borderIndex);

                    int currentIndex = this.topBorderIndexes[j];
                    counter = currentIndex - fillIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[currentIndex, j] = DataStructureCaseType.FILLED_CASE;
                        currentIndex--;
                    }
                    counter = currentIndex - borderIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[currentIndex, j] = DataStructureCaseType.BORDER_CASE;
                        currentIndex--;
                    }
                }
                if (this.botBorderIndexes[j] >= 0)
                {
                    this.GetFillBorderIndexes(this.botBorderIndexes, j, out fillIndex, out borderIndex);

                    int currentIndex = this.botBorderIndexes[j];
                    counter = currentIndex - fillIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[height - currentIndex - 1, j] = DataStructureCaseType.FILLED_CASE;
                        currentIndex--;
                    }
                    counter = currentIndex - borderIndex;
                    for (int z = 0; z < counter; z++)
                    {
                        this.dataStructureCasesType[height - currentIndex - 1, j] = DataStructureCaseType.BORDER_CASE;
                        currentIndex--;
                    }
                }
            }

            //this.ExportDataStructureInFile();
        }

        private void GetFillBorderIndexes(int[] borderIndexes, int index, out int fillIndex, out int borderIndex)
        {
            fillIndex = borderIndexes[index];
            borderIndex = fillIndex - 1;

            if(index > 0 && index < borderIndexes.Length - 1)
            {
                int prevIndex = borderIndexes[index - 1];
                int currentIndex = borderIndexes[index];
                int nextIndex = borderIndexes[index + 1];

                if (prevIndex >= 0 && currentIndex >= 0 && nextIndex >= 0)
                {
                    if (prevIndex < currentIndex && nextIndex < currentIndex)
                    {
                        fillIndex = Math.Max(prevIndex, nextIndex);
                    }

                    borderIndex = Math.Min(currentIndex, Math.Min(prevIndex, nextIndex)) - 1;
                }
            }
        }

        [Obsolete]
        private void ExportDataStructureInFile()
        {
            int height = this.DataStructureCases.GetLength(0);
            int width = this.DataStructureCases.GetLength(1);

            string result = string.Empty;
            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result += this.dataStructureCasesType[i, j];
                    if (j != width - 1)
                    {
                        result += ";";
                    }
                }
                result += "\n";
            }

            System.IO.File.WriteAllText("dataStructure.xlsx", result);
        }

        private int GetNextIndex(Random random, int space, int previousIndex, int otherIndex, float ratio)
        {
            float ratioSide = 0.5f;
            if(previousIndex < otherIndex)
            {
                ratioSide = ratioSide - (0.5f * ratio);
            }
            else if(previousIndex > otherIndex)
            {
                ratioSide = ratioSide + (0.5f * ratio);
            }

            if(random.NextDouble() > ratioSide)
            {
                return random.Next(previousIndex, previousIndex + space + 1);
            }
            else
            {
                return random.Next(previousIndex - space, previousIndex + 1);
            }
        }

        protected enum DataStructureCaseType
        {
            EMPTY_CASE,
            BORDER_CASE,
            FILLED_CASE
        }
    }
}
