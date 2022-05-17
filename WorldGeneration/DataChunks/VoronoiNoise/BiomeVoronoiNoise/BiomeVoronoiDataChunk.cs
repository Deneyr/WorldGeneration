using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.Maths;

namespace WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise
{
    internal class BiomeVoronoiDataChunk : VoronoiDataChunk
    {
        //private List<VoronoiRelativePoint> surroundingBiomes;
        //private VoronoiDataPoint firstChunkPoint;

        protected Offset2DDataAgreggator offset2DDataAgreggator;

        private bool areThereOtherBiomes;

        private bool areThereOtherRiverBiomes;

        public BiomeVoronoiDataChunk(Vector2i position, int nbCaseSide, int nbPointsInside, int sampleLevel)
            : base(position, nbCaseSide, nbPointsInside, sampleLevel)
        {
            //this.surroundingBiomes = new List<VoronoiRelativePoint>();
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            base.PrepareChunk(dataChunksMonitor, parentLayer);

            this.offset2DDataAgreggator = dataChunksMonitor.DataAgreggators["2DOffset"] as Offset2DDataAgreggator;

            WeatherDataAgreggator weatherDataAgreggator = dataChunksMonitor.DataAgreggators["weather"] as WeatherDataAgreggator;

            foreach(VoronoiDataPoint point in this.Points)
            {
                float temperature = weatherDataAgreggator.GetTemperatureAtWorldCoordinates(point.PointPosition.X, point.PointPosition.Y);
                float humidity = weatherDataAgreggator.GetHumidityAtWorldCoordinates(point.PointPosition.X, point.PointPosition.Y);

                int biomeValue = (int) dataChunksMonitor.WeatherMonitor.GetBiomeAt(temperature, humidity);

                point.PointValue = biomeValue;
                (point as BiomeVoronoiDataPoint).RiverPointValue = this.GetMatrixValueFrom(2, 2, temperature, humidity);
            }

            //this.firstChunkPoint = this.Points.First();
        }

        protected override VoronoiDataPoint CreateVoronoiDataPoint()
        {
            return new BiomeVoronoiDataPoint();
        }

        public override void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            base.GenerateChunk(dataChunksMonitor, parentLayer);
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            BiomeVoronoiDataCase generatedCase = new BiomeVoronoiDataCase(x * this.SampleLevel, y * this.SampleLevel);

            if (this.surroundingPoints == null)
            {
                this.CreateSurroundingPoints(parentLayer);
            }

            if (this.areThereOtherBiomes)
            {
                this.GetBiomeNearestPoints(x, y, out VoronoiDataPoint nearestPoint, out float minDist, out VoronoiDataPoint secondNearestPoint, out float secondMinDist);

                generatedCase.ParentDataPoint = nearestPoint;

                if (secondNearestPoint != null)
                {
                    generatedCase.BorderValue = Math.Min(1, Math.Abs(secondMinDist - minDist) / (this.NbCaseSide / 2));
                }
            }
            else
            {
                generatedCase.ParentDataPoint = this.surroundingPoints.First();
                //generatedCase.UpdateBiomeWeight(generatedCase.Value, 1);
                generatedCase.BorderValue = 1;
            }

            if (this.areThereOtherRiverBiomes)
            {
                this.GetRiverNearestPoints(x, y, out BiomeVoronoiDataPoint riverNearestPoint, out float riverMinDist, out BiomeVoronoiDataPoint riverSecondNearestPoint, out float riverSecondMinDist);

                if (riverSecondNearestPoint != null)
                {
                    generatedCase.RiverBorderValue = Math.Abs((riverSecondMinDist - riverMinDist) / 2);
                }
            }
            else
            {
                generatedCase.RiverBorderValue = float.MaxValue;
            }

            //generatedCase.FinalCaseUpdate(this.NbCaseSide);

            return generatedCase;
        }

        protected override void CreateSurroundingPoints(IDataChunkLayer parentLayer)
        {
            this.surroundingPoints = new List<VoronoiDataPoint>();

            VoronoiDataChunkLayer voronoiDataLayer = parentLayer as VoronoiDataChunkLayer;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    ChunkContainer chunkContainer = voronoiDataLayer.ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + j, this.Position.Y + i);

