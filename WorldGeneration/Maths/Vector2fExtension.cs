using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Maths
{
    public static class Vector2fExtension
    {
        public static float Len2(this Vector2f obj)
        {
            return obj.X * obj.X + obj.Y * obj.Y;
        }

        public static float Len(this Vector2f obj)
        {
            return (float) Math.Sqrt(obj.Len2());
        }

        public static float Len2(this Vector2i obj)
        {
            return obj.X * obj.X + obj.Y * obj.Y;
        }

        public static float Len(this Vector2i obj)
        {
            return (float)Math.Sqrt(obj.Len2());
        }

        public static float Len2(this Vector3f obj)
        {
            return obj.X * obj.X + obj.Y * obj.Y + obj.Z * obj.Z;
        }

        public static float Len(this Vector3f obj)
        {
            return (float)Math.Sqrt(obj.Len2());
        }

        public static float Angle(this Vector2f obj)
        {
            double len = obj.Len();
            double acos = Math.Acos(obj.X / len);

            float result = 0;
            if (obj.Y >= 0)
            {
                result = (float) acos;
            }
            else
            {
                result = (float) ((Math.PI - acos) + Math.PI);
            }

            return (float) (result * 180 / Math.PI);
        }

        public static Vector2f Rotate(this Vector2f obj, double angle)
        {
            Vector2f rotatedVector = new Vector2f((float)(obj.X * Math.Cos(angle) - obj.Y * Math.Sin(angle)),
                (float)(obj.X * Math.Sin(angle) + obj.Y * Math.Cos(angle)));

            return rotatedVector;
        }

        public static float Dot(this Vector2f obj, Vector2f vector)
        {
            return obj.X * vector.X + obj.Y * vector.Y;
        }

        public static float CrossZ(this Vector2f obj, Vector2f vector)
        {
            return obj.X * vector.Y - obj.Y * vector.X;
        }

        public static float Dot(this Vector3f obj, Vector3f vector)
        {
            return obj.X * vector.X + obj.Y * vector.Y + obj.Z * vector.Z;
        }

        public static Vector3f Cross(this Vector3f obj, Vector3f vector)
        {
            return new Vector3f(
                obj.Y * vector.Z - obj.Z * vector.Y,
                obj.Z * vector.X - obj.X * vector.Z,
                obj.X * vector.Y - obj.Y * vector.X);
        }

        public static Vector2f Projection (this Vector2f obj, Vector2f vector)
        {
            float vectorLen = vector.Len();
            float len = obj.Dot(vector) / vectorLen;

            return vector / vectorLen * len;
        }

        public static Vector2f OppProjection(this Vector2f obj, Vector2f vector)
        {
            return obj - obj.Projection(vector);
        }

        public static Vector2f Normalize(this Vector2f obj)
        {
            return obj / obj.Len();
        }

        public static Vector3f Normalize(this Vector3f obj)
        {
            return obj / obj.Len();
        }

        public static bool IsZero(this Vector2f obj)
        {
            return obj.X == 0
                && obj.Y == 0;
        }

        public static bool IsZero(this Vector3f obj)
        {
            return obj.X == 0
                && obj.Y == 0
                && obj.Z == 0;
        }

    }
}
