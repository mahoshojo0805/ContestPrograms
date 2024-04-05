using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class MyPoint
    {
        public string ID;
        public double x, y;
        public double gx, gy;
        public static double GetDistance(in MyPoint p1,in MyPoint p2)
        {
            return Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
        }
    }
}
