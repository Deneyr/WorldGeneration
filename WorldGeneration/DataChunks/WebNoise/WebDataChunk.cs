using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.Maths;

namespace WorldGeneration.DataChunks.WebNoise
{
    internal class WebDataChunk : ADataChunk
    {
        protected List<WebDataEdge> surroundingEdges;

        protected Offset2DDataAgreggator offset2DDataAgreggator;

        protected AltitudeDataAgreggator slopeAltitudeAgreggator;

        //protected BiomeDSDataChunkLayer biomeDSDataChunk;

        //public int NbPointsInside
        //{
        //    get;
        //    private set;
        //}

        public int WebMargin
        {
            get;
            private set;
        }

        public Vector2f Point
        {
            get;
            private set;
        }

        public WebDataChunk(Vector2i position, int nbCaseSide, int webMargin, int sampleLevel) 
            : base(position, nbCaseSide, sampleLevel)
        {
            //this.NbPointsInside = nbPointsInside;
            this.WebMargin = Math.Min(nbCaseSide / 2, webMargin);
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            int generatingWindow = this.NbCaseSide - this.WebMargin;

            this.offset2DDataAgreggator = dataChunksMonitor.DataAgreggators["2DOffset"] as Offset2DDataAgreggator;
            this.slopeAltitudeAgreggator = dataChunksMonitor.DataAgreggators["slopeAltitude"] as AltitudeDataAgreggator;

            //for (int i = 0; i < this.NbPointsInside; i++)
            //{
            Vector2i pointPosition = new Vector2i(random.Next(this.WebMargin, generatingWindow), random.Next(this.WebMargin, generatingWindow));
            Vector2i worldPointPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, pointPosition));

            this.Point = new Vector2f(worldPointPosition.X + ((float) random.NextDouble()), worldPointPosition.Y + ((float) random.NextDouble()));
            //}

            this.surroundingEdges = null;
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            if (this.surroundingEdges == null)
            {
                this.CreateSurroundingEdges(parentLayer);
            }

