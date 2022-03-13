using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks;

namespace WorldGeneration.ViewTest
{
    public class ViewChunk : IChunk
    {
        private RectangleShape[,] caseArray;

        private static Dictionary<BiomeType, Color> biomeValueToColor;

        private static Dictionary<int, Color> altitudeValueToColor;

        public int NbCaseSide
        {
            get;
            private set;
        }

        public Vector2i Position
        {
            get;
            private set;
        }
         
        static ViewChunk()
        {
            Random rand = new Random();
            biomeValueToColor = new Dictionary<BiomeType, Color>();

            biomeValueToColor.Add(BiomeType.BOREAL_FOREST, new Color(0x23725eff));
            biomeValueToColor.Add(BiomeType.TUNDRA, new Color(0xbefff2ff));
            biomeValueToColor.Add(BiomeType.TEMPERATE_RAINFOREST, new Color(0x223d35ff));
            biomeValueToColor.Add(BiomeType.TEMPERATE_FOREST, new Color(0x44b36aff));
            biomeValueToColor.Add(BiomeType.SAVANNA, new Color(0xb8c862ff));
            biomeValueToColor.Add(BiomeType.DESERT, new Color(0xffffb2ff));
            biomeValueToColor.Add(BiomeType.RAINFOREST, new Color(0x214d29ff));
            biomeValueToColor.Add(BiomeType.SEASONAL_FOREST, new Color(0x6a9026ff));
            biomeValueToColor.Add(BiomeType.TROPICAL_WOODLAND, new Color(0xbca135ff));

            altitudeValueToColor = new Dictionary<int, Color>();
            altitudeValueToColor.Add(0, Color.Yellow);
            altitudeValueToColor.Add(1, Color.Green);
            altitudeValueToColor.Add(2, new Color(255, 128, 0));
            altitudeValueToColor.Add(3, Color.White);
        }

        public ViewChunk(IChunk chunk)
        {
            this.NbCaseSide = chunk.NbCaseSide;
            this.Position = chunk.Position;

            this.caseArray = new RectangleShape[this.NbCaseSide, this.NbCaseSide];

            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    TestCase testCase = ChunkHelper.GetCaseAtLocalCoordinates(chunk, j, i) as TestCase;

                    RectangleShape rectangle = new RectangleShape(new Vector2f(16, 16));

                    //Color color = Color.White;
                    Color color = biomeValueToColor[testCase.BiomeValue];

                    float colorValue = testCase.AltitudeValue / 32f;
                    //float colorValue = testCase.TestValue;

                    color.R = (byte)(color.R * colorValue);
                    color.G = (byte)(color.G * colorValue);
                    color.B = (byte)(color.B * colorValue);

                    if (testCase.IsUnderSea)
                    {
                        color = Color.Black;
                    }
                    rectangle.FillColor = color;

                    Vector2i modelPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(chunk.Position.X, chunk.Position.Y, j, i));
                    rectangle.Position = new Vector2f(modelPosition.X * ViewMonitor.MODEL_TO_VIEW - 0.0000001f, modelPosition.Y * ViewMonitor.MODEL_TO_VIEW - 0.0000001f);

                    this.caseArray[i, j] = rectangle;
                }
            }
        }

        private byte ClampColorValue(byte value)
        {
            return (byte) ((value / 16) * 16);
        }

        private float ClampColorValue(float value)
        {
            return (float) (Math.Floor(value * 24)) / 24;
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            foreach(RectangleShape rectangleShape in this.caseArray)
            {
                window.Draw(rectangleShape);
            }
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            return null;
        }
    }
}
