using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Tools
{
    public class ColorTool
    {
        #region Member Variable
        static Dictionary<Color, List<string>> dic = new Dictionary<Color, List<string>>();
        #endregion

        #region Constructor
        static ColorTool()
        {

            var vals = typeof(Color).GetProperties();
            foreach (var v in vals.Where(x=>x.PropertyType == typeof(Color)))
            {
                try
                {
                    var color = (Color)v.GetValue(null);
                    var name = v.Name;
                    if (!dic.ContainsKey(color)) dic.Add(color, new List<string>());
                    dic[color].Add(name);
                }
                catch (Exception ex) { }
            }

        }
        #endregion

        #region MixColorAlpha
        /// <summary>
        /// 원본 색상에 투명도가 적용된 색상을 덧씌웠을 때 색상
        /// </summary>
        /// <param name="dest">원본 색</param>
        /// <param name="src">덧씌울 색</param>
        /// <param name="srcAlpha">덧씌울 색 투명도</param>
        /// <returns>최종색</returns>
        public static Color MixColorAlpha(Color dest, Color src, int srcAlpha)
        {
            var r = Convert.ToByte(MathTool.Constrain(MathTool.Map(srcAlpha, 0.0, 255.0, dest.R, src.R), 0, 255));
            var g = Convert.ToByte(MathTool.Constrain(MathTool.Map(srcAlpha, 0.0, 255.0, dest.G, src.G), 0, 255));
            var b = Convert.ToByte(MathTool.Constrain(MathTool.Map(srcAlpha, 0.0, 255.0, dest.B, src.B), 0, 255));

            return Color.FromArgb(r, g, b);
        }
        #endregion
        #region GetName
        public static string GetName(Color c, ColorCodeType code)
        {
            var ret = "";
            if (dic.ContainsKey(c)) ret = dic[c].First();
            else
            {
                if (code == ColorCodeType.ARGB) ret = c.A.ToString() + "," + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString();
                else if (code == ColorCodeType.RGB) ret = c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString();
                else if (code == ColorCodeType.CodeARGB) ret = "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
                else if (code == ColorCodeType.CodeRGB) ret = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
            }
            return ret;
        }
        #endregion
    }

    public enum ColorCodeType { ARGB, RGB, CodeRGB, CodeARGB }
}
