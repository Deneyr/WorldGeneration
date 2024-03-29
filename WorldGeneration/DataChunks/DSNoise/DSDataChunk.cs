﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.DSNoise
{
    internal class DSDataChunk : ADataChunk
    {
        //protected HashSet<Vector2i> notGeneratedCases;

        private ChunkPosition currentChunkPosition;

        public DSDataChunk(Vector2i position, int nbCaseSide, int sampleLevel) 
            : base(position, nbCaseSide, sampleLevel)
        {
            //this.notGeneratedCases = new HashSet<Vector2i>();
            this.currentChunkPosition = ChunkPosition.NOT_GENERATED;
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            DSDataCase generatedCase = new DSDataCase(0, 0);

            generatedCase.Value = ((float) random.NextDouble()) / 2 + 0.25f;

            this.casesArray[0, 0] = generatedCase;
        }

        public override void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            DSDataChunkLayer dataChunkLayer = parentLayer as DSDataChunkLayer;

            int currentNbStep = dataChunkLayer.CurrentNbStep;
            bool isCurrentStepSquare = currentNbStep % 2 == 1;

            if(currentNbStep == 0)
            {
                this.currentChunkPosition = this.GetChunkPosition(parentLayer);
            }

            int chunkSeed = this.GenerateChunkSeed((dataChunksMonitor.WorldSeed - parentLayer.Id.GetHashCode()) * currentNbStep);
            Random random = new Random(chunkSeed);

            int xCaseToGenerate = 0;
            int yCaseToGenerate = 0;

            int midStepLength = dataChunkLayer.CurrentStepLength / 2;
            if (isCurrentStepSquare)
            {
                int secondStepPower = dataChunkLayer.CurrentStepPower * 2;
                for (int i = 0; i < secondStepPower; i++)
                {
                    yCaseToGenerate = i * midStepLength;
                    for (int j = 0; j < dataChunkLayer.CurrentStepPower; j++)
                    {
                        xCaseToGenerate = midStepLength * ((i + 1) % 2) + j * dataChunkLayer.CurrentStepLength;

                        ICase generatedCase = this.GenerateCase(dataChunksMonitor, parentLayer, xCaseToGenerate, yCaseToGenerate, random);

                        if (generatedCase != null)
                        {
                            this.casesArray[yCaseToGenerate, xCaseToGenerate] = generatedCase;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < dataChunkLayer.CurrentStepPower; i++)
                {
                    yCaseToGenerate = midStepLength + i * dataChunkLayer.CurrentStepLength;
                    for (int j = 0; j < dataChunkLayer.CurrentStepPower; j++)
                    {
                        xCaseToGenerate = midStepLength + j * dataChunkLayer.CurrentStepLength;

                        ICase generatedCase = this.GenerateCase(dataChunksMonitor, parentLayer, xCaseToGenerate, yCaseToGenerate, random);

                        if (generatedCase != null)
                        {
                            this.casesArray[yCaseToGenerate, xCaseToGenerate] = generatedCase;
                        }
                    }
                }
            }
        }

        private bool GetCasesSummit(IDataChunkLayer parentLayer, int x, int y, out ICase topLeftCase, out ICase topRightCase, out ICase botLeftCase, out ICase botRightCase)
        {
            DSDataChunkLayer dataChunkLayer = parentLayer as DSDataChunkLayer;

            int currentNbStep = dataChunkLayer.CurrentNbStep;
            bool isCurrentStepSquare = currentNbStep % 2 == 1;

            int midStepLength = dataChunkLayer.CurrentStepLength / 2;

            //int xStart = 0;
            //int yStart = 0;
            //int xCase = 0;
            //int yCase = 0;
            if (isCurrentStepSquare)
            {
                //xStart = x - midStepLength;
                //yStart = y - midStepLength;
                //for (int i = 0; i < 2; i++)
                //{
                //    for (int j = 0; j < 2; j++)
                //    {
                //        yCase = yStart + (i + j) * midStepLength;
                //        xCase = xStart + (i + j) * midStepLength;

                //    }
                //}
                topLeftCase = this.GetCaseAtLocal(parentLayer, x, y - midStepLength);

                topRightCase = this.GetCaseAtLocal(parentLayer, x + midStepLength, y);

                botLeftCase = this.GetCaseAtLocal(parentLayer, x - midStepLength, y);

                botRightCase = this.GetCaseAtLocal(parentLayer, x, y + midStepLength);

            }
            else
            {
                //xStart = x - midStepLength;
                //yStart = y - midStepLength;
                //for (int i = 0; i < 2; i++)
                //{
                //    yCase = yStart + i;
                //    for (int j = 0; j < 2; j++)
                //    {
                //        xCase = xStart + j;


                //    }
                //}
                topLeftCase = this.GetCaseAtLocal(parentLayer, x - midStepLength, y - midStepLength);

                topRightCase = this.GetCaseAtLocal(parentLayer, x + midStepLength, y - midStepLength);

                botLeftCase = this.GetCaseAtLocal(parentLayer, x - midStepLength, y + midStepLength);

                botRightCase = this.GetCaseAtLocal(parentLayer, x + midStepLength, y + midStepLength);
            }

            return topLeftCase != null && topRightCase != null && botLeftCase != null && botRightCase != null;
        }

        private ICase GetCaseAtLocal(IDataChunkLayer parentLayer, int x, int y)
        {
            int xChunkOffset = 0;
            int yChunkOffset = 0;

            if(x < 0)
            {
                xChunkOffset = -1;
                x = this.realNbCaseSide + x;
            }
            else if(x >= this.realNbCaseSide)
            {
                xChunkOffset = 1;
                x = this.realNbCaseSide - x;
            }


            if (y < 0)
            {
                yChunkOffset = -1;
                y = this.realNbCaseSide + y;
            }
            else if (y >= this.realNbCaseSide)
            {
                yChunkOffset = 1;
                y = this.realNbCaseSide - y;
            }

            if(xChunkOffset == 0
                && yChunkOffset == 0)
            {
                return this.casesArray[y, x];
            }
            else
            {
                ChunkContainer nextChunkContainer = (parentLayer as AExtendedDataChunkLayer).ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + xChunkOffset, this.Position.Y + yChunkOffset);

                if(nextChunkContainer != null && nextChunkContainer.ContainedChunk != null)
                {
                    return nextChunkContainer.ContainedChunk.GetCaseAtLocal(x * this.SampleLevel, y * this.SampleLevel);
                }
            }

            return null;
        }

        protected virtual ICase GenerateCaseFrom(IDataChunkLayer parentLayer, int x, int y, ICase topLeftCase, ICase topRightCase, ICase botLeftCase, ICase botRightCase, int valueGenerated)
        {
            Random random = new Random(valueGenerated);

            DSDataCase generatedCase = new DSDataCase(x * this.SampleLevel, y * this.SampleLevel);

            float ratioValueGenerated = (float)((random.NextDouble() * 2) - 1);

            generatedCase.Value = ((topLeftCase as DSDataCase).Value + (topRightCase as DSDataCase).Value + (botLeftCase as DSDataCase).Value + (botRightCase as DSDataCase).Value) / 4 
                + ratioValueGenerated * this.GetCurrentAddingRatio((parentLayer as DSDataChunkLayer).CurrentNbStep);

            return generatedCase;
        }

        protected virtual float GetCurrentAddingRatio(int currentNbStep)
        {
            float ratio = 2f / (currentNbStep + 3);
            return ratio * ratio * 0.75f;
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            int valueGenerated = random.Next();

            if (this.casesArray[y, x] == null)
            {
                if (this.GetCasesSummit(parentLayer, x, y, out ICase topLeftCase, out ICase topRightCase, out ICase botLeftCase, out ICase botRightCase))
                {
                    return this.GenerateCaseFrom(parentLayer, x, y, topLeftCase, topRightCase, botLeftCase, botRightCase, valueGenerated);
                }
            }

            //if(this.Position.X == 0 && this.Position.Y == 0)
            //{
            //    return null;
            //}

            return null;
        }

        internal bool MustBeGenerated(IDataChunkLayer parentLayer)
        {
            if(this.currentChunkPosition == ChunkPosition.CENTER)
            {
                return false;
            }

            ChunkPosition newArea = this.GetChunkPosition(parentLayer);

            if(newArea == ChunkPosition.OUT_BORDER)
            {
                return false;
            }

            if(this.currentChunkPosition == newArea)
            {
                return false;
            }

            return true;
        }

        private ChunkPosition GetChunkPosition(IDataChunkLayer parentLayer)
        {
            IntRect currentArea = parentLayer.ChunksMonitor.CurrentArea;

            int horizontalPosition = -1;
            if(this.Position.X < currentArea.Left - 1)
            {
                horizontalPosition = -1;
            }
            else if(this.Position.X < currentArea.Left)
            {
                horizontalPosition = 0;
            }
            else if(this.Position.X < currentArea.Left + currentArea.Width)
            {
                horizontalPosition = 1;
            }
            else if(this.Position.X <= currentArea.Left + currentArea.Width)
            {
                horizontalPosition = 2;
            }

            int verticalPosition = -1;
            if (this.Position.Y < currentArea.Top - 1)
            {
                verticalPosition = -1;
            }
            else if (this.Position.Y < currentArea.Top)
            {
                verticalPosition = 0;
            }
            else if (this.Position.Y < currentArea.Top + currentArea.Height)
            {
                verticalPosition = 1;
            }
            else if (this.Position.Y <= currentArea.Top + currentArea.Height)
            {
                verticalPosition = 2;
            }

            switch (horizontalPosition)
            {
                case 0:
                    switch (verticalPosition)
                    {
                        case 0:
                            return ChunkPosition.TOP_LEFT;
                        case 1:
                            return ChunkPosition.LEFT;
                        case 2:
                            return ChunkPosition.BOT_LEFT;
                    }
                    break;
                case 1:
                    switch (verticalPosition)
                    {
                        case 0:
                            return ChunkPosition.TOP;
                        case 1:
                            return ChunkPosition.CENTER;
                        case 2:
                            return ChunkPosition.BOT;
                    }
                    break;
                case 2:
                    switch (verticalPosition)
                    {
                        case 0:
                            return ChunkPosition.TOP_RIGHT;
                        case 1:
                            return ChunkPosition.RIGHT;
                        case 2:
                            return ChunkPosition.BOT_RIGHT;
                    }
                    break; 
            }
            return ChunkPosition.OUT_BORDER;
        }

        internal enum ChunkPosition
        {
            NOT_GENERATED,
            OUT_BORDER,
            CENTER,
            TOP,
            TOP_LEFT,
            LEFT,
            BOT_LEFT,
            BOT,
            BOT_RIGHT,
            RIGHT,
            TOP_RIGHT
        }
    }
}
