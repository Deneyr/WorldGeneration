using System;
using System.Collections.Generic;
using System.Geometry;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.DataChunks.WeatherMonitoring
{
    internal class AltitudeBiomeMonitor
    {
        private Dictionary<BiomeType, Bezier> biomeTypeToBezierCurve;

        public AltitudeBiomeMonitor()
        {
            this.biomeTypeToBezierCurve = new Dictionary<BiomeType, Bezier>();

            this.biomeTypeToBezierCurve.Add(BiomeType.DESERT, new Bezier(new Vector2(0, 0), new Vector2(0.294f, 0.097f), new Vector2(0.833f, 0.198f), new Vector2(1f, 0.2f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.SAVANNA, new Bezier(new Vector2(0, 0), new Vector2(0.34f, 0.09f), new Vector2(0.817f, 0.98f), new Vector2(1f, 0.1f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.TROPICAL_WOODLAND, new Bezier(new Vector2(0, 0), new Vector2(0.15f, 0.318f), new Vector2(0.726f, 0.2f), new Vector2(1f, 0.1f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.TUNDRA, new Bezier(new Vector2(0, 0), new Vector2(0.561f, 1f), new Vector2(0.35f, 1f), new Vector2(1f, 1f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.SEASONAL_FOREST, new Bezier(new Vector2(0, 0), new Vector2(0.736f, 0.532f), new Vector2(0.382f, 0.443f), new Vector2(1f, 0.32f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.RAINFOREST, new Bezier(new Vector2(0, 0), new Vector2(0.272f, 0.075f), new Vector2(0.65f, 1f), new Vector2(1f, 1f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.TEMPERATE_FOREST, new Bezier(new Vector2(0, 0), new Vector2(0.718f, 0.55f), new Vector2(0.401f, 0.43f), new Vector2(1f, 0.3f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.TEMPERATE_RAINFOREST, new Bezier(new Vector2(0, 0), new Vector2(0.677f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f)));
            this.biomeTypeToBezierCurve.Add(BiomeType.BOREAL_FOREST, new Bezier(new Vector2(0, 0), new Vector2(0.406f, 0.04f), new Vector2(0.698f, 0.075f), new Vector2(1f, 0.03f)));
        }

        public float FilterAltitudeFromBiome(BiomeType biomeType, float altitude)
        {
            //float altitude2 = altitude * altitude;
            //float altitude3 = altitude2 * altitude;
            //float altitude4 = altitude3 * altitude;
            //switch (biomeType)
            //{
            //    case BiomeType.DESERT:
            //        return 0.1f * altitude4 - 0.2f * altitude3 - 0.1f * altitude2 + 0.4f * altitude;
            //    case BiomeType.SAVANNA:
            //        return 0.08f * altitude4 - 0.1f * altitude3 - 0.1f * altitude2 + 0.2f * altitude;
            //    case BiomeType.TROPICAL_WOODLAND:
            //        return -0.9f * altitude4 + 2.2f * altitude3 - 2.2f * altitude2 + altitude;
            //    case BiomeType.TUNDRA:
            //        return 1.6f * altitude4 - 2.3f * altitude3 - altitude2 + 2.7f * altitude;
            //    case BiomeType.SEASONAL_FOREST:
            //        return 2.8f * altitude4 - 5.77f * altitude3 + 2.9f * altitude2 + 0.4f * altitude;
            //    case BiomeType.RAINFOREST:
            //        return 0.7f * altitude4 - 3.1f * altitude3 + 3.3f * altitude2 + 0.1f * altitude;
            //    case BiomeType.TEMPERATE_FOREST:
            //        return 1.55f * altitude4 - 3.3f * altitude3 + 1.4f * altitude2 + 0.7f * altitude;
            //    case BiomeType.TEMPERATE_RAINFOREST:
            //        return 2.1f * altitude4 - 4.2f * altitude3 + 1.9f * altitude2 + 0.4f * altitude;
            //    case BiomeType.BOREAL_FOREST:
            //        return 0.3f * altitude4 - 0.65f * altitude3 + 0.4f * altitude2;
            //}
            //return altitude;

            Bezier filterToApply = this.biomeTypeToBezierCurve[biomeType];
            float test = 0;
            if (altitude > 0.1)
            {
                test = -2;
            }

            //if(altitude < 0.01)
            //{
            //    altitude = 0;
            //}
            //else if(altitude > 0.99)
            //{
            //    altitude = 1f;
            //}

            float[] intersections = filterToApply.Intersects(new Line(new Vector2(altitude, test), new Vector2(altitude, 2f)));

            if (intersections.Length == 0)
            {
                return altitude;
            }

            return filterToApply.Position(filterToApply.Intersects(new Line(new Vector2(altitude, test), new Vector2(altitude, 2f)))[0]).Y;
        }
    }
}
