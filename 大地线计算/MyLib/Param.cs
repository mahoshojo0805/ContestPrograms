using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Param
    {
        public double a;
        public double b;
        public double e2;
        public double edot2;
        //public double f;
        //public double N, M;
        public enum Tuoqiu { K,CGCS2000};
        public Tuoqiu ep = Tuoqiu.K;


        public Param()
        {
            SetParam(Tuoqiu.K);
        }
        public void SetParam(Tuoqiu t)
        {
            switch(t)
            {
                case Tuoqiu.K:
                    a = 6378245;
                    e2 = 0.00669342162297;
                    ep = Tuoqiu.K;
                    break;
                case Tuoqiu.CGCS2000:
                    a = 6378137;
                    e2 = 0.00669438002290;
                    ep = Tuoqiu.CGCS2000;
                    break;
            }
            edot2 = e2 / (1.0 - e2);
            b = Math.Sqrt(a * a - a * a * e2);
        }
    }
}
