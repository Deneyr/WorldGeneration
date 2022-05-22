using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Helpers
{
    public struct HSL
    {
        public double Hue;
        public double Saturation;
        public double Luminance;

        public HSL(int H, int S, int L)
        {
            this.Hue = 0;
            this.Saturation = 0;
            this.Luminance = 0;

            ///Range control for Hue.
            if (H <= 360 && H >= 0)
            {
                Hue = H;
            }
            else
            {
                if (H > 360) { Hue = H % 360; }
                else if (H < 0 && H > -360) { Hue = -H; }
                else if (H < 0 && H < -360) { Hue = -(H % 360); }
            }

            ///Range control for Saturation.
            if (S <= 100 && S >= 0)
            {
                Saturation = S;
            }
            else
            {
                if (S > 100) { Saturation = S % 100; }
                else if (S < 0 && S > -100) { Saturation = -S; }
                else if (S < 0 && S < -100) { Saturation = -(S % 100); }
            }

            ///Range control for Luminance
            if (L <= 100 && L >= 0)
            {
                Luminance = L;
            }
            else
            {
                if (L > 100) { Luminance = L % 100; }
                if (L < 0 && L > -100) { Luminance = -L; }
                if (L < 0 && L < -100) { Luminance = -(L % 100); }
            }
        }

        public Color TurnToRGB()
        {
            ///Reconvert to range [0,1]
            double H = Hue / 360.0;
            double S = Saturation / 100.0;
            double L = Luminance / 100.0;

            float arg1, arg2;

            if (S <= HSLColor.D_EPSILON)
            {
                Color C = new Color((byte)(L*255), (byte)(L * 255), (byte)(L * 255));
                return C;
            }
            else
            {
                if (L < 0.5)
                {
                    arg2 = (float) (L * (1 + S));
                }
                else
                {
                    arg2 = (float) ((L + S) - (S * L));
                }
                arg1 = (float) (2 * L - arg2);

                byte r = (byte) ((255 * HueToRGB(arg1, arg2, (H + 1.0 / 3.0))));
                byte g = (byte) ((255 * HueToRGB(arg1, arg2, H)));
                byte b = (byte) ((255 * HueToRGB(arg1, arg2, (H - 1.0 / 3.0))));
                Color C = new Color(r, g, b);
                return C;
            }
        }

        private double HueToRGB(double arg1, double arg2, double H)
        {
            if (H < 0) H += 1;
            if (H > 1) H -= 1;
            if ((6 * H) < 1) { return (arg1 + (arg2 - arg1) * 6 * H); }
            if ((2 * H) < 1) { return arg2; }
            if ((3 * H) < 2) { return (arg1 + (arg2 - arg1) * ((2.0 / 3.0) - H) * 6); }
            return arg1;
        }
    }


    public static class HSLColor
    {
        internal const double D_EPSILON = 0.00000000000001;

        public static HSL TurnToHSL(Color C)
        {
            ///Trivial cases.
            if (C == Color.White)
            {
                return new HSL(0, 0, 100);
            }

            if (C == Color.Black)
            {
                return new HSL(0, 0, 0);
            }

            if (C == Color.Red)
            {
                return new HSL(0, 100, 50);
            }

            if (C == Color.Yellow)
            {
                return new HSL(60, 100, 50);
            }

            if (C == Color.Green)
            {
                return new HSL(120, 100, 50);
            }

            if (C == Color.Cyan)
            {
                return new HSL(180, 100, 50);
            }

            if (C == Color.Blue)
            {
                return new HSL(240, 100, 50);
            }

            if (C == Color.Cyan)
            {
                return new HSL(300, 100, 50);
            }

            double R, G, B;

            R = C.R / 255.0;
            G = C.G / 255.0;
            B = C.B / 255.0;
            ///Casos no triviales.
            double max, min, l, s = 0;

            ///Maximos
            max = Math.Max(Math.Max(R, G), B);

            ///Minimos
            min = Math.Min(Math.Min(R, G), B);


            HSL A = new HSL();
            l = ((max + min) / 2.0);

            if (max - min <= D_EPSILON)
            {
                A.Hue = 0;
                A.Saturation = 0;
            }
            else
            {
                double diff = max - min;

                if (A.Luminance < 0.5)
                {
                    s = diff / (max + min);
                }
                else
                {
                    s = diff / (2 - max - min);
                }

                double diffR = (((max - R) * 60) + (diff / 2.0)) / diff;
                double diffG = (((max - G) * 60) + (diff / 2.0)) / diff;
                double diffB = (((max - B) * 60) + (diff / 2.0)) / diff;


                if (max - R <= D_EPSILON) { A.Hue = diffB - diffG; }
                else if (max - G <= D_EPSILON) { A.Hue = (1 * 360) / 3.0 + (diffR - diffB); }
                else if (max - B <= D_EPSILON) { A.Hue = (2 * 360) / 3.0 + (diffG - diffR); }

                if (A.Hue <= 0 || A.Hue >= 360)
                {
                    A.Hue = A.Hue % 360;
                }

                s *= 100;
            }

            l *= 100;
            A.Saturation = s;
            A.Luminance = l;
            return A;
        }
    }
}
