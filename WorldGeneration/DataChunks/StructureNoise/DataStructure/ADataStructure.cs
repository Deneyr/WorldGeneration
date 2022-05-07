using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using WorldGeneration.DataChunks.StructureNoise.TallGrassStructure;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal abstract class ADataStructure : IDataStructure
    {
        protected IDataStructureCase[,] dataStructureCases;

        public IntRect StructureBoundingBox
        {
            get;
            private set;
        }

        public ADataStructure(int left, int top, int width, int height)
        {
            this.StructureBoundingBox = new IntRect(left, top, width, height);
            this.dataStructureCases = new IDataStructureCase[height, width];
        }

        public abstract void GenerateStructure(Random random, IDataStructureTemplate structureTemplate);

        public IDataStructureCase GetStructureCaseAtChunkCoordinate(int x, int y)
        {
            if (this.StructureBoundingBox.Contains(x, y))
            {
                int localX = (x - this.StructureBoundingBox.Left);
                int localY = (y - this.StructureBoundingBox.Top);

                return this.dataStructureCases[localY, localX];
            }
            return null;
        }

        protected virtual void GenerateStructureBoundaries(Random random, float ratioMargin)
        {
            int height = this.dataStructureCases.GetLength(0);
            int width = this.dataStructureCases.GetLength(1);
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
                            this.dataStructureCases[i, j] = null;
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
                            this.dataStructureCases[i, width - j - 1] = null;
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
                            this.dataStructureCases[i, j] = null;
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
                            this.dataStructureCases[height - i - 1, j] = null;
                        }
                    }
                }
            }

        }

        protected virtual void GenerateStructureBoundaries2(Random random, int margin, int space)
        {
            int height = this.dataStructureCases.GetLength(0);
            int width = this.dataStructureCases.GetLength(1);
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
                    this.dataStructureCases[i, j] = null;
                }
                for (int j = 0; j < botIndex; j++)
                {
                    this.dataStructureCases[height - i - 1, j] = null;
                }
            }
            if(height % 2 == 1)
            {      
                int middleIndex = this.GetNextIndex(random, space, topIndex, botIndex, 0f);

                middleIndex = Math.Min(midHeight - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int j = 0; j < topIndex; j++)
                {
                    this.dataStructureCases[midHeight, j] = null;
                }
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
                    this.dataStructureCases[i, width - j - 1] = null;
                }
                for (int j = 0; j < botIndex; j++)
                {
                    this.dataStructureCases[height - i - 1, width - j - 1] = null;
                }
            }
            if (height % 2 == 1)
            {
                int middleIndex = this.GetNextIndex(random, space, topIndex, botIndex, 0f);

                middleIndex = Math.Min(midHeight - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int j = 0; j < topIndex; j++)
                {
                    this.dataStructureCases[midHeight, width - j - 1] = null;
                }
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
                    this.dataStructureCases[i, j] = null;
                }
                for (int i = 0; i < rightIndex; i++)
                {
                    this.dataStructureCases[i, width - j - 1] = null;
                }
            }
            if (width % 2 == 1)
            {
                int middleIndex = this.GetNextIndex(random, space, leftIndex, rightIndex, 0f);

                middleIndex = Math.Min(midWidth - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int i = 0; i < leftIndex; i++)
                {
                    this.dataStructureCases[i, midWidth] = null;
                }
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
                    this.dataStructureCases[height - i - 1, j] = null;
                }
                for (int i = 0; i < rightIndex; i++)
                {
                    this.dataStructureCases[height - i - 1, width - j - 1] = null;
                }
            }
            if (width % 2 == 1)
            {
                int middleIndex = this.GetNextIndex(random, space, leftIndex, rightIndex, 0f);

                middleIndex = Math.Min(midWidth - 1, Math.Min(margin, Math.Max(0, middleIndex)));

                for (int i = 0; i < leftIndex; i++)
                {
                    this.dataStructureCases[height - i - 1, midWidth] = null;
                }
            }

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
    }
}
