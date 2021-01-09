using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Tools
{
    public class MathTool
    {
        #region Map
        public static double Map(double val, double min, double max, double convert_min, double convert_max)
        {
            double ret = 0;
            if ((max - min) != 0) ret = (val - min) * (convert_max - convert_min) / (max - min) + convert_min;
            return ret;
        }
        public static decimal Map(decimal val, decimal min, decimal max, decimal convert_min, decimal convert_max)
        {
            decimal ret = 0;
            if ((max - min) != 0) ret = (val - min) * (convert_max - convert_min) / (max - min) + convert_min;
            return ret;
        }
        #endregion
        #region Constrain
        public static double Constrain(double val, double min, double max)
        {
            double ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }
        public static long Constrain(long val, long min, long max)
        {
            long ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }
        public static decimal Constrain(decimal val, decimal min, decimal max)
        {
            decimal ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }
        #endregion
        #region GetAngle
        public static double GetAngle(Point from, Point to)
        {
            return Math.Atan2(to.Y - from.Y, to.X - from.X) * 180.0 / Math.PI;
        }
        public static double GetAngle(PointF from, PointF to)
        {
            return Math.Atan2(to.Y - from.Y, to.X - from.X) * 180.0 / Math.PI;
        }
        #endregion
        #region GetDistance
        public static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
        public static double GetDistance(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static double GetDistance(PointF LN1, PointF LN2, PointF pt)
        {
            double a, b, c, d;
            double rM;
            double C;
            a = Math.Sqrt(Math.Pow(LN1.X - LN2.X, 2) + Math.Pow(LN1.Y - LN2.Y, 2));
            b = Math.Sqrt(Math.Pow(LN2.X - pt.X, 2) + Math.Pow(LN2.Y - pt.Y, 2));
            c = Math.Sqrt(Math.Pow(LN1.X - pt.X, 2) + Math.Pow(LN1.Y - pt.Y, 2));

            if (Math.Pow(a, 2) + Math.Pow(b, 2) <= Math.Pow(c, 2))
                return b;
            else if (Math.Pow(a, 2) + Math.Pow(c, 2) <= Math.Pow(b, 2))
                return c;

            if (LN1.X == LN2.X)
            {
                d = pt.X - LN1.X;
                return d;
            }
            if (LN1.Y == LN2.Y)
            {
                d = pt.Y - LN1.Y;
                return d;
            }

            rM = (double)(LN1.Y - LN2.Y) / (double)(LN1.X - LN2.X);
            C = LN1.Y - rM * LN1.X;
            d = Math.Abs((double)(rM * (double)pt.X - (double)pt.Y + C)) / Math.Sqrt(Math.Pow(rM, 2) + 1);
            return d;
        }

        public static double GetDistance(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double dz = z2 - z1;

            return Math.Sqrt(dx * dx + dy * dy * dz * dz);
        }

        public static double GetDistanceN(double[] first, double[] second)
        {
            var sum = first.Select((x, i) => (x - second[i]) * (x - second[i])).Sum();
            return Math.Sqrt(sum);
        }
        #endregion

        #region AbsRectangle
        public static RectangleF AbsRectangle(RectangleF rt) { return AbsRectangle(rt.Left, rt.Right, rt.Top, rt.Bottom); }
        public static RectangleF AbsRectangle(float Left, float Right, float Top, float Bottom)
        {
            var Width = Math.Abs(Right - Left);
            var Height = Math.Abs(Bottom - Top);
            var X = Math.Max(Left, Right) - Width;
            var Y = Math.Max(Top, Bottom) - Height;
            return new RectangleF(X, Y, Width, Height);
        }
        #endregion

        #region RotatePoint
        public static PointF RotatePoint(PointF Center, PointF Target, float angle)
        {
            double angleInRadians = angle * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new PointF
            {
                X = Convert.ToSingle((cosTheta * (Target.X - Center.X) - sinTheta * (Target.Y - Center.Y) + Center.X)),
                Y = Convert.ToSingle((sinTheta * (Target.X - Center.X) + cosTheta * (Target.Y - Center.Y) + Center.Y))
            };
        }
        #endregion
        #region CenterPoint
        #region CenterPoint ( Rectangle )
        public static Point CenterPoint(Rectangle rt)
        {
            return new Point(rt.X + rt.Width / 2, rt.Y + rt.Height / 2);
        }

        public static PointF CenterPoint(RectangleF rt)
        {
            return new PointF(rt.X + rt.Width / 2F, rt.Y + rt.Height / 2F);
        }
        #endregion
        #region CenterPoint ( p1, p2 )
        public static Point CenterPoint(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public static PointF CenterPoint(PointF p1, PointF p2)
        {
            return new PointF((p1.X + p2.X) / 2F, (p1.Y + p2.Y) / 2F);
        }
        #endregion
        #region CenterPoint ( List<Point> )
        /*
        public static PointF CenterPoint(IEnumerable<PointF> pts)
        {
            var x1 = pts.Min(x => x.X);
            var y1 = pts.Min(x => x.Y);
            var x2 = pts.Max(x => x.X);
            var y2 = pts.Max(x => x.Y);
            return new PointF(x1 + ((x2 - x1) / 2F), y1 + ((y2 / y1) / 2F));
        }
        */
        public PointF CenterPoint(List<PointF> sourceList)
        {
            float centerX = 0F;
            float centerY = 0F;
            float polygonArea = 0F;

            int firstIndex;
            int secondIndex;
            int sourceCount = sourceList.Count;

            PointF firstPoint;
            PointF secondPoint;

            float factor = 0F;

            for (firstIndex = 0; firstIndex < sourceCount; firstIndex++)
            {
                secondIndex = (firstIndex + 1) % sourceCount;

                firstPoint = sourceList[firstIndex];
                secondPoint = sourceList[secondIndex];

                factor = ((firstPoint.X * secondPoint.Y) - (secondPoint.X * firstPoint.Y));

                polygonArea += factor;
                centerX += (firstPoint.X + secondPoint.X) * factor;
                centerY += (firstPoint.Y + secondPoint.Y) * factor;
            }

            polygonArea /= 2F;
            polygonArea *= 6F;

            factor = 1F / polygonArea;

            centerX *= factor;
            centerY *= factor;

            return new PointF(centerX, centerY);

        }
        #endregion
        #endregion
        #region MakeRectangle
        #region MakeRectangle ( Rectangle Center )
        /// <summary>
        /// rect 중심 기준 sz 크기의 사각형 생성
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Rectangle MakeRectangle(Rectangle rect, Size size)
        {
            return new Rectangle(rect.X + (rect.Width / 2) - (size.Width / 2), rect.Y + (rect.Height / 2) - (size.Height / 2), size.Width, size.Height);
        }
        public static RectangleF MakeRectangle(RectangleF rect, SizeF size)
        {
            return new RectangleF(rect.X + (rect.Width / 2F) - (size.Width / 2F), rect.Y + (rect.Height / 2F) - (size.Height / 2F), size.Width, size.Height);
        }
        #endregion
        #region MakeRectangle ( Two Point )
        /// <summary>
        /// 두점을 기준으로 사각형 생성
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static Rectangle MakeRectangle(Point pt1, Point pt2)
        {
            int minx = Math.Min(pt1.X, pt2.X);
            int miny = Math.Min(pt1.Y, pt2.Y);
            int maxx = Math.Max(pt1.X, pt2.X);
            int maxy = Math.Max(pt1.Y, pt2.Y);

            int rx = maxx - minx - 1; if (rx < 0) rx = 0;
            int ry = maxy - miny - 1; if (ry < 0) ry = 0;

            return new Rectangle(minx, miny, rx, ry);
        }
        public static RectangleF MakeRectangle(PointF pt1, PointF pt2)
        {
            var minx = Math.Min(pt1.X, pt2.X);
            var miny = Math.Min(pt1.Y, pt2.Y);
            var maxx = Math.Max(pt1.X, pt2.X);
            var maxy = Math.Max(pt1.Y, pt2.Y);

            var rx = maxx - minx - 1F; if (rx < 0F) rx = 0F;
            var ry = maxy - miny - 1F; if (ry < 0F) ry = 0F;

            return new RectangleF(minx, miny, rx, ry);
        }
        #endregion
        #region MakeRectangle ( Points ) 
        public static RectangleF MakeRectangle(IEnumerable<Point> pts)
        {
            if (pts.Count() >= 2)
            {
                float minx = pts.Min(x => x.X);
                float miny = pts.Min(x => x.Y);
                float maxx = pts.Max(x => x.X);
                float maxy = pts.Max(x => x.Y);

                float rx = maxx - minx - 1F; if (rx < 0F) rx = 0F;
                float ry = maxy - miny - 1F; if (ry < 0F) ry = 0F;

                return new RectangleF(minx, miny, rx, ry);
            }
            else throw new Exception("POINTS LENGTH < 2");
        }
        #endregion
        #region MakeRectangle ( Center Point )
        public static Rectangle MakeRectangle(Point pt, int Size) { return new Rectangle(pt.X - (Size / 2), pt.Y - (Size / 2), Size, Size); }
        public static RectangleF MakeRectangle(PointF pt, float Size) { return new RectangleF(pt.X - (Size / 2F), pt.Y - (Size / 2F), Size, Size); }

        public static Rectangle MakeRectangle(Point pt, int rWIdth, int rHeight) { return new Rectangle(pt.X - rWIdth, pt.Y - rHeight, rWIdth * 2, rHeight * 2); }
        public static RectangleF MakeRectangle(PointF pt, float rWIdth, float rHeight) { return new RectangleF(pt.X - rWIdth, pt.Y - rHeight, rWIdth * 2F, rHeight * 2F); }

        public static Rectangle MakeRectangle(int X, int Y, int Size) { return new Rectangle(X - (Size / 2), Y - (Size / 2), Size, Size); }
        public static RectangleF MakeRectangle(float X, float Y, float Size) { return new RectangleF(X - (Size / 2F), Y - (Size / 2F), Size, Size); }
        #endregion
        #endregion

        #region GetPoints
        public static PointF[] GetPoints(RectangleF rt)
        {
            return new PointF[] { new PointF(rt.Left, rt.Top), new PointF(rt.Right, rt.Top), new PointF(rt.Right, rt.Bottom), new PointF(rt.Left, rt.Bottom) };
        }
        #endregion
        #region GetPoint
        public static PointF GetPointWithAngle(PointF p, float angle, float dist)
        {
            float x = GetX_WithAngle(p, angle, dist);
            float y = GetY_WithAngle(p, angle, dist);
            return new PointF(x, y);
        }
        public static float GetX_WithAngle(PointF p, float angle, float dist) { return p.X + dist * Convert.ToSingle(Math.Cos(angle * Math.PI / 180.0)); }
        public static float GetY_WithAngle(PointF p, float angle, float dist) { return p.Y + dist * Convert.ToSingle(Math.Sin(angle * Math.PI / 180.0)); }

        public static PointF GetPointWithAngle(Point p, float angle, float dist)
        {
            float x = GetX_WithAngle(p, angle, dist);
            float y = GetY_WithAngle(p, angle, dist);
            return new PointF(x, y);
        }
        public static float GetX_WithAngle(Point p, float angle, float dist) { return p.X + dist * Convert.ToSingle(Math.Cos(angle * Math.PI / 180.0)); }
        public static float GetY_WithAngle(Point p, float angle, float dist) { return p.Y + dist * Convert.ToSingle(Math.Sin(angle * Math.PI / 180.0)); }
        #endregion

        #region LinearEquation
        public static float LinearEquationY(PointF p1, PointF p2, float x)
        {
            float x1 = p1.X, x2 = p2.X, y1 = p1.Y, y2 = p2.Y;
            float a = (y2 - y1) / (x2 - x1);
            float b = y1 - a * x1;
            float y = a * x + b;

            return y;
        }

        public static float LinearEquationX(PointF p1, PointF p2, float y)
        {
            float x1 = p1.X, x2 = p2.X, y1 = p1.Y, y2 = p2.Y;
            float a = (y2 - y1) / (x2 - x1);
            float b = y1 - a * x1;
            float x = (y - b) / a;

            return x;
        }
        #endregion
    }
}
