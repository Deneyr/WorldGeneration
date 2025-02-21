using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Maths
{
    internal class CubicBezierCurve
    {
        private Vector2f P0, P1, P2, P3;

        public CubicBezierCurve(Vector2f point0, Vector2f point1, Vector2f point2, Vector2f point3)
        {
            P0 = point0;
            P1 = point1;
            P2 = point2;
            P3 = point3;
        }

        public Vector2f PointAt(float t)
        {
            float u = 1 - t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t2 = t * t;
            float t3 = t2 * t;

            return (u3 * P0) + (3 * u2 * t * P1) + (3 * u * t2 * P2) + (t3 * P3);
        }

        public float ImageAt(float x, float tolerance = 1e-5f, int maxIterations = 20)
        {
            float tMin = 0, tMax = 1;
            float t = 0.5f; // Commence à t = 0.5

            for (int i = 0; i < maxIterations; i++)
            {
                Vector2f point = PointAt(t);
                float error = point.X - x;

                if (Math.Abs(error) < tolerance) // Assez proche ?
                    return point.Y;

                if (error > 0)
                    tMax = t;
                else
                    tMin = t;

                t = (tMin + tMax) / 2; // Recherche dichotomique
            }

            return PointAt(t).Y; // Retourne l'image de l'approximation trouvée
        }
    }
}
