using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.Maths;

namespace WorldGeneration.DataChunks.VoronoiNoise
{
    internal class VoronoiDataChunk : ADataChunk
    {
        List<Tuple<Vector2i, int>> surroundingPoints;

        public int NbPointsInside
        {
            get;
            private set;
        }

        public int BlurLength
        {
            get;
            private set;
        }

        public List<Tuple<Vector2i, int>> Points
        {
            get;
            private set;
        }

        public VoronoiDataChunk(Vector2i position, int nbCaseSide, int nbPointsInside, int blurLength) 
            : base(position, nbCaseSide)
        {
            this.NbPointsInside = nbPointsInside;
            this.BlurLength = blurLength;

            this.Points = new List<Tuple<Vector2i, int>> ();
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);
            for (int i = 0; i < this.NbPointsInside; i++)
            {
                Vector2i pointPosition = new Vector2i(random.Next(0, this.NbCaseSide), random.Next(0, this.NbCaseSide));
                Vector2i worldPointPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, pointPosition));

                this.Points.Add(new Tuple<Vector2i, int>(worldPointPosition, random.Next()));
            }

            this.surroundingPoints = null;
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            VoronoiDataCase generatedCase = new VoronoiDataCase(x, y);

            if (this.surroundingPoints == null)
            {
                this.surroundingPoints = new List<Tuple<Vector2i, int>>();

                VoronoiDataChunkLayer voronoiDataLayer = parentLayer as VoronoiDataChunkLayer;
                for(int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        ChunkContainer chunkContainer = voronoiDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + j, this.Position.Y + i);

                        this.surroundingPoints.AddRange((chunkContainer.ContainedChunk as VoronoiDataChunk).Points);
                    }
                }
            }

            int nearestPointValue = 0;
            float minDist = int.MaxValue;

            Vector2i worldCasePosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, new Vector2i(x, y)));
            Vector2f casePosition = new Vector2f(worldCasePosition.X, worldCasePosition.Y);

            foreach(Tuple<Vector2i, int> point in this.surroundingPoints)
            {
                Vector2f pointPosition = new Vector2f(point.Item1.X, point.Item1.Y);
                float len2Dist = (pointPosition - casePosition).Len2() + random.Next(-this.BlurLength, this.BlurLength);
                if (len2Dist < minDist)
                {
                    minDist = len2Dist;
                    nearestPointValue = point.Item2;
                }
            }

            generatedCase.Value = nearestPointValue;

            return generatedCase;
        }
    }
}
