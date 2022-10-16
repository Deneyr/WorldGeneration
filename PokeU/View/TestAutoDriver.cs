using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public class TestAutoDriver
    {
        private float speed;

        private Vector2f target;

        public TestAutoDriver(Vector2f startPosition, float speed)
        {
            this.speed = speed;

            this.InitTarget(startPosition);
        }

        public Vector2f GetNextPosition(Vector2f currentPosition, float deltaSecond)
        {
            Vector2f diffVector = this.target - currentPosition;

            if(diffVector.X < 10 && diffVector.Y < 10)
            {
                this.InitTarget(currentPosition);
                diffVector = this.target - currentPosition;
            }
            float length = (float) Math.Sqrt(diffVector.X * diffVector.X + diffVector.Y * diffVector.Y);
            diffVector.X = diffVector.X / length;
            diffVector.Y = diffVector.Y / length;

            currentPosition.X += diffVector.X * this.speed * deltaSecond;
            currentPosition.Y += diffVector.Y * this.speed * deltaSecond;

            return currentPosition;
        }

        private void InitTarget(Vector2f startPosition)
        {
            Random random = new Random();

            double radius = random.NextDouble() * 10000 + 5000;
            double angle = Math.PI * 2 * random.NextDouble();

            this.target = new Vector2f(startPosition.X + (float)(radius * Math.Cos(angle)), startPosition.Y + (float)(radius * Math.Sin(angle)));

            if(this.target.X < -2000000000 || this.target.X > 2000000000
                || this.target.Y < -2000000000 || this.target.Y > 2000000000)
            {
                this.target = new Vector2f((float) random.NextDouble() * 1000000000, (float) random.NextDouble() * 1000000000);
            }
        }

    }
}
