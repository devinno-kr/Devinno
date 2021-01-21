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
    }
}