            Vector2i worldPointPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, new Vector2i(x * this.SampleLevel, y * this.SampleLevel)));

            Vector2f offsetVector = this.offset2DDataAgreggator.GetOffsetAtWorldCoordinates(worldPointPosition.X, worldPointPosition.Y);
            Vector2f casePosition = new Vector2f(worldPointPosition.X + offsetVector.X * 20, worldPointPosition.Y + offsetVector.Y * 20);

            float weight = this.GetWeightAt(casePosition);

            return new WebDataCase(x * this.SampleLevel, y * this.SampleLevel, Math.Min(1, weight / 2));
        }

        protected virtual void CreateSurroundingEdges(IDataChunkLayer parentLayer)
        {
            this.surroundingEdges = new List<WebDataEdge>();

            WebDataChunkLayer webDataLayer = parentLayer as WebDataChunkLayer;

            ChunkContainer chunkContainer;
            ChunkContainer chunkContainer2;
            ChunkContainer chunkContainer3;

            // Left-Top
            chunkContainer2 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X, this.Position.Y - 1);
            chunkContainer = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X - 1, this.Position.Y - 1);
            chunkContainer3 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X - 1, this.Position.Y);
            this.CreateWebDataEdges(
                this.Point,
                (chunkContainer.ContainedChunk as WebDataChunk).Point,
                (chunkContainer2.ContainedChunk as WebDataChunk).Point,
                (chunkContainer3.ContainedChunk as WebDataChunk).Point);

            // Left-Bot
            chunkContainer2 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X - 1, this.Position.Y);
            chunkContainer = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X - 1, this.Position.Y + 1);
            chunkContainer3 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X, this.Position.Y + 1);
            this.CreateWebDataEdges(
                this.Point,
                (chunkContainer.ContainedChunk as WebDataChunk).Point,
                (chunkContainer2.ContainedChunk as WebDataChunk).Point,
                (chunkContainer3.ContainedChunk as WebDataChunk).Point);

            // Right-Bot
            chunkContainer2 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X, this.Position.Y + 1);
            chunkContainer = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + 1, this.Position.Y + 1);
            chunkContainer3 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + 1, this.Position.Y);
            this.CreateWebDataEdges(
                this.Point,
                (chunkContainer.ContainedChunk as WebDataChunk).Point,
                (chunkContainer2.ContainedChunk as WebDataChunk).Point,
                (chunkContainer3.ContainedChunk as WebDataChunk).Point);

            // Right-Top
            chunkContainer2 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + 1, this.Position.Y);
            chunkContainer = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + 1, this.Position.Y - 1);
            chunkContainer3 = webDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X, this.Position.Y - 1);
            this.CreateWebDataEdges(
                this.Point,
                (chunkContainer.ContainedChunk as WebDataChunk).Point,
                (chunkContainer2.ContainedChunk as WebDataChunk).Point,
                (chunkContainer3.ContainedChunk as WebDataChunk).Point);
        }

        private void CreateWebDataEdges(Vector2f point1, Vector2f point2, Vector2f point3, Vector2f point4)
        {
            Random random = new Random((int) (point1.X + point1.Y + point2.X + point2.Y + point3.X + point3.Y + point4.X + point4.Y));

            float weight1 = 1;//0.5f + (float)(random.NextDouble() * 0.5f);
            float weight2 = 1;//0.5f + (float)(random.NextDouble() * 0.5f);
            float weight3 = 1;// 0.5f + (float)(random.NextDouble() * 0.5f);
            float weight4 = 1;// 0.5f + (float)(random.NextDouble() * 0.5f);
            float sumWeight = weight1 + weight2 + weight3 + weight4;

            float centerX = (point1.X * weight1 + point2.X * weight2 + point3.X * weight3 + point4.X * weight4) / sumWeight;
            float centerY = (point1.Y * weight1 + point2.Y * weight2 + point3.Y * weight3 + point4.Y * weight4) / sumWeight;

            Vector2f centerPoint = new Vector2f(centerX, centerY);

            if (random.NextDouble() > 0.8)
            {
                WebDataEdge webDataEdge;
                float threshold = 0.5f;
                webDataEdge = new WebDataEdge(centerPoint, point1);
                if (webDataEdge.SlopeRatio(this.slopeAltitudeAgreggator) > threshold)
                {
                    this.surroundingEdges.Add(new WebDataEdge(centerPoint, point1));
                }
                webDataEdge = new WebDataEdge(centerPoint, point2);
                if (webDataEdge.SlopeRatio(this.slopeAltitudeAgreggator) > threshold)
                {
                    this.surroundingEdges.Add(new WebDataEdge(centerPoint, point2));
                }
                webDataEdge = new WebDataEdge(centerPoint, point3);
                if (webDataEdge.SlopeRatio(this.slopeAltitudeAgreggator) > threshold)
                {
                    this.surroundingEdges.Add(new WebDataEdge(centerPoint, point3));
                }
                webDataEdge = new WebDataEdge(centerPoint, point4);
                if (webDataEdge.SlopeRatio(this.slopeAltitudeAgreggator) > threshold)
                {
                    this.surroundingEdges.Add(new WebDataEdge(centerPoint, point4));
                }
            }
        }

        private float GetWeightAt(Vector2f worldCasePosition)
        {
            //Vector2f worldPosition = new Vector2f(worldCasePosition.X, worldCasePosition.Y);

            float weight = float.MaxValue;
            foreach(WebDataEdge webDataEdge in this.surroundingEdges)
            {
                weight = Math.Min(weight, webDataEdge.GetEdgeDistanceAt(worldCasePosition));
            }

            return weight;
        }

        internal class WebDataEdge
        {
            private static readonly Vector3f UP_VECTOR = new Vector3f(0, 0, 1); 

            public Vector2f Point1
            {
                get;
                private set;
            }

            public Vector2f Point2
            {
                get;
                private set;
            }

            public Vector2f NormalizeVector
            {
                get;
                private set;
            }

            public WebDataEdge(Vector2f point1, Vector2f point2)
            {
                this.Point1 = point1;
                this.Point2 = point2;

                this.NormalizeVector = (this.Point2 - this.Point1).Normalize();
            }

            public float GetEdgeDistanceAt(Vector2f worldPosition)
            {
                Vector2f vector1 = worldPosition - this.Point1;

                float proj1 = this.NormalizeVector.Dot(vector1);

                Vector2f vector2 = worldPosition - this.Point2;

                float proj2 = (-this.NormalizeVector).Dot(vector2);

                if(proj1 > 0 && proj2 > 0)
                {
                    return (vector1 - (this.NormalizeVector * proj1)).Len();
                }

                if(proj1 <= 0)
                {
                    return vector1.Len();
                }
                else
                {
                    return vector2.Len();
                }
            }

            public float SlopeRatio(AltitudeDataAgreggator slopeAltitudeAgreggator)
            {
                Vector3f slopeDirection = new Vector3f(this.NormalizeVector.X, this.NormalizeVector.Y, 0);

                float slopeRatio1 = this.SlopeRatioAtWorldPoint(slopeAltitudeAgreggator, slopeDirection, this.Point1.X, this.Point1.Y);
                float slopeRatio2 = this.SlopeRatioAtWorldPoint(slopeAltitudeAgreggator, slopeDirection, this.Point2.X, this.Point2.Y);
                float slopeRatio3 = this.SlopeRatioAtWorldPoint(slopeAltitudeAgreggator, slopeDirection, (this.Point1.X + this.Point2.X) / 2, (this.Point1.Y + this.Point2.Y) / 2);

                return (slopeRatio1 + slopeRatio2 + slopeRatio3) / 3;
            }

            private float SlopeRatioAtWorldPoint(AltitudeDataAgreggator slopeAltitudeAgreggator, Vector3f slopeDirection, float x, float y)
            {
                Vector3f normal = slopeAltitudeAgreggator.GetNormalAtWorldCoordinate((int) x, (int) y);

                if (normal.IsZero() == false)
                {
                    Vector3f slopeNormal = UP_VECTOR.Cross(normal).Normalize();
                    if(slopeNormal.IsZero() == false)
                    {
                        Vector3f slopeNormalOnPlane = slopeNormal.Cross(UP_VECTOR);

                        return Math.Abs(slopeDirection.Dot(slopeNormalOnPlane));
                    }
                }
                return 0;
            }
        }
    }
}
