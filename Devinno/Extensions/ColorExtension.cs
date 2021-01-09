using Devinno.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Devinno.Extensions
{
    public static class ColorExtension
    {
        #region ToHSV
        public static Color ToHSV(this System.Drawing.Color color)
        {
            const double toDouble = 1.0 / 255;
            var r = toDouble * color.R;
            var g = toDouble * color.G;
            var b = toDouble * color.B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

            if (chroma == 0)
            {
                h1 = 0;
            }
            else if (max == r)
            {
                h1 = (((g - b) / chroma) + 6) % 6;
            }
            else if (max == g)
            {
                h1 = 2 + ((b - r) / chroma);
            }
            else
            {
                h1 = 4 + ((r - g) / chroma);
            }

            double saturation = chroma == 0 ? 0 : chroma / max;
            Color ret;
            ret.H = 60 * h1;
            ret.S = saturation;
            ret.V = max;
            ret.A = toDouble * color.A;
            return ret;
        }
        #endregion
        #region ToHSL
        public static HslColor ToHSL(this System.Drawing.Color color)
        {
            double _R = (color.R / 255.0);
            double _G = (color.G / 255.0);
            double _B = (color.B / 255.0);
            double _A = (color.A / 255.0);

            double _Min = Math.Min(Math.Min(_R, _G), _B);
            double _Max = Math.Max(Math.Max(_R, _G), _B);
            double _Delta = _Max - _Min;

            double H = 0;
            double S = 0;
            double L = (double)((_Max + _Min) / 2.0f);

            if (_Delta != 0)
            {
                if (L < 0.5f)
                {
                    S = (double)(_Delta / (_Max + _Min));
                }
                else
                {
                    S = (double)(_Delta / (2.0f - _Max - _Min));
                }

                double _Delta_R = (double)(((_Max - _R) / 6.0f + (_Delta / 2.0f)) / _Delta);
                double _Delta_G = (double)(((_Max - _G) / 6.0f + (_Delta / 2.0f)) / _Delta);
                double _Delta_B = (double)(((_Max - _B) / 6.0f + (_Delta / 2.0f)) / _Delta);

                if (_R == _Max)
                {
                    H = _Delta_B - _Delta_G;
                }
                else if (_G == _Max)
                {
                    H = (1.0f / 3.0f) + _Delta_R - _Delta_B;
                }
                else if (_B == _Max)
                {
                    H = (2.0f / 3.0f) + _Delta_G - _Delta_R;
                }

                if (H < 0) H += 1.0f;
                if (H > 1) H -= 1.0f;
            }
            return new HslColor() { A = _A, H = H, S = S, L = L };
        }
        #endregion
        #region BrightnessTransmit
        public static System.Drawing.Color BrightnessTransmit(this System.Drawing.Color color, double Brightness)
        {
            var c = ToHSL(color);
            var lc = MathTool.Constrain(c.L * Brightness, -1, 1);
            c.L = MathTool.Constrain(c.L + lc, 0, 1);
            return c.ToRGB();
        }
        #endregion
        #region BrightnessChange
        public static System.Drawing.Color BrightnessChange(this System.Drawing.Color color, double Brightness)
        {
            var c = ToHSL(color);
            var lc = MathTool.Map(MathTool.Constrain(Brightness, -1, 1), -1, 1, 0, 1);
            c.L = lc;
            return c.ToRGB();
        }
        #endregion
    }

    #region struct : HsvColor
    public struct Color
    {
        public double H;
        public double S;
        public double V;
        public double A;

        public System.Drawing.Color ToRGB()
        {
            var hue = MathTool.Constrain(H, 0, 360);
            var saturation = MathTool.Constrain(S, 0, 1);
            var value = MathTool.Constrain(V, 0, 1);
            var alpha = MathTool.Constrain(A, 0, 1);

            double chroma = value * saturation;
            double h1 = hue / 60.0;
            double x = chroma * (1.0 - Math.Abs((h1 % 2.0) - 1.0));
            double m = value - chroma;
            double r1, g1, b1;

            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            byte r = (byte)Math.Round(255.0 * (r1 + m));
            byte g = (byte)Math.Round(255.0 * (g1 + m));
            byte b = (byte)Math.Round(255.0 * (b1 + m));
            byte a = (byte)Math.Round(255.0 * alpha);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }
    }
    #endregion
    #region struct : HslColor
    public struct HslColor
    {
        public double H;
        public double S;
        public double L;
        public double A;

        public System.Drawing.Color ToRGB()
        {
            double var_1, var_2;
            byte R = 0, G = 0, B = 0;
            if (S == 0)
            {
                R = (byte)Math.Round(L * 255.0);
                G = (byte)Math.Round(L * 255.0);
                B = (byte)Math.Round(L * 255.0);
            }
            else
            {
                if (L < 0.5) var_2 = L * (1.0 + S);
                else var_2 = (L + S) - (S * L);

                var_1 = 2.0 * L - var_2;

                R = (byte)Math.Round(255 * H2RGB(var_1, var_2, H + (1.0 / 3.0)));
                G = (byte)Math.Round(255 * H2RGB(var_1, var_2, H));
                B = (byte)Math.Round(255 * H2RGB(var_1, var_2, H - (1.0 / 3.0)));
            }

            return System.Drawing.Color.FromArgb(R, G, B);
        }

        private double H2RGB(double v1, double v2, double vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;
            if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
            if ((2 * vH) < 1) return (v2);
            if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2 / 3) - vH) * 6);
            return (v1);
        }
    }
    #endregion
}