                    this.surroundingPoints.AddRange((chunkContainer.ContainedChunk as VoronoiDataChunk).Points);
                }
            }

            int firstPointBiome = this.surroundingPoints.First().PointValue;
            int firstPointRiverBiome = (this.surroundingPoints.First() as BiomeVoronoiDataPoint).RiverPointValue;

            this.areThereOtherBiomes = this.surroundingPoints.Any(pElem => pElem.PointValue != firstPointBiome);
            this.areThereOtherRiverBiomes = this.surroundingPoints.Any(pElem => (pElem as BiomeVoronoiDataPoint).RiverPointValue != firstPointBiome);
            //foreach (VoronoiDataPoint point in this.surroundingPoints)
            //{
            //    //point.InitializeRelativeDataFrom(this.firstChunkPoint.PointPosition);
            //    this.areThereOtherBiomes |= (point.PointValue != this.firstChunkPoint.PointValue);
            //}
        }

        private void GetBiomeNearestPoints(int x, int y, out VoronoiDataPoint nearestPoint, out float minDist, out VoronoiDataPoint secondNearestPoint, out float secondMinDist)
        {
            secondNearestPoint = null;
            secondMinDist = int.MaxValue;

            Vector2i worldCasePosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, new Vector2i(x * this.SampleLevel, y * this.SampleLevel)));

            Vector2f offsetVector = this.offset2DDataAgreggator.GetOffsetAtWorldCoordinates(worldCasePosition.X, worldCasePosition.Y);
            Vector2f casePosition = new Vector2f(worldCasePosition.X + offsetVector.X * 20, worldCasePosition.Y + offsetVector.Y * 20);

            nearestPoint = null;
            minDist = int.MaxValue;

            foreach (VoronoiDataPoint point in this.surroundingPoints)
            {
                Vector2f pointPosition = new Vector2f(point.PointPosition.X, point.PointPosition.Y);

                float lenDist = (pointPosition - casePosition).Len();
                if (lenDist < minDist)
                {
                    if (nearestPoint != null
                        && point.PointValue != nearestPoint.PointValue)
                    {
                        secondNearestPoint = nearestPoint;
                        secondMinDist = minDist;
                    }

                    minDist = lenDist;
                    nearestPoint = point;
                }
                else if (lenDist < secondMinDist
                    && point.PointValue != nearestPoint.PointValue)
                {
                    secondNearestPoint = point;
                    secondMinDist = lenDist;
                }
            }
        }

        private void GetRiverNearestPoints(int x, int y, out BiomeVoronoiDataPoint nearestPoint, out float minDist, out BiomeVoronoiDataPoint secondNearestPoint, out float secondMinDist)
        {
            secondNearestPoint = null;
            secondMinDist = int.MaxValue;

            Vector2i worldCasePosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, new Vector2i(x * this.SampleLevel, y * this.SampleLevel)));

            Vector2f offsetVector = this.offset2DDataAgreggator.GetSmoothOffsetAtWorldCoordinates(worldCasePosition.X, worldCasePosition.Y);
            Vector2f casePosition = new Vector2f(worldCasePosition.X + offsetVector.X * 3.5f, worldCasePosition.Y + offsetVector.Y * 3.5f);

            nearestPoint = null;
            minDist = int.MaxValue;

            foreach (VoronoiDataPoint point in this.surroundingPoints)
            {
                BiomeVoronoiDataPoint biomeVoronoiDataPoint = point as BiomeVoronoiDataPoint;

                Vector2f pointPosition = new Vector2f(point.PointPosition.X, point.PointPosition.Y);

                float lenDist = (pointPosition - casePosition).Len();
                if (lenDist < minDist)
                {
                    if (nearestPoint != null
                        && biomeVoronoiDataPoint.RiverPointValue != nearestPoint.RiverPointValue)
                    {
                        secondNearestPoint = nearestPoint;
                        secondMinDist = minDist;
                    }

                    minDist = lenDist;
                    nearestPoint = biomeVoronoiDataPoint;
                }
                else if (lenDist < secondMinDist
                    && biomeVoronoiDataPoint.RiverPointValue != nearestPoint.RiverPointValue)
                {
                    secondNearestPoint = biomeVoronoiDataPoint;
                    secondMinDist = lenDist;
                }
            }
        }

        private int GetMatrixValueFrom(int nbColumn, int nbRow, float x, float y)
        {
            return ((int)(nbRow * y)) * nbColumn + ((int)(nbColumn * x));
        }

        internal class BiomeVoronoiDataPoint: VoronoiDataPoint
        {
            public int RiverPointValue
            {
                get;
                set;
            }

            public BiomeVoronoiDataPoint()
            {

            }
        }
    }
}
