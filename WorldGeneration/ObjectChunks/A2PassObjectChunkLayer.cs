using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace WorldGeneration.ObjectChunks
{
    internal abstract class A2PassObjectChunkLayer : A1PassObjectChunkLayer
    {
        public int[,] SecondAreaBuffer
        {
            get;
            private set;
        }

        public List<LandTransition>[,] TransitionAreaBuffer
        {
            get;
            private set;
        }

        public int[,] MaxValueAreaBuffer
        {
            get;
            private set;
        }

        public override int ObjectChunkMargin
        {
            get
            {
                return 2;
            }
        }

        public virtual bool GenerateAllLevels
        {
            get
            {
                return false;
            }
        }

        public A2PassObjectChunkLayer(string id)
            : base(id)
        {
            this.TransitionAreaBuffer = null;
            this.MaxValueAreaBuffer = null;
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            for (int i = -this.ObjectChunkMargin; i < objectChunk.NbCaseSide + this.ObjectChunkMargin; i++)
            {
                for (int j = -this.ObjectChunkMargin; j < objectChunk.NbCaseSide + this.ObjectChunkMargin; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

                    this.ComputeBufferArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
                }
            }

            int secondObjectChunkMargin = this.ObjectChunkMargin - 1;
            for (int i = -secondObjectChunkMargin; i < objectChunk.NbCaseSide + secondObjectChunkMargin; i++)
            {
                for (int j = -secondObjectChunkMargin; j < objectChunk.NbCaseSide + secondObjectChunkMargin; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

                    this.ComputeSecondBufferArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
                }
            }

            int transitionObjectChunkMargin = this.ObjectChunkMargin - 2;
            for (int i = -transitionObjectChunkMargin; i < objectChunk.NbCaseSide + transitionObjectChunkMargin; i++)
            {
                for (int j = -transitionObjectChunkMargin; j < objectChunk.NbCaseSide + transitionObjectChunkMargin; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

                    this.ComputeTransitionAreaBuffer(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
                }
            }

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    this.ComputeChunkArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), this.GetWorldPosition(objectChunk, j, i));
                }
            }
        }

        protected virtual void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            int i = localPosition.Y + this.ObjectChunkMargin - 1;
            int j = localPosition.X + this.ObjectChunkMargin - 1;

            this.SecondAreaBuffer[i, j] = LandCreationHelper.NeedToFillMaxAt(this.AreaBuffer, localPosition.Y, localPosition.X, this.ObjectChunkMargin);
        }

        protected virtual void ComputeTransitionAreaBuffer(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            int[,] subAreaInt = new int[3, 3];
            int maxLocalAltitude = int.MinValue;

            maxLocalAltitude = this.GetComputedMatrix(localPosition.Y, localPosition.X, ref subAreaInt);

            int diffAltitude = maxLocalAltitude - subAreaInt[1, 1];
            int i = localPosition.Y + this.ObjectChunkMargin - 2;
            int j = localPosition.X + this.ObjectChunkMargin - 2;
            //this.SecondAreaBuffer[i, j] = maxLocalAltitude;

            this.MaxValueAreaBuffer[i, j] = maxLocalAltitude;

            this.TransitionAreaBuffer[i, j].Clear();

            if (diffAltitude > 0)
            {
                if (this.GenerateAllLevels == false)
                {
                    //diffAltitude = Math.Min(diffAltitude, 1);

                    subAreaInt[1, 1] = maxLocalAltitude - 1;
                    this.GetComputedLandType(ref subAreaInt, maxLocalAltitude, out LandTransition landTransition);

                    this.TransitionAreaBuffer[i, j].Add(landTransition);
                }
                else
                {
                    for (int offset = 0; offset < diffAltitude; offset++)
                    {
                        this.GetComputedLandType(ref subAreaInt, maxLocalAltitude, out LandTransition landTransition);

                        this.TransitionAreaBuffer[i, j].Add(landTransition);

                        subAreaInt[1, 1]++;
                    }
                }
            }
        }

        protected int GetComputedMatrix(int i, int j, ref int[,] subAreaInt)
        {
            int maxValue = int.MinValue;
            int minValue = int.MaxValue;

            int secondObjectChunkMargin = this.ObjectChunkMargin - 1;

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = this.SecondAreaBuffer[i + secondObjectChunkMargin + y, j + secondObjectChunkMargin + x];

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            return maxValue;
        }

        protected void GetComputedLandType(
            ref int[,] subAreaInt,
            int maxValue,
            out LandTransition landtransition)
        {
            bool[,] subAreaBool = new bool[3, 3];

            landtransition = LandTransition.NONE;

            if (subAreaInt[1, 1] < maxValue)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (subAreaInt[y, x] <= subAreaInt[1, 1])
                        {
                            subAreaBool[y, x] = false;
                        }
                        else
                        {
                            subAreaBool[y, x] = true;
                        }
                    }
                }

                landtransition = LandCreationHelper.GetLandTransitionFrom(ref subAreaBool);
            }
        }

        public override void InitObjectChunkLayer(int nbCaseSide)
        {
            base.InitObjectChunkLayer(nbCaseSide);

            int caseSideExtended = nbCaseSide + (this.ObjectChunkMargin - 1) * 2;

            this.SecondAreaBuffer = new int[caseSideExtended, caseSideExtended];

            caseSideExtended = nbCaseSide + (this.ObjectChunkMargin - 2) * 2;
            this.TransitionAreaBuffer = new List<LandTransition>[caseSideExtended, caseSideExtended];

            for(int i = 0; i < caseSideExtended; i++)
            {
                for (int j = 0; j < caseSideExtended; j++)
                {
                    this.TransitionAreaBuffer[i, j] = new List<LandTransition>();
                }
            }

            this.MaxValueAreaBuffer = new int[caseSideExtended, caseSideExtended];
        }

        public List<LandTransition> GetLandTransitionAtLocal(int x, int y)
        {
            return this.TransitionAreaBuffer[y + this.ObjectChunkMargin - 2, x + this.ObjectChunkMargin - 2];
        }

        public int GetMaxAreaBufferValueAtLocal(int x, int y)
        {
            return this.MaxValueAreaBuffer[y + this.ObjectChunkMargin - 2, x + this.ObjectChunkMargin - 2];
        }

        public int GetSecondAreaBufferValueAtLocal(int x, int y)
        {
            return this.SecondAreaBuffer[y + this.ObjectChunkMargin - 1, x + this.ObjectChunkMargin - 1];
        }
    }
}