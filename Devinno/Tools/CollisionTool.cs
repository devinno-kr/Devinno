using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Tools
{
    public class CollisionTool
    {
        #region Check
        #region Check(Rectangle, X, Y)
        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="pt">좌표</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(Rectangle rt, Point pt) => CollisionTool.Check(rt, pt.X, pt.Y);
        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="pt">좌표</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(Rectangle rt, PointF pt) => CollisionTool.Check(rt, pt.X, pt.Y);
        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="pt">좌표</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(RectangleF rt, Point pt) => CollisionTool.Check(rt, pt.X, pt.Y);
        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="pt">좌표</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(RectangleF rt, PointF pt) => CollisionTool.Check(rt, pt.X, pt.Y);


        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="x">좌표 X</param>
        /// <param name="y">좌표 Y</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(RectangleF rt, float x, float y) => rt.X < x && rt.Y < y && rt.X + rt.Width > x && rt.Y + rt.Height > y;
        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="x">좌표 X</param>
        /// <param name="y">좌표 Y</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(RectangleF rt, int x, int y) => rt.X < x && rt.Y < y && rt.X + rt.Width > x && rt.Y + rt.Height > y;
        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="x">좌표 X</param>
        /// <param name="y">좌표 Y</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(Rectangle rt, int x, int y) => rt.X < x && rt.Y < y && rt.X + rt.Width > x && rt.Y + rt.Height > y;
        /// <summary>
        /// 사각형과 한 점간 충돌체크
        /// </summary>
        /// <param name="rt">사각형</param>
        /// <param name="x">좌표 X</param>
        /// <param name="y">좌표 Y</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(Rectangle rt, float x, float y) => rt.X < x && rt.Y < y && rt.X + rt.Width > x && rt.Y + rt.Height > y;
        #endregion
        #region Check(Rectangle, Rectangle)
        /// <summary>
        /// 서로 다른 두 사각형간 충돌 체크 
        /// </summary>
        /// <param name="rt1">사각형1</param>
        /// <param name="rt2">사각형2</param>
        /// <returns>충돌 여부</returns>
        public static bool Check(Rectangle rt1, Rectangle rt2) => (rt2.Right >= rt1.Left && rt1.Right >= rt2.Left && rt2.Bottom >= rt1.Top && rt1.Bottom >= rt2.Top);
        public static bool Check(RectangleF rt1, Rectangle rt2) => (rt2.Right >= rt1.Left && rt1.Right >= rt2.Left && rt2.Bottom >= rt1.Top && rt1.Bottom >= rt2.Top);
        public static bool Check(Rectangle rt1, RectangleF rt2) => (rt2.Right >= rt1.Left && rt1.Right >= rt2.Left && rt2.Bottom >= rt1.Top && rt1.Bottom >= rt2.Top);
        public static bool Check(RectangleF rt1, RectangleF rt2) => (rt2.Right >= rt1.Left && rt1.Right >= rt2.Left && rt2.Bottom >= rt1.Top && rt1.Bottom >= rt2.Top);
        #endregion
        #region CheckCircle(Rectangle, Point)
        /// <summary>
        /// 원과 한 점간 충돌 체크
        /// </summary>
        /// <param name="rt1">원</param>
        /// <param name="pt">점</param>
        /// <returns>충돌여부</returns>
        public static bool CheckCircle(Rectangle rt1, Point pt)
        {
            var gap = Math.Min(rt1.Width, rt1.Height) / 2;
            var v = MathTool.GetDistance(MathTool.CenterPoint(rt1), pt);
            return v < gap;
        }
        /// <summary>
        /// 원과 한 점간 충돌 체크
        /// </summary>
        /// <param name="rt1">원</param>
        /// <param name="pt">점</param>
        /// <returns>충돌여부</returns>
        public static bool CheckCircle(RectangleF rt1, PointF pt)
        {
            var gap = Math.Min(rt1.Width, rt1.Height) / 2;
            var v = MathTool.GetDistance(MathTool.CenterPoint(rt1), pt);
            return v < gap;
        }
        /// <summary>
        /// 원과 한 점간 충돌 체크
        /// </summary>
        /// <param name="rt1">원</param>
        /// <param name="pt">점</param>
        /// <returns>충돌여부</returns>
        public static bool CheckCircle(Rectangle rt1, PointF pt)
        {
            var gap = Math.Min(rt1.Width, rt1.Height) / 2;
            var v = MathTool.GetDistance(MathTool.CenterPoint(rt1), pt);
            return v < gap;
        }
        /// <summary>
        /// 원과 한 점간 충돌 체크
        /// </summary>
        /// <param name="rt1">원</param>
        /// <param name="pt">점</param>
        /// <returns>충돌여부</returns>
        public static bool CheckCircle(RectangleF rt1, Point pt)
        {
            var gap = Math.Min(rt1.Width, rt1.Height) / 2;
            var v = MathTool.GetDistance(MathTool.CenterPoint(rt1), pt);
            return v < gap;
        }
        #endregion
        #region CheckEllipse
        /// <summary>
        /// 타원과 한 점간의 충돌 체크 
        /// </summary>
        /// <param name="ellipse">타원</param>
        /// <param name="pt">좌표</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckEllipse(Rectangle ellipse, Point pt)
        {
            EllipseCollision e = new EllipseCollision(10);
            var cp = MathTool.CenterPoint(ellipse);
            return e.Collide(cp.X, cp.Y, ellipse.Width / 2, ellipse.Height / 2, pt.X, pt.Y, 1);
        }
        /// <summary>
        /// 타원과 한 점간의 충돌 체크 
        /// </summary>
        /// <param name="ellipse">타원</param>
        /// <param name="pt">좌표</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckEllipse(RectangleF ellipse, PointF pt)
        {
            EllipseCollision e = new EllipseCollision(10);
            var cp = MathTool.CenterPoint(ellipse);
            return e.Collide(cp.X, cp.Y, ellipse.Width / 2F, ellipse.Height / 2F, pt.X, pt.Y, 0.1);
        }
        /// <summary>
        /// 서로 다른 두 타원간의 충돌 체크 
        /// </summary>
        /// <param name="ellipse1">타원1</param>
        /// <param name="ellipse2">타원2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckEllipse(RectangleF ellipse1, Rectangle ellipse2)
        {
            EllipseCollision e = new EllipseCollision(10);
            var cp = MathTool.CenterPoint(ellipse1);
            var cpT = MathTool.CenterPoint(ellipse2);
            return e.Collide(cp.X, cp.Y, ellipse1.Width / 2F, ellipse1.Height / 2, cpT.X, cpT.Y, ellipse2.Width / 2F, ellipse2.Height / 2F);
        }
        #endregion
        #region CheckVertical
        /// <summary>
        /// 두 사각형간 Y좌표만 기준으로 충돌 체크
        /// </summary>
        /// <param name="rt1">사각형1</param>
        /// <param name="rt2">사각형2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckVertical(Rectangle rt1, Rectangle rt2) { return (rt2.Bottom >= rt1.Top && rt1.Bottom >= rt2.Top); }
        /// <summary>
        /// 두 사각형간 Y좌표만 기준으로 충돌 체크
        /// </summary>
        /// <param name="rt1">사각형1</param>
        /// <param name="rt2">사각형2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckVertical(RectangleF rt1, RectangleF rt2) { return (rt2.Bottom >= rt1.Top && rt1.Bottom >= rt2.Top); }
        /// <summary>
        /// 두 상/하단이 겹치는지 충돌 체크
        /// </summary>
        /// <param name="Top1">상단1</param>
        /// <param name="Bottom1">하단1</param>
        /// <param name="Top2">상단2</param>
        /// <param name="Bottom2">하단2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckVertical(int Top1, int Bottom1, int Top2, int Bottom2) { return (Bottom2 >= Top1 && Bottom1 >= Top2); }
        /// <summary>
        /// 두 상/하단이 겹치는지 충돌 체크
        /// </summary>
        /// <param name="Top1">상단1</param>
        /// <param name="Bottom1">하단1</param>
        /// <param name="Top2">상단2</param>
        /// <param name="Bottom2">하단2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckVertical(float Top1, float Bottom1, float Top2, float Bottom2) { return (Bottom2 >= Top1 && Bottom1 >= Top2); }
        #endregion
        #region CheckHorizon
        /// <summary>
        /// 두 사각형간 X좌표만 기준으로 충돌 체크
        /// </summary>
        /// <param name="rt1">사각형1</param>
        /// <param name="rt2">사각형2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckHorizon(Rectangle rt1, Rectangle rt2) { return (rt2.Right >= rt1.Left && rt1.Right >= rt2.Left); }
        /// <summary>
        /// 두 사각형간 X좌표만 기준으로 충돌 체크
        /// </summary>
        /// <param name="rt1">사각형1</param>
        /// <param name="rt2">사각형2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckHorizon(RectangleF rt1, RectangleF rt2) { return (rt2.Right >= rt1.Left && rt1.Right >= rt2.Left); }
        /// <summary>
        /// 두 좌/우측이 겹치는지 충돌 체크
        /// </summary>
        /// <param name="Top1">좌측1</param>
        /// <param name="Bottom1">우측1</param>
        /// <param name="Top2">좌측2</param>
        /// <param name="Bottom2">우측2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckHorizon(int Left1, int Right1, int Left2, int Right2) { return (Right2 >= Left1 && Right1 >= Left2); }
        /// <summary>
        /// 두 좌/우측이 겹치는지 충돌 체크
        /// </summary>
        /// <param name="Top1">좌측1</param>
        /// <param name="Bottom1">우측1</param>
        /// <param name="Top2">좌측2</param>
        /// <param name="Bottom2">우측2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckHorizon(float Left1, float Right1, float Left2, float Right2) { return (Right2 >= Left1 && Right1 >= Left2); }
        #endregion
        #region CheckLine
        /// <summary>
        /// 선분이 지정한 중점을 기준으로한 반경안에 드는지 충돌 체크 
        /// </summary>
        /// <param name="p1">선분 좌표1</param>
        /// <param name="p2">선분 좌표2</param>
        /// <param name="Location">중점</param>
        /// <param name="Dist">반경</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckLine(Point p1, Point p2, Point Location, float Dist) { return Math.Abs(MathTool.GetDistance(p1, p2, Location)) < Dist; }

        /// <summary>
        /// 선분이 지정한 중점을 기준으로한 반경안에 드는지 충돌 체크 
        /// </summary>
        /// <param name="p1">선분 좌표1</param>
        /// <param name="p2">선분 좌표2</param>
        /// <param name="Location">중점</param>
        /// <param name="Dist">반경</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckLine(PointF p1, PointF p2, PointF Location, float Dist) { return Math.Abs(MathTool.GetDistance(p1, p2, Location)) < Dist; }

        /// <summary>
        /// 선분이 사각형을 지나는지 충돌 체크
        /// </summary>
        /// <param name="p1">선분 좌표1</param>
        /// <param name="p2">선분 좌표2</param>
        /// <param name="rt">사각형</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckLine(PointF p1, PointF p2, RectangleF rt)
        {
            var left = CheckLine(p1, p2, new PointF(rt.Left, rt.Top), new PointF(rt.Left, rt.Bottom));
            var right = CheckLine(p1, p2, new PointF(rt.Right, rt.Top), new PointF(rt.Right, rt.Bottom));
            var top = CheckLine(p1, p2, new PointF(rt.Left, rt.Top), new PointF(rt.Right, rt.Top));
            var bottom = CheckLine(p1, p2, new PointF(rt.Left, rt.Bottom), new PointF(rt.Right, rt.Bottom));

            return left || right || top || bottom || (rt.Contains(p1) || rt.Contains(p2));
        }

        /// <summary>
        /// 서로 다른 두 선분이 서로 교차되는지 충돌 체크
        /// </summary>
        /// <param name="Line1Start">선분1 시작점</param>
        /// <param name="Line1End">선분1 끝점</param>
        /// <param name="Line2Start">선분2 시작점</param>
        /// <param name="Line2End">선분2 끝점</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckLine(PointF Line1Start, PointF Line1End, PointF Line2Start, PointF Line2End)
        {
            var x1 = Line1Start.X;
            var y1 = Line1Start.Y;
            var x2 = Line1End.X;
            var y2 = Line1End.Y;
            var x3 = Line2Start.X;
            var y3 = Line2Start.Y;
            var x4 = Line2End.X;
            var y4 = Line2End.Y;


            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1) return true;
            return false;
        }
        #endregion
        #region CheckPolygon
        /// <summary>
        /// 서로 다른 두 다각형의 충돌 체크
        /// </summary>
        /// <param name="poly1">다각형 1</param>
        /// <param name="poly2">다각형 2</param>
        /// <returns>충돌 여부</returns>
        public static bool CheckPolygon(PointF[] poly1, PointF[] poly2)
        {
            PolygonForCollision p1 = new PolygonForCollision();
            PolygonForCollision p2 = new PolygonForCollision();
            p1.Points.AddRange(poly1.Select(x => new VectorForCollision(x.X, x.Y)));
            p2.Points.AddRange(poly2.Select(x => new VectorForCollision(x.X, x.Y)));
            p1.BuildEdges();
            p2.BuildEdges();
            return PolygonCollision(p1, p2);
        }

        #region Polygon
        static bool PolygonCollision(PolygonForCollision polygonA, PolygonForCollision polygonB)
        {
            bool Intersect = true;

            int edgeCountA = polygonA.Edges.Count;
            int edgeCountB = polygonB.Edges.Count;
            VectorForCollision edge;

            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = polygonA.Edges[edgeIndex];
                }
                else
                {
                    edge = polygonB.Edges[edgeIndex - edgeCountA];
                }

                VectorForCollision axis = new VectorForCollision(-edge.Y, edge.X);
                axis.Normalize();

                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                if (IntervalDistance(minA, maxA, minB, maxB) > 0) Intersect = false;
            }

            return Intersect;
        }

        static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        static void ProjectPolygon(VectorForCollision axis, PolygonForCollision polygon, ref float min, ref float max)
        {
            float d = axis.DotProduct(polygon.Points[0]);
            min = d;
            max = d;
            for (int i = 0; i < polygon.Points.Count; i++)
            {
                d = polygon.Points[i].DotProduct(axis);
                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }
        #endregion
        #endregion
        #endregion

    }

    #region EllipseCollision
    internal class EllipseCollision
    {
        #region Member Variable
        private double[] innerPolygonCoef;
        private double[] outerPolygonCoef;
        private int maxIterations;
        #endregion

        #region Constructor
        public EllipseCollision(int maxIterations)
        {
            this.maxIterations = maxIterations;
            innerPolygonCoef = new double[maxIterations + 1];
            outerPolygonCoef = new double[maxIterations + 1];
            for (int t = 0; t <= maxIterations; t++)
            {
                int numNodes = 4 << t;
                innerPolygonCoef[t] = 0.5 / Math.Cos(4 * Math.Acos(0.0) / numNodes);
                outerPolygonCoef[t] = 0.5 / (Math.Cos(2 * Math.Acos(0.0) / numNodes) * Math.Cos(2 * Math.Acos(0.0) / numNodes));
            }
        }
        #endregion

        #region Method
        #region Iterate
        bool Iterate(double x, double y, double c0x, double c0y, double c2x, double c2y, double rr)
        {
            for (int t = 1; t <= maxIterations; t++)
            {
                double c1x = (c0x + c2x) * innerPolygonCoef[t];
                double c1y = (c0y + c2y) * innerPolygonCoef[t];
                double tx = x - c1x;
                double ty = y - c1y;
                if (tx * tx + ty * ty <= rr)
                {
                    return true;
                }
                double t2x = c2x - c1x;
                double t2y = c2y - c1y;
                if (tx * t2x + ty * t2y >= 0 && tx * t2x + ty * t2y <= t2x * t2x + t2y * t2y &&
                (ty * t2x - tx * t2y >= 0 || rr * (t2x * t2x + t2y * t2y) >= (ty * t2x - tx * t2y) * (ty * t2x - tx * t2y)))
                {
                    return true;
                }
                double t0x = c0x - c1x;
                double t0y = c0y - c1y;
                if (tx * t0x + ty * t0y >= 0 && tx * t0x + ty * t0y <= t0x * t0x + t0y * t0y &&
                (ty * t0x - tx * t0y <= 0 || rr * (t0x * t0x + t0y * t0y) >= (ty * t0x - tx * t0y) * (ty * t0x - tx * t0y)))
                {
                    return true;
                }
                double c3x = (c0x + c1x) * outerPolygonCoef[t];
                double c3y = (c0y + c1y) * outerPolygonCoef[t];
                if ((c3x - x) * (c3x - x) + (c3y - y) * (c3y - y) < rr)
                {
                    c2x = c1x;
                    c2y = c1y;
                    continue;
                }
                double c4x = c1x - c3x + c1x;
                double c4y = c1y - c3y + c1y;
                if ((c4x - x) * (c4x - x) + (c4y - y) * (c4y - y) < rr)
                {
                    c0x = c1x;
                    c0y = c1y;
                    continue;
                }
                double t3x = c3x - c1x;
                double t3y = c3y - c1y;
                if (ty * t3x - tx * t3y <= 0 || rr * (t3x * t3x + t3y * t3y) > (ty * t3x - tx * t3y) * (ty * t3x - tx * t3y))
                {
                    if (tx * t3x + ty * t3y > 0)
                    {
                        if (Math.Abs(tx * t3x + ty * t3y) <= t3x * t3x + t3y * t3y || (x - c3x) * (c0x - c3x) + (y - c3y) * (c0y - c3y) >= 0)
                        {
                            c2x = c1x;
                            c2y = c1y;
                            continue;
                        }
                    }
                    else if (-(tx * t3x + ty * t3y) <= t3x * t3x + t3y * t3y || (x - c4x) * (c2x - c4x) + (y - c4y) * (c2y - c4y) >= 0)
                    {
                        c0x = c1x;
                        c0y = c1y;
                        continue;
                    }
                }
                return false;
            }
            return false;
        }
        #endregion
        #region Collide
        public bool Collide(double x0, double y0, double wx0, double wy0, double hw0,
                     double x1, double y1, double wx1, double wy1, double hw1)
        {
            double rr = hw1 * hw1 * (wx1 * wx1 + wy1 * wy1) * (wx1 * wx1 + wy1 * wy1) * (wx1 * wx1 + wy1 * wy1);
            double x = hw1 * wx1 * (wy1 * (y1 - y0) + wx1 * (x1 - x0)) - wy1 * (wx1 * (y1 - y0) - wy1 * (x1 - x0));
            double y = hw1 * wy1 * (wy1 * (y1 - y0) + wx1 * (x1 - x0)) + wx1 * (wx1 * (y1 - y0) - wy1 * (x1 - x0));
            double temp = wx0;
            wx0 = hw1 * wx1 * (wy1 * wy0 + wx1 * wx0) - wy1 * (wx1 * wy0 - wy1 * wx0);
            double temp2 = wy0;
            wy0 = hw1 * wy1 * (wy1 * wy0 + wx1 * temp) + wx1 * (wx1 * wy0 - wy1 * temp);
            double hx0 = hw1 * wx1 * (wy1 * (temp * hw0) - wx1 * temp2 * hw0) - wy1 * (wx1 * (temp * hw0) + wy1 * temp2 * hw0);
            double hy0 = hw1 * wy1 * (wy1 * (temp * hw0) - wx1 * temp2 * hw0) + wx1 * (wx1 * (temp * hw0) + wy1 * temp2 * hw0);

            if (wx0 * y - wy0 * x < 0)
            {
                x = -x;
                y = -y;
            }

            if ((wx0 - x) * (wx0 - x) + (wy0 - y) * (wy0 - y) <= rr)
            {
                return true;
            }
            else if ((wx0 + x) * (wx0 + x) + (wy0 + y) * (wy0 + y) <= rr)
            {
                return true;
            }
            else if ((hx0 - x) * (hx0 - x) + (hy0 - y) * (hy0 - y) <= rr)
            {
                return true;
            }
            else if ((hx0 + x) * (hx0 + x) + (hy0 + y) * (hy0 + y) <= rr)
            {
                return true;
            }
            else if (x * (hy0 - wy0) + y * (wx0 - hx0) <= hy0 * wx0 - hx0 * wy0 &&
                 y * (wx0 + hx0) - x * (wy0 + hy0) <= hy0 * wx0 - hx0 * wy0)
            {
                return true;
            }
            else if (x * (wx0 - hx0) - y * (hy0 - wy0) > hx0 * (wx0 - hx0) - hy0 * (hy0 - wy0)
                 && x * (wx0 - hx0) - y * (hy0 - wy0) < wx0 * (wx0 - hx0) - wy0 * (hy0 - wy0)
                 && (x * (hy0 - wy0) + y * (wx0 - hx0) - hy0 * wx0 + hx0 * wy0) * (x * (hy0 - wy0) + y * (wx0 - hx0) - hy0 * wx0 + hx0 * wy0)
                 <= rr * ((wx0 - hx0) * (wx0 - hx0) + (wy0 - hy0) * (wy0 - hy0)))
            {
                return true;
            }
            else if (x * (wx0 + hx0) + y * (wy0 + hy0) > -wx0 * (wx0 + hx0) - wy0 * (wy0 + hy0)
                 && x * (wx0 + hx0) + y * (wy0 + hy0) < hx0 * (wx0 + hx0) + hy0 * (wy0 + hy0)
                 && (y * (wx0 + hx0) - x * (wy0 + hy0) - hy0 * wx0 + hx0 * wy0) * (y * (wx0 + hx0) - x * (wy0 + hy0) - hy0 * wx0 + hx0 * wy0)
                 <= rr * ((wx0 + hx0) * (wx0 + hx0) + (wy0 + hy0) * (wy0 + hy0)))
            {
                return true;
            }
            else
            {
                if ((hx0 - wx0 - x) * (hx0 - wx0 - x) + (hy0 - wy0 - y) * (hy0 - wy0 - y) <= rr)
                {
                    return Iterate(x, y, hx0, hy0, -wx0, -wy0, rr);
                }
                else if ((hx0 + wx0 - x) * (hx0 + wx0 - x) + (hy0 + wy0 - y) * (hy0 + wy0 - y) <= rr)
                {
                    return Iterate(x, y, wx0, wy0, hx0, hy0, rr);
                }
                else if ((wx0 - hx0 - x) * (wx0 - hx0 - x) + (wy0 - hy0 - y) * (wy0 - hy0 - y) <= rr)
                {
                    return Iterate(x, y, -hx0, -hy0, wx0, wy0, rr);
                }
                else if ((-wx0 - hx0 - x) * (-wx0 - hx0 - x) + (-wy0 - hy0 - y) * (-wy0 - hy0 - y) <= rr)
                {
                    return Iterate(x, y, -wx0, -wy0, -hx0, -hy0, rr);
                }
                else if (wx0 * y - wy0 * x < wx0 * hy0 - wy0 * hx0 && Math.Abs(hx0 * y - hy0 * x) < hy0 * wx0 - hx0 * wy0)
                {
                    if (hx0 * y - hy0 * x > 0)
                    {
                        return Iterate(x, y, hx0, hy0, -wx0, -wy0, rr);
                    }
                    return Iterate(x, y, wx0, wy0, hx0, hy0, rr);
                }
                else if (wx0 * x + wy0 * y > wx0 * (hx0 - wx0) + wy0 * (hy0 - wy0) && wx0 * x + wy0 * y < wx0 * (hx0 + wx0) + wy0 * (hy0 + wy0)
                 && (wx0 * y - wy0 * x - hy0 * wx0 + hx0 * wy0) * (wx0 * y - wy0 * x - hy0 * wx0 + hx0 * wy0) < rr * (wx0 * wx0 + wy0 * wy0))
                {
                    if (wx0 * x + wy0 * y > wx0 * hx0 + wy0 * hy0)
                    {
                        return Iterate(x, y, wx0, wy0, hx0, hy0, rr);
                    }
                    return Iterate(x, y, hx0, hy0, -wx0, -wy0, rr);
                }
                else
                {
                    if (hx0 * y - hy0 * x < 0)
                    {
                        x = -x;
                        y = -y;
                    }
                    if (hx0 * x + hy0 * y > -hx0 * (wx0 + hx0) - hy0 * (wy0 + hy0) && hx0 * x + hy0 * y < hx0 * (hx0 - wx0) + hy0 * (hy0 - wy0)
                        && (hx0 * y - hy0 * x - hy0 * wx0 + hx0 * wy0) * (hx0 * y - hy0 * x - hy0 * wx0 + hx0 * wy0) < rr * (hx0 * hx0 + hy0 * hy0))
                    {
                        if (hx0 * x + hy0 * y > -hx0 * wx0 - hy0 * wy0)
                        {
                            return Iterate(x, y, hx0, hy0, -wx0, -wy0, rr);
                        }
                        return Iterate(x, y, -wx0, -wy0, -hx0, -hy0, rr);
                    }
                    return false;
                }
            }
        }

        public bool Collide(double x0, double y0, double w0, double h0, double x1, double y1, double w1, double h1)
        {

            double x = Math.Abs(x1 - x0) * h1;
            double y = Math.Abs(y1 - y0) * w1;
            w0 *= h1;
            h0 *= w1;
            double r = w1 * h1;

            if (x * x + (h0 - y) * (h0 - y) <= r * r || (w0 - x) * (w0 - x) + y * y <= r * r || x * h0 + y * w0 <= w0 * h0
                || ((x * h0 + y * w0 - w0 * h0) * (x * h0 + y * w0 - w0 * h0) <= r * r * (w0 * w0 + h0 * h0) && x * w0 - y * h0 >= -h0 * h0 && x * w0 - y * h0 <= w0 * w0))
            {
                return true;
            }
            else
            {
                if ((x - w0) * (x - w0) + (y - h0) * (y - h0) <= r * r || (x <= w0 && y - r <= h0) || (y <= h0 && x - r <= w0))
                {
                    return Iterate(x, y, w0, 0, 0, h0, r * r);
                }
                return false;
            }
        }

        public bool Collide(double x0, double y0, double w, double h, double x1, double y1, double r)
        {

            double x = Math.Abs(x1 - x0);
            double y = Math.Abs(y1 - y0);

            if (x * x + (h - y) * (h - y) <= r * r || (w - x) * (w - x) + y * y <= r * r || x * h + y * w <= w * h
            || ((x * h + y * w - w * h) * (x * h + y * w - w * h) <= r * r * (w * w + h * h) && x * w - y * h >= -h * h && x * w - y * h <= w * w))
            {
                return true;
            }
            else
            {
                if ((x - w) * (x - w) + (y - h) * (y - h) <= r * r || (x <= w && y - r <= h) || (y <= h && x - r <= w))
                {
                    return Iterate(x, y, w, 0, 0, h, r * r);
                }
                return false;
            }
        }
        #endregion
        #endregion
    }
    #endregion
    #region PolygonForCollision
    internal class PolygonForCollision
    {
        #region Member Variable
        private List<VectorForCollision> points = new List<VectorForCollision>();
        private List<VectorForCollision> edges = new List<VectorForCollision>();
        #endregion

        #region Constructor
        public void BuildEdges()
        {
            VectorForCollision p1;
            VectorForCollision p2;
            edges.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                p1 = points[i];
                if (i + 1 >= points.Count) p2 = points[0];
                else p2 = points[i + 1];
                edges.Add(p2 - p1);
            }
        }
        #endregion

        #region Properties
        public List<VectorForCollision> Edges => edges;
        public List<VectorForCollision> Points => points;
        public VectorForCollision Center
        {
            get
            {
                float totalX = 0;
                float totalY = 0;

                for (int i = 0; i < points.Count; i++)
                {
                    totalX += points[i].X;
                    totalY += points[i].Y;
                }

                return new VectorForCollision(totalX / (float)points.Count, totalY / (float)points.Count);
            }
        }
        #endregion

        #region Method
        public void Offset(VectorForCollision v)
        {
            Offset(v.X, v.Y);
        }

        public void Offset(float x, float y)
        {
            for (int i = 0; i < points.Count; i++)
            {
                VectorForCollision p = points[i];
                points[i] = new VectorForCollision(p.X + x, p.Y + y);
            }
        }
        #endregion

        #region Override
        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < points.Count; i++)
            {
                if (result != "") result += " ";
                result += "{" + points[i].ToString(true) + "}";
            }

            return result;
        }
        #endregion
    }
    #endregion
    #region VectorForCollision
    internal struct VectorForCollision
    {
        #region Member Variable
        public float X;
        public float Y;
        #endregion

        #region Static Method
        static public VectorForCollision FromPoint(Point p) => VectorForCollision.FromPoint(p.X, p.Y);
        static public VectorForCollision FromPoint(int x, int y) => new VectorForCollision((float)x, (float)y);
        #endregion

        #region Constructor
        public VectorForCollision(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion

        #region Properties
        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);
        #endregion

        #region Method
        public void Normalize()
        {
            float magnitude = Magnitude;
            X = X / magnitude;
            Y = Y / magnitude;
        }

        public VectorForCollision GetNormalized()
        {
            float magnitude = Magnitude;

            return new VectorForCollision(X / magnitude, Y / magnitude);
        }

        public float DotProduct(VectorForCollision vector)
        {
            return this.X * vector.X + this.Y * vector.Y;
        }

        public float DistanceTo(VectorForCollision vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector.X - this.X, 2) + Math.Pow(vector.Y - this.Y, 2));
        }
        #endregion

        #region Override
        public override bool Equals(object obj)
        {
            VectorForCollision v = (VectorForCollision)obj;

            return X == v.X && Y == v.Y;
        }

        public bool Equals(VectorForCollision v)
        {
            return X == v.X && Y == v.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }

        public string ToString(bool rounded)
        {
            if (rounded)
            {
                return (int)Math.Round(X) + ", " + (int)Math.Round(Y);
            }
            else
            {
                return ToString();
            }
        }

        #endregion

        #region Operator
        public static implicit operator Point(VectorForCollision p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        public static implicit operator PointF(VectorForCollision p)
        {
            return new PointF(p.X, p.Y);
        }

        public static VectorForCollision operator +(VectorForCollision a, VectorForCollision b)
        {
            return new VectorForCollision(a.X + b.X, a.Y + b.Y);
        }

        public static VectorForCollision operator -(VectorForCollision a)
        {
            return new VectorForCollision(-a.X, -a.Y);
        }

        public static VectorForCollision operator -(VectorForCollision a, VectorForCollision b)
        {
            return new VectorForCollision(a.X - b.X, a.Y - b.Y);
        }

        public static VectorForCollision operator *(VectorForCollision a, float b)
        {
            return new VectorForCollision(a.X * b, a.Y * b);
        }

        public static VectorForCollision operator *(VectorForCollision a, int b)
        {
            return new VectorForCollision(a.X * b, a.Y * b);
        }

        public static VectorForCollision operator *(VectorForCollision a, double b)
        {
            return new VectorForCollision((float)(a.X * b), (float)(a.Y * b));
        }

        public static bool operator ==(VectorForCollision a, VectorForCollision b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(VectorForCollision a, VectorForCollision b)
        {
            return a.X != b.X || a.Y != b.Y;
        }
        #endregion
    }
    #endregion
}
