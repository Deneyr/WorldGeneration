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

        public BiomeVoronoiDataChunk(Vector2i position, int nbCaseSide, int nbPointsInside, int sampleLevel)
            : base(position, nbCaseSide, nbPointsInside, sampleLevel)
        {
            //this.surroundingBiomes = new List<VoronoiRelativePoint>();
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            base.PrepareChunk(dataChunksMonitor, parentLayer);
            //int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            //Random random = new Random(chunkSeed);
            //for (int i = 0; i < this.NbPointsInside; i++)
            //{
            //    Vector2i pointPosition = new Vector2i(random.Next(0, this.NbCaseSide), random.Next(0, this.NbCaseSide));
            //    Vector2i worldPointPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, pointPosition));

            //    this.Points.Add(new VoronoiRelativePoint(worldPointPosition, random.Next()));
            //}

            this.offset2DDataAgreggator = dataChunksMonitor.DataAgreggators["2DOffset"] as Offset2DDataAgreggator;

            WeatherDataAgreggator weatherDataAgreggator = dataChunksMonitor.DataAgreggators["weather"] as WeatherDataAgreggator;

            foreach(VoronoiDataPoint point in this.Points)
            {
                float temperature = weatherDataAgreggator.GetTemperatureAtWorldCoordinates(point.PointPosition.X, point.PointPosition.Y);
                float humidity = weatherDataAgreggator.GetHumidityAtWorldCoordinates(point.PointPosition.X, point.PointPosition.Y);

                int biomeValue = (int) dataChunksMonitor.WeatherMonitor.GetBiomeAt(temperature, humidity);

                point.PointValue = biomeValue;
            }

            //this.firstChunkPoint = this.Points.First();
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
                VoronoiDataPoint secondNearestPoint = null;
                float secondMinDist = int.MaxValue;

                Vector2i worldCasePosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position, new Vector2i(x * this.SampleLevel, y * this.SampleLevel)));

                //BiomeDSDataCase biomeDSDataCase = this.biomeDSDataChunk.GetCaseAtWorldCoordinates(worldCasePosition.X, worldCasePosition.Y) as BiomeDSDataCase;
                Vector2f offsetVector = this.offset2DDataAgreggator.GetOffsetAtWorldCoordinates(worldCasePosition.X, worldCasePosition.Y);
                Vector2f casePosition = new Vector2f(worldCasePosition.X + offsetVector.X * 20, worldCasePosition.Y + offsetVector.Y * 20);
                //Vector2f casePosition = new Vector2f(worldCasePosition.X, worldCasePosition.Y);

                //Vector2i centerPointPosition = this.firstChunkPoint.PointPosition;
                //Vector2f vectorToCase = new Vector2f(casePosition.X - centerPointPosition.X, casePosition.Y - centerPointPosition.Y);

                VoronoiDataPoint nearestPoint = null;
                float minDist = int.MaxValue;
                //float firstPointWeight = 0;
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

                    //float weight = 1;
                    //if (point.PointValue != this.firstChunkPoint.PointValue)
                    //{
                    //    weight = point.GetWeightFrom(vectorToCase);
                    //    firstPointWeight += (1 - weight);
                    //}
                    //generatedCase.UpdateBiomeWeight(point.PointValue, lenDist);
                }

                //generatedCase.UpdateBiomeWeight(this.firstChunkPoint.PointValue, firstPointWeight);

                generatedCase.ParentDataPoint = nearestPoint;

                if (secondMinDist != int.MaxValue)
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
            this.areThereOtherBiomes = this.surroundingPoints.Any(pElem => pElem.PointValue != firstPointBiome);
            //foreach (VoronoiDataPoint point in this.surroundingPoints)
            //{
            //    //point.InitializeRelativeDataFrom(this.firstChunkPoint.PointPosition);
            //    this.areThereOtherBiomes |= (point.PointValue != this.firstChunkPoint.PointValue);
            //}
        }

        //protected class VoronoiRelativePoint: VoronoiDataPoint
        //{
        //    public Vector2f NormalizeVectorToPoint
        //    {
        //        get;
        //        private set;
        //    }

        //    public float VectorLength
        //    {
        //        get;
        //        private set;
        //    }

        //    public VoronoiRelativePoint(Vector2i pointPosition, int biome):
        //        base(pointPosition, biome)
        //    {

        //    }

        //    public void InitializeRelativeDataFrom(Vector2i pointFrom)
        //    {
        //        Vector2i vector = this.PointPosition - pointFrom;

        //        this.VectorLength = vector.Len();

        //        this.NormalizeVectorToPoint = new Vector2f(vector.X, vector.Y) / this.VectorLength;
        //    }

        //    public float GetWeightFrom(Vector2f vectorCasePosition)
        //    {
        //        return this.NormalizeVectorToPoint.Dot(vectorCasePosition) / this.VectorLength;
        //    }
        //}
    }
}
