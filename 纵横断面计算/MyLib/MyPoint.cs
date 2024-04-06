using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class MyPoint
    {
        public string name;
        public double x, y, h;
        public static double GetDistance2D(MyPoint P1,MyPoint P2)
        {
            return Math.Sqrt((P1.x - P2.x) * (P1.x - P2.x) + (P1.y - P2.y) * (P1.y - P2.y));
        }
    }
}
