using Devinno.Communications.Modbus.TCP;
using Devinno.Data;
using Devinno.Extensions;
using Devinno.Measure;
using Devinno.Timers;
using Devinno.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sample
{
    class Program
    {
        static void Print(string s, bool v) => Console.WriteLine(s + " : " + (v ? "충돌" : "미충돌"));
        static void Main(string[] args)
        {
            Print("Rect(0,0,80,20), Point(10,10)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Point(10, 10)));
            Print("Rect(0,0,80,20), Point(100,10)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Point(100, 10)));
            Print("Rect(0,0,80,20), Rect(10,10,30,30)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Rectangle(10, 10, 30, 30)));
            Print("Rect(0,0,80,20), Rect(100,10,30,30)", CollisionTool.Check(new Rectangle(0, 0, 80, 20), new Rectangle(100, 10, 30, 30)));
            Print("Circle(0,0,80,80), Point(30,30)", CollisionTool.CheckCircle(new Rectangle(0, 0, 80, 80), new Point(30, 30)));
            Print("Circle(0,0,80,80), Point(10,10)", CollisionTool.CheckCircle(new Rectangle(0, 0, 80, 80), new Point(10, 10)));
            Print("Ellipse(0,0,80,30), Ellipse(40,15,80,30)", CollisionTool.CheckEllipse(new Rectangle(0, 0, 80, 30), new Rectangle(40, 15, 80, 30)));
            Print("Ellipse(0,0,80,30), Ellipse(70,15,80,30)", CollisionTool.CheckEllipse(new Rectangle(0, 0, 80, 30), new Rectangle(70, 15, 80, 30)));
            Print("Line(0,0,30,30), Line(10,10,40,40)", CollisionTool.CheckLine(new Point(0, 0), new Point(30, 30), new Point(10, 10), new Point(40, 40)));
            Print("Line(0,0,30,30), Line(40,10,10,40)", CollisionTool.CheckLine(new Point(0, 0), new Point(30, 30), new Point(40, 10), new Point(10, 40)));
            Console.ReadKey();
        }
    }
}
