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
        /// <summary>
        /// 비례값 구하기
        /// </summary>
        /// <param name="val">현재값</param>
        /// <param name="min">현재 최소값</param>
        /// <param name="max">현재 최대값</param>
        /// <param name="convert_min">비례 최소값</param>
        /// <param name="convert_max">비례 최대값</param>
        /// <returns>비례값</returns>
        public static long Map(long val, long min, long max, long convert_min, long convert_max)
        {
            long ret = 0;
            if ((max - min) != 0) ret = (val - min) * (convert_max - convert_min) / (max - min) + convert_min;
            return ret;
        }

        /// <summary>
        /// 비례값 구하기
        /// </summary>
        /// <param name="val">현재값</param>
        /// <param name="min">현재 최소값</param>
        /// <param name="max">현재 최대값</param>
        /// <param name="convert_min">비례 최소값</param>
        /// <param name="convert_max">비례 최대값</param>
        /// <returns>비례값</returns>
        public static double Map(double val, double min, double max, double convert_min, double convert_max)
        {
            double ret = 0;
            if ((max - min) != 0) ret = (val - min) * (convert_max - convert_min) / (max - min) + convert_min;
            return ret;
        }
        #endregion

        #region Constrain
        /// <summary>
        /// 제한값 구하기
        /// </summary>
        /// <param name="val">현재값</param>
        /// <param name="min">최소값</param>
        /// <param name="max">최대값</param>
        /// <returns>제한값</returns>


        public static byte Constrain(byte val, byte min, byte max)
        {
            byte ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }

        public static short Constrain(short val, short min, short max)
        {
            short ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }

        public static int Constrain(int val, int min, int max)
        {
            int ret = val;
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

        public static sbyte Constrain(sbyte val, sbyte min, sbyte max)
        {
            sbyte ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }

        public static ushort Constrain(ushort val, ushort min, ushort max)
        {
            ushort ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }

        public static uint Constrain(uint val, uint min, uint max)
        {
            uint ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }

        public static ulong Constrain(ulong val, ulong min, ulong max)
        {
            ulong ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }


        /// <summary>
        /// 제한값 구하기
        /// </summary>
        /// <param name="val">현재값</param>
        /// <param name="min">최소값</param>
        /// <param name="max">최대값</param>
        /// <returns>제한값</returns>
        public static double Constrain(double val, double min, double max)
        {
            double ret = val;
            if (ret < min) ret = min;
            if (ret > max) ret = max;
            if (min > max) ret = min;
            return ret;
        }

        public static float Constrain(float val, float min, float max)
        {
            float ret = val;
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
        /// <summary>
        /// 시작점에서 끝점의 각도
        /// </summary>
        /// <param name="from">시작점</param>
        /// <param name="to">끝점</param>
        /// <returns>각독</returns>
        public static double GetAngle(Point from, Point to)
        {
            return Math.Atan2(to.Y - from.Y, to.X - from.X) * 180.0 / Math.PI;
        }

        /// <summary>
        /// 시작점에서 끝점의 각도
        /// </summary>
        /// <param name="from">시작점</param>
        /// <param name="to">끝점</param>
        /// <returns>각독</returns>
        public static double GetAngle(PointF from, PointF to)
        {
            return Math.Atan2(to.Y - from.Y, to.X - from.X) * 180.0 / Math.PI;
        }
        #endregion

        #region GetDistance
        /// <summary>
        /// 두 점 사이 거리
        /// </summary>
        /// <param name="a">점 1</param>
        /// <param name="b">점 2</param>
        /// <returns>거리</returns>
        public static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /// <summary>
        /// 두 점 사이 거리
        /// </summary>
        /// <param name="a">점 1</param>
        /// <param name="b">점 2</param>
        /// <returns>거리</returns>
        public static double GetDistance(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /// <summary>
        /// 선분과 한 점 사이 거리
        /// </summary>
        /// <param name="LN1">선분 시작점</param>
        /// <param name="LN2">선분 끝점</param>
        /// <param name="pt">점</param>
        /// <returns>거리</returns>
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
        /// <summary>
        /// 3차원 상 두점의 거리 구하기
        /// </summary>
        /// <param name="x1">점1 X</param>
        /// <param name="y1">점1 Y</param>
        /// <param name="z1">점1 Z</param>
        /// <param name="x2">점2 X</param>
        /// <param name="y2">점2 Y</param>
        /// <param name="z2">점2 Z</param>
        /// <returns></returns>
        public static double GetDistance(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double dz = z2 - z1;

            return Math.Sqrt(dx * dx + dy * dy * dz * dz);
        }

        public static double GetDistance(double[] first, double[] second)
        {
            var sum = first.Select((x, i) => (x - second[i]) * (x - second[i])).Sum();
            return Math.Sqrt(sum);
        }
        #endregion

        #region RotatePoint
        /// <summary>
        /// 중점을 기준으로 한점을 회전 시켰을때 좌표 
        /// </summary>
        /// <param name="Center">중점</param>
        /// <param name="Target">점</param>
        /// <param name="angle">회전각</param>
        /// <returns>회전 좌표</returns>
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

        #region Center
        /// <summary>
        /// p1 과 p2 사이 중간값
        /// </summary>
        /// <param name="p1">값1</param>
        /// <param name="p2">값2</param>
        /// <returns>중간값</returns>
        public static int Center(int p1, int p2) => p1 + ((p2 - p1) / 2);

        /// <summary>
        /// p1 과 p2 사이 중간값
        /// </summary>
        /// <param name="p1">값1</param>
        /// <param name="p2">값2</param>
        /// <returns>중간값</returns>
        public static float Center(float p1, float p2) => p1 + ((p2 - p1) / 2F);

        /// <summary>
        /// x에서 dist 거리 사이이 중간값
        /// </summary>
        /// <param name="x">값</param>
        /// <param name="dist">거리</param>
        /// <returns>중간값</returns>
        public static int CenterDist(int x, int dist) => x + (dist / 2);

        /// <summary>
        /// x에서 dist 거리 사이이 중간값
        /// </summary>
        /// <param name="x">값</param>
        /// <param name="dist">거리</param>
        /// <returns>중간값</returns>
        public static float CenterDist(float x, float dist) => x + (dist / 2F);
        #endregion

        #region CenterPoint
        #region CenterPoint ( Rectangle )
        /// <summary>
        /// 사각형의 중점 구하기
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <returns>중심 좌표</returns>
        public static Point CenterPoint(Rectangle rt)
        {
            return new Point(rt.X + rt.Width / 2, rt.Y + rt.Height / 2);
        }

        /// <summary>
        /// 사각형의 중점 구하기
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <returns>중심 좌표</returns>
        public static PointF CenterPoint(RectangleF rt)
        {
            return new PointF(rt.X + rt.Width / 2F, rt.Y + rt.Height / 2F);
        }
        #endregion
        #region CenterPoint ( p1, p2 )
        /// <summary>
        /// 두 점 사이 중점 구하기
        /// </summary>
        /// <param name="p1">점 1</param>
        /// <param name="p2">점 2</param>
        /// <returns>중심 좌표</returns>
        public static Point CenterPoint(Point p1, Point p2) => new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);

        /// <summary>
        /// 두 점 사이 중점 구하기
        /// </summary>
        /// <param name="p1">점 1</param>
        /// <param name="p2">점 2</param>
        /// <returns>중심 좌표</returns>
        public static PointF CenterPoint(PointF p1, PointF p2) => new PointF((p1.X + p2.X) / 2F, (p1.Y + p2.Y) / 2F);
        #endregion
        #region CenterPoint ( List<Point> )
        /// <summary>
        /// 지정한 좌표들의 중심 좌표 구하기
        /// </summary>
        /// <param name="sourceList">좌표 리스트</param>
        /// <returns>중심 좌표</returns>
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
        /// 지정한 사각형의 중심을 기준으로 지정한 크기의 사각형 생성
        /// </summary>
        /// <param name="rect">사각형</param>
        /// <param name="size">생성할 사각형 크기</param>
        /// <returns>생성한 사각형</returns>
        public static Rectangle MakeRectangle(Rectangle rect, Size size)
        {
            return new Rectangle(rect.X + (rect.Width / 2) - (size.Width / 2), rect.Y + (rect.Height / 2) - (size.Height / 2), size.Width, size.Height);
        }

        /// <summary>
        /// 지정한 사각형의 중심을 기준으로 지정한 크기의 사각형 생성
        /// </summary>
        /// <param name="rect">사각형</param>
        /// <param name="size">생성할 사각형 크기</param>
        /// <returns>생성한 사각형</returns>
        public static RectangleF MakeRectangle(RectangleF rect, SizeF size)
        {
            return new RectangleF(rect.X + (rect.Width / 2F) - (size.Width / 2F), rect.Y + (rect.Height / 2F) - (size.Height / 2F), size.Width, size.Height);
        }
        #endregion
        #region MakeRectangle ( Two Point )
        /// <summary>
        /// 두 점을 기준으로 사각형 생성
        /// </summary>
        /// <param name="pt1">점 1</param>
        /// <param name="pt2">점 2</param>
        /// <returns>생성한 사각형</returns>
        public static Rectangle MakeRectangle(Point pt1, Point pt2)
        {
            int minx = Math.Min(pt1.X, pt2.X);
            int miny = Math.Min(pt1.Y, pt2.Y);
            int maxx = Math.Max(pt1.X, pt2.X);
            int maxy = Math.Max(pt1.Y, pt2.Y);

            int rx = maxx - minx - 0; if (rx < 0) rx = 0;
            int ry = maxy - miny - 0; if (ry < 0) ry = 0;

            return new Rectangle(minx, miny, rx, ry);
        }
        /// <summary>
        /// 두 점을 기준으로 사각형 생성
        /// </summary>
        /// <param name="pt1">점 1</param>
        /// <param name="pt2">점 2</param>
        /// <returns>생성한 사각형</returns>
        public static RectangleF MakeRectangle(PointF pt1, PointF pt2)
        {
            var minx = Math.Min(pt1.X, pt2.X);
            var miny = Math.Min(pt1.Y, pt2.Y);
            var maxx = Math.Max(pt1.X, pt2.X);
            var maxy = Math.Max(pt1.Y, pt2.Y);

            var rx = maxx - minx - 0; if (rx < 0F) rx = 0F;
            var ry = maxy - miny - 0; if (ry < 0F) ry = 0F;

            return new RectangleF(minx, miny, rx, ry);
        }
        #endregion
        #region MakeRectangle ( Points ) 
        /// <summary>
        /// 지정한 좌표들을 포함하는 사각형 생성
        /// </summary>
        /// <param name="pts">좌표 리스트</param>
        /// <return>생성한 사각형</return>
        public static RectangleF MakeRectangle(IEnumerable<Point> pts)
        {
            if (pts.Count() >= 2)
            {
                float minx = pts.Min(x => x.X);
                float miny = pts.Min(x => x.Y);
                float maxx = pts.Max(x => x.X);
                float maxy = pts.Max(x => x.Y);

                float rx = maxx - minx - 0; if (rx < 0F) rx = 0F;
                float ry = maxy - miny - 0; if (ry < 0F) ry = 0F;

                return new RectangleF(minx, miny, rx, ry);
            }
            else throw new Exception("POINTS 개수가 2개 미만입니다.");
        }
        #endregion
        #region MakeRectangle ( Center Point )
        /// <summary>
        /// 지정한 점을 기준으로 지정한 크기의 정사각형 생성
        /// </summary>
        /// <param name="pt">점</param>
        /// <param name="Size">크기</param>
        /// <returns>생성한 사각형</returns>
        public static Rectangle MakeRectangle(Point pt, int Size) { return new Rectangle(pt.X - (Size / 2), pt.Y - (Size / 2), Size, Size); }
        
        /// <summary>
        /// 지정한 점을 기준으로 지정한 크기의 정사각형 생성
        /// </summary>
        /// <param name="pt">점</param>
        /// <param name="Size">크기</param>
        /// <returns>생성한 사각형</returns>
        public static RectangleF MakeRectangle(PointF pt, float Size) { return new RectangleF(pt.X - (Size / 2F), pt.Y - (Size / 2F), Size, Size); }

        /// <summary>
        /// 지정한 점을 기준으로 지정한 넓이와 높이의 사각형 생성
        /// </summary>
        /// <param name="pt">점</param>
        /// <param name="rWIdth">넓이</param>
        /// <param name="rHeight">높이</param>
        /// <returns>생성한 사각형</returns>
        public static Rectangle MakeRectangle(Point pt, int rWIdth, int rHeight) { return new Rectangle(pt.X - rWIdth, pt.Y - rHeight, rWIdth * 2, rHeight * 2); }

        /// <summary>
        /// 지정한 점을 기준으로 지정한 넓이와 높이의 사각형 생성
        /// </summary>
        /// <param name="pt">점</param>
        /// <param name="rWIdth">넓이</param>
        /// <param name="rHeight">높이</param>
        /// <returns>생성한 사각형</returns>
        public static RectangleF MakeRectangle(PointF pt, float rWIdth, float rHeight) { return new RectangleF(pt.X - rWIdth, pt.Y - rHeight, rWIdth * 2F, rHeight * 2F); }

        /// <summary>
        /// 지정한 점을 기준으로 지정한 크기의 정사각형 생성
        /// </summary>
        /// <param name="X">점 X좌표</param>
        /// <param name="Y">점 Y좌표</param>
        /// <param name="Size">크기</param>
        /// <returns>생성한 사각형</returns>
        public static Rectangle MakeRectangle(int X, int Y, int Size) { return new Rectangle(X - (Size / 2), Y - (Size / 2), Size, Size); }

        /// <summary>
        /// 지정한 점을 기준으로 지정한 크기의 정사각형 생성
        /// </summary>
        /// <param name="X">점 X좌표</param>
        /// <param name="Y">점 Y좌표</param>
        /// <param name="Size">크기</param>
        /// <returns>생성한 사각형</returns>
        public static RectangleF MakeRectangle(float X, float Y, float Size) { return new RectangleF(X - (Size / 2F), Y - (Size / 2F), Size, Size); }
        #endregion
        #endregion

        #region GetPoints
        /// <summary>
        /// 사각형의 네 점 반환
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <returns>네 점</returns>
        public static PointF[] GetPoints(RectangleF rt)
        {
            return new PointF[] { new PointF(rt.Left, rt.Top), new PointF(rt.Right, rt.Top), new PointF(rt.Right, rt.Bottom), new PointF(rt.Left, rt.Bottom) };
        }
        #endregion

        #region GetPoint
        /// <summary>
        /// 한 점을 기준으로 각도와 거리를 알 때 점의 위치   
        /// </summary>
        /// <param name="p">점</param>
        /// <param name="angle">각도</param>
        /// <param name="dist">거리</param>
        /// <returns>좌표</returns>
        public static PointF GetPointWithAngle(PointF p, float angle, float dist)
        {
            float x = GetX_WithAngle(p, angle, dist);
            float y = GetY_WithAngle(p, angle, dist);
            return new PointF(x, y);
        }

        /// <summary>
        /// 한 점을 기준으로 각도와 거리를 알 때 점의 위치 
        /// </summary>
        /// <param name="p">점</param>
        /// <param name="angle">각도</param>
        /// <param name="dist">거리</param>
        /// <returns>좌표</returns>
        public static PointF GetPointWithAngle(Point p, float angle, float dist)
        {
            float x = GetX_WithAngle(p, angle, dist);
            float y = GetY_WithAngle(p, angle, dist);
            return new PointF(x, y);
        }

        /// <summary>
        /// 한 점을 기준으로 각도와 거리를 알 때 점의 X좌료 위치 
        /// </summary>
        /// <param name="p">점</param>
        /// <param name="angle">각도</param>
        /// <param name="dist">거리</param>
        /// <returns>x 좌표</returns>
        public static float GetX_WithAngle(PointF p, float angle, float dist) { return p.X + dist * Convert.ToSingle(Math.Cos(angle * Math.PI / 180.0)); }

        /// <summary>
        /// 한 점을 기준으로 각도와 거리를 알 때 점의 Y좌표 위치 
        /// </summary>
        /// <param name="p">점</param>
        /// <param name="angle">각도</param>
        /// <param name="dist">거리</param>
        /// <returns>Y 좌표</returns>
        public static float GetY_WithAngle(PointF p, float angle, float dist) { return p.Y + dist * Convert.ToSingle(Math.Sin(angle * Math.PI / 180.0)); }

        /// <summary>
        /// 한 점을 기준으로 각도와 거리를 알 때 점의 X좌료 위치 
        /// </summary>
        /// <param name="p">점</param>
        /// <param name="angle">각도</param>
        /// <param name="dist">거리</param>
        /// <returns>X 좌표</returns>
        public static float GetX_WithAngle(Point p, float angle, float dist) { return p.X + dist * Convert.ToSingle(Math.Cos(angle * Math.PI / 180.0)); }

        /// <summary>
        /// 한 점을 기준으로 각도와 거리를 알 때 점의 Y좌표 위치 
        /// </summary>
        /// <param name="p">점</param>
        /// <param name="angle">각도</param>
        /// <param name="dist">거리</param>
        /// <returns>Y 좌표</returns>
        public static float GetY_WithAngle(Point p, float angle, float dist) { return p.Y + dist * Convert.ToSingle(Math.Sin(angle * Math.PI / 180.0)); }
        #endregion

        #region LinearEquation
        /// <summary>
        /// 두 점을 지나는 직선의 X좌표가 주어졌을시 Y좌표의 값
        /// </summary>
        /// <param name="p1">점 1</param>
        /// <param name="p2">점 2</param>
        /// <param name="x">X 좌표</param>
        /// <returns>Y좌표</returns>
        public static float LinearEquationY(PointF p1, PointF p2, float x)
        {
            float x1 = p1.X, x2 = p2.X, y1 = p1.Y, y2 = p2.Y;
            float a = (y2 - y1) / (x2 - x1);
            float b = y1 - a * x1;
            float y = a * x + b;

            return y;
        }

        /// <summary>
        /// 두 점을 지나는 직선의 Y좌표가 주어졌을시 X좌표의 값
        /// </summary>
        /// <param name="p1">점 1</param>
        /// <param name="p2">점 2</param>
        /// <param name="x">Y 좌표</param>
        /// <returns>X좌표</returns>
        public static float LinearEquationX(PointF p1, PointF p2, float y)
        {
            float x1 = p1.X, x2 = p2.X, y1 = p1.Y, y2 = p2.Y;
            float a = (y2 - y1) / (x2 - x1);
            float b = y1 - a * x1;
            float x = (y - b) / a;

            return x;
        }
        #endregion

        #region StandardAngle
        /// <summary>
        /// 지정한 각도를 0~360 사이의 각으로 변환 
        /// </summary>
        /// <param name="angle">각도</param>
        /// <returns>변환각</returns>
        public static int StandardAngle(int angle)
        {
            int ret = angle;

            var v = Convert.ToInt32(Math.Floor(Math.Abs(ret) / 360.0));

            if (ret > 360) ret -= 360 * v;
            if (ret < 0) ret += 360 * v;
            return ret;
        }

        /// <summary>
        /// 지정한 각도를 0~360 사이의 각으로 변환 
        /// </summary>
        /// <param name="angle">각도</param>
        /// <returns>변환각</returns>
        public static float StandardAngle(float angle)
        {
            float ret = angle;

            var v = Convert.ToInt32(Math.Floor(Math.Abs(ret) / 360.0));

            if (ret > 360) ret -= 360 * v;
            if (ret < 0) ret += 360 * v;
            return ret;
        }

        /// <summary>
        /// 지정한 각도를 0~360 사이의 각으로 변환 
        /// </summary>
        /// <param name="angle">각도</param>
        /// <returns>변환각</returns>
        public static double StandardAngle(double angle)
        {
            double ret = angle;

            var v = Convert.ToInt32(Math.Floor(Math.Abs(ret) / 360.0));

            if (ret > 360) ret -= 360 * v;
            if (ret < 0) ret += 360 * v;
            return ret;
        }
        #endregion

        #region CompareAngle
        /// <summary>
        /// 지정한 각도가 시작각과 끝각 사이에 있는지 확인
        /// </summary>
        /// <param name="Angle">각도</param>
        /// <param name="StartAngle">시작각</param>
        /// <param name="EndAngle">끝각</param>
        /// <returns>비교 결과</returns>
        public static bool CompareAngle(double Angle, double StartAngle, double EndAngle)
        {
            bool ret = false;

            var ang = MathTool.StandardAngle(Angle);
            var stang = MathTool.StandardAngle(StartAngle);
            var edang = MathTool.StandardAngle(EndAngle);
            if (stang > edang)
            {
                double s1 = stang - 360, e1 = edang;
                double s2 = stang, e2 = edang + 360;

                ret = (s1 <= ang && ang <= e1) || (s2 <= ang && ang <= e2);
            }
            else
            {
                ret = stang <= ang && ang <= edang;
            }
            return ret;
        }
        #endregion
    }
}
