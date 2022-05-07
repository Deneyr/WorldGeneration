using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.Maths;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal class PerlinStructureNoise
    {
        private Vector2f topLeftVector;
        private Vector2f botLeftVector;
        private Vector2f botRightVector;
        private Vector2f topRightVector;

        protected Vector2f GenerateSummitVector(Random random)
        {
            double angle = random.NextDouble() * 2 * Math.PI;

            return new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void GenerateStructureNoise(Random random)
        {
            this.topLeftVector = this.GenerateSummitVector(random);
            this.botLeftVector = this.GenerateSummitVector(random);
            this.botRightVector = this.GenerateSummitVector(random);
            this.topRightVector = this.GenerateSummitVector(random);
        }

        public float GetValueAt(float ratioX, float ratioY)
        {
            float topLeftValue = this.topLeftVector.Dot(new Vector2f(ratioX, ratioY));
            float botLeftValue = this.botLeftVector.Dot(new Vector2f(ratioX, ratioY - 1));
            float topRightValue = this.topRightVector.Dot(new Vector2f(ratioX - 1, ratioY));
            float botRightValue = this.botRightVector.Dot(new Vector2f(ratioX - 1, ratioY - 1));

            float fadeX = Fade(ratioX);
            float fadeY = Fade(ratioY);

            float topValue = topLeftValue * (1 - fadeX) + topRightValue * fadeX;
            float botValue = botLeftValue * (1 - fadeX) + botRightValue * fadeX;

            return topValue * (1 - fadeY) + botValue * fadeY;
        }

        public static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
            //return (3 - 2 * t) * t * t;
        }
    }
}
