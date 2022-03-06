using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.DSNoise
{
    internal class DSDataChunk : ADataChunk
    {
        //protected HashSet<Vector2i> notGeneratedCases;

        internal bool IsFullyGenerated
        {
            get;
            private set;
        }

        public DSDataChunk(Vector2i position, int nbCaseSide) 
            : base(position, nbCaseSide)
        {
            //this.notGeneratedCases = new HashSet<Vector2i>();
            this.IsFullyGenerated = false;
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            DSDataCase generatedCase = new DSDataCase(0, 0);

            generatedCase.Value = (((float) random.NextDouble()) / 2 - 0.25f) + 0.5f;

            this.CasesArray[0, 0] = generatedCase;
        }

        public override void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            DSDataChunkLayer dataChunkLayer = parentLayer as DSDataChunkLayer;

            if (dataChunkLayer.IsOutGeneratingArea(this))
            {
                return;
            }

            int currentNbStep = dataChunkLayer.CurrentNbStep;
            bool isCurrentStepSquare = currentNbStep % 2 == 1;

            if(currentNbStep == 0)
            {
                this.IsFullyGenerated = false;
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
                            this.CasesArray[yCaseToGenerate, xCaseToGenerate] = generatedCase;
                        }
                        else
                        {
                            this.IsFullyGenerated = false;
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
                            this.CasesArray[yCaseToGenerate, xCaseToGenerate] = generatedCase;
                        }
                        else
                        {
                            this.IsFullyGenerated = false;
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
                x = this.NbCaseSide + x;
            }
            else if(x >= this.NbCaseSide)
            {
                xChunkOffset = 1;
                x = this.NbCaseSide - x;
            }


            if (y < 0)
            {
                yChunkOffset = -1;
                y = this.NbCaseSide + y;
            }
            else if (y >= this.NbCaseSide)
            {
                yChunkOffset = 1;
                y = this.NbCaseSide - y;
            }

            if(xChunkOffset == 0
                && yChunkOffset == 0)
            {
                return this.CasesArray[y, x];
            }
            else
            {
                ChunkContainer nextChunkContainer = (parentLayer as AExtendedDataChunkLayer).ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + xChunkOffset, this.Position.Y + yChunkOffset);

                if(nextChunkContainer != null && nextChunkContainer.ContainedChunk != null)
                {
                    return nextChunkContainer.ContainedChunk.CasesArray[y, x];
                }
            }

            return null;
        }

        protected virtual ICase GenerateCaseFrom(IDataChunkLayer parentLayer, int x, int y, ICase topLeftCase, ICase topRightCase, ICase botLeftCase, ICase botRightCase, float valueGenerated)
        {
            DSDataCase generatedCase = new DSDataCase(x, y);

            generatedCase.Value = ((topLeftCase as DSDataCase).Value + (topRightCase as DSDataCase).Value + (botLeftCase as DSDataCase).Value + (botRightCase as DSDataCase).Value) / 4 
                + valueGenerated * this.GetCurrentAddingRatio((parentLayer as DSDataChunkLayer).CurrentNbStep);

            return generatedCase;
        }

        protected virtual float GetCurrentAddingRatio(int currentNbStep)
        {
            float ratio = 2f / (currentNbStep + 3);
            return ratio * ratio * 0.85f;
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            float valueGenerated = (float)((random.NextDouble() * 2) - 1);
            if (this.GetCasesSummit(parentLayer, x, y, out ICase topLeftCase, out ICase topRightCase, out ICase botLeftCase, out ICase botRightCase))
            {
                return this.GenerateCaseFrom(parentLayer, x, y, topLeftCase, topRightCase, botLeftCase, botRightCase, valueGenerated);
            }

            //if(this.Position.X == 0 && this.Position.Y == 0)
            //{
            //    return null;
            //}

            return null;
        }
    }
}
