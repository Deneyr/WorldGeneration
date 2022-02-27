using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.ObjectChunks;

namespace WorldGeneration.ViewTest
{
    public class ViewChunk : IChunk
    {
        private RectangleShape[,] caseArray;

        private static Dictionary<int, Color> biomeValueToColor;

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

        public ICase[,] CasesArray
        {
            get;
        }
         
        static ViewChunk()
        {
            Random rand = new Random();
            biomeValueToColor = new Dictionary<int, Color>();

            biomeValueToColor.Add(0, Color.Blue);
            biomeValueToColor.Add(1, Color.Green);
            biomeValueToColor.Add(2, Color.Red);
            biomeValueToColor.Add(3, Color.Yellow);
        }

        public ViewChunk(IChunk chunk)
        {
            this.NbCaseSide = chunk.CasesArray.GetLength(0);
            this.Position = chunk.Position;

            this.caseArray = new RectangleShape[this.NbCaseSide, this.NbCaseSide];

            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    TestCase testCase = ChunkHelper.GetCaseAtLocalCoordinates(chunk, j, i) as TestCase;

                    RectangleShape rectangle = new RectangleShape(new Vector2f(16, 16));

                    Color color = biomeValueToColor[testCase.BiomeValue % 4];

                    float value = (testCase.Value + 1) / 2;
                    byte colorValue = this.ClampColorValue((byte)(value * 255));

                    value = this.ClampColorValue(value);

                    color.R = (byte) (color.R * value);
                    color.G = (byte) (color.G * value);
                    color.B = (byte) (color.B * value);

                    if (colorValue < 140)
                    {
                        color = Color.Black;
                    }
                    //rectangle.FillColor = new Color(colorValue, colorValue, colorValue);
                    rectangle.FillColor = color;

                    Vector2i modelPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(chunk.Position.X, chunk.Position.Y, j, i));
                    rectangle.Position = new Vector2f(modelPosition.X * ViewMonitor.MODEL_TO_VIEW + 0.1f, modelPosition.Y * ViewMonitor.MODEL_TO_VIEW + 0.1f);

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
            return (float) (Math.Floor(value * 16)) / 16f;
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            foreach(RectangleShape rectangleShape in this.caseArray)
            {
                window.Draw(rectangleShape);
            }
        }
    }
}
