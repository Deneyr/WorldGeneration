using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DSNoise.BiomeDSNoise;
using WorldGeneration.Maths;

namespace WorldGeneration.DataChunks.VoronoiNoise
{
    internal class VoronoiDataChunk : ADataChunk
    {
        List<VoronoiDataPoint> surroundingPoints;

        //protected BiomeDSDataChunkLayer biomeDSDataChunk;

        public int NbPointsInside
        {
            get;
            private set;
        }

        //public int BlurLength
        //{
        //    get;
        //    private set;
        //}

        public List<VoronoiDataPoint> Points
        {
            get;
            private set;
        }

        public VoronoiDataChunk(Vector2i position, int nbCaseSide, int nbPointsInside) 
            : base(position, nbCaseSide)
        {
            this.NbPointsInside = nbPointsInside;

            this.Points = new List<VoronoiDataPoint>();
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);
            for (int i = 0; i < this.NbPointsInside; i++)
            {
                Vector2i pointPosition = new Vector2i(random.Next(0, this.NbCaseSide), random.Next(0, this.NbCaseSide));
                Vector2i worldPointPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, pointPosition));

                this.Points.Add(new VoronoiDataPoint(worldPointPosition, random.Next()));
            }

            this.surroundingPoints = null;
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            VoronoiDataCase generatedCase = new VoronoiDataCase(x * this.SampleLevel, y * this.SampleLevel);

            if (this.surroundingPoints == null)
            {
                this.surroundingPoints = new List<VoronoiDataPoint>();

                VoronoiDataChunkLayer voronoiDataLayer = parentLayer as VoronoiDataChunkLayer;
                for(int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        ChunkContainer chunkContainer = voronoiDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + j, this.Position.Y + i);

                        this.surroundingPoints.AddRange((chunkContainer.ContainedChunk as VoronoiDataChunk).Points);
                    }
                }

                //this.biomeDSDataChunk = dataChunksMonitor.DataChunksLayers["biomeOffset"] as BiomeDSDataChunkLayer;
            }

            int nearestPointValue = 0;
            float minDist = int.MaxValue;

            Vector2i worldCasePosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, new Vector2i(x * this.SampleLevel, y * this.SampleLevel)));

            //BiomeDSDataCase biomeDSDataCase = this.biomeDSDataChunk.GetCaseAtWorldCoordinates(worldCasePosition.X, worldCasePosition.Y) as BiomeDSDataCase;

            //Vector2f casePosition = new Vector2f(worldCasePosition.X + biomeDSDataCase.Value[0] * 20, worldCasePosition.Y + biomeDSDataCase.Value[1] * 20);
            Vector2f casePosition = new Vector2f(worldCasePosition.X, worldCasePosition.Y);

            foreach (VoronoiDataPoint point in this.surroundingPoints)
            {
                Vector2f pointPosition = new Vector2f(point.PointPosition.X, point.PointPosition.Y);
                float len2Dist = (pointPosition - casePosition).Len2();
                if (len2Dist < minDist)
                {
                    minDist = len2Dist;
                    nearestPointValue = point.PointValue;
                }
            }

            generatedCase.Value = nearestPointValue;

            return generatedCase;
        }

        internal class VoronoiDataPoint
        {
            public Vector2i PointPosition
            {
                get;
                set;
            }

            public int PointValue
            {
                get;
                set;
            }

            public VoronoiDataPoint(Vector2i pointPosition, int pointValue)
            {
                this.PointPosition = pointPosition;
                this.PointValue = pointValue;
            }
        }
    }
}
