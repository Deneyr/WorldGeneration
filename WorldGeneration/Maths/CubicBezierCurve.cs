using MathNet.Numerics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Maths
{
    internal class CubicBezierCurve
    {
        public Vector2f Point0
        {
            get;
            private set;
        }

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

        public Vector2f Point3
        {
            get;
            private set;
        }

        public CubicBezierCurve(Vector2f point0, Vector2f point1, Vector2f point2, Vector2f point3)
        {
            this.Point0 = point0;
            this.Point1 = point1;
            this.Point2 = point2;
            this.Point3 = point3;
        }

        public Vector2f PointAt(float t)
        {
            float a = 1 - t;
            float a2 = a * a;
            float a3 = a2 * a;

            float t2 = t * t;
            float t3 = t2 * t;

            float x = a3 * this.Point0.X + 3 * a2 * t * this.Point1.X + 3 * a * t2 * this.Point2.X + t3 * this.Point3.X;
            float y = a3 * this.Point0.Y + 3 * a2 * t * this.Point1.Y + 3 * a * t2 * this.Point2.Y + t3 * this.Point3.Y;

            return new Vector2f(x, y);
        }

        public float ImageAt(float x)
        {
            if(x == 0)
            {
                return 0;
            }

            // 3*p1 - p0 - 3*p2 + p3
            float a = 3 * this.Point1.X - this.Point0.X - 3 * this.Point2.X + this.Point3.X;

            // 3*p0 - 6*p1 + 3*p2
            float b = 3 * this.Point0.X - 6 * this.Point1.X + 3 * this.Point2.X;

            // 3*p1 - 3*p0
            float c = 3 * this.Point1.X - 3 * this.Point0.X;

            // p0 - x
            float d = this.Point0.X - x;

            var roots = FindRoots.Cubic(d, c, b, a);

            float t = 0;
            if (roots.Item1.Real >= 0 && roots.Item1.Real <= 1)
            {
                t = (float)roots.Item1.Real;
            }
            else if (roots.Item2.Real >= 0 && roots.Item2.Real <= 1)
            {
                t = (float)roots.Item2.Real;
            }
            else if (roots.Item3.Real >= 0 && roots.Item3.Real <= 1)
            {
                t = (float)roots.Item3.Real;
            }
            else
            {
                throw new Exception("No real solution to bezier curve equation");
            }

            float a1 = 1 - t;
            float a2 = a1 * a1;
            float a3 = a2 * a1;

            float t2 = t * t;
            float t3 = t2 * t;
            return a3 * this.Point0.Y + 3 * a2 * t * this.Point1.Y + 3 * a1 * t2 * this.Point2.Y + t3 * this.Point3.Y;
        }
    }
}
