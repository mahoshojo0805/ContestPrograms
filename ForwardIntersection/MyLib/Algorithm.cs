using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Algorithm
    {
        const double deg2rad = Math.PI / 180.0;
        public static void GetAuxiliaryCoor(ref FileHelper file)
        {
            for(int i=0;i<2;i++)
            {
                double cosphi=Math.Cos(file.pic[i].phi * deg2rad);
                double sinphi = Math.Sin(file.pic[i].phi * deg2rad);
                double cosomega = Math.Cos(file.pic[i].omega * deg2rad);
                double sinomega = Math.Sin(file.pic[i].omega * deg2rad);
                double cosk = Math.Cos(file.pic[i].k * deg2rad);
                double sink = Math.Sin(file.pic[i].k * deg2rad);

                double a1 = cosphi * cosk - cosphi * sinomega * sink;
                double a2 = -cosphi * sink - sinphi * sinomega * sink;
                double a3 = -sinphi * cosomega;

                double b1 = cosomega * sink;
                double b2 = cosomega * cosk;
                double b3 = -sinomega;

                double c1 = sinphi * cosk + cosphi * sinomega * sink;
                double c2 = -sinomega * cosk + cosphi * sinomega * sink;
                double c3 = cosphi * cosomega;

                file.pic[i].u = a1 * file.pic[i].x + a2 * file.pic[i].y - a3 * file.pic[i].f;
                file.pic[i].v = b1 * file.pic[i].x + b2 * file.pic[i].y - b3 * file.pic[i].f;
                file.pic[i].w = c1 * file.pic[i].x + c2 * file.pic[i].y - c3 * file.pic[i].f;
            }
        }
        public static void GetCoef(ref FileHelper file)
        {
            double BU = file.pic[1].Xs - file.pic[0].Xs;
            double BV = file.pic[1].Ys - file.pic[0].Ys;
            double BW = file.pic[1].Zs - file.pic[0].Zs;

            file.N1 = (BU * file.pic[1].w - BW * file.pic[1].u) / (file.pic[0].u * file.pic[1].w - file.pic[1].u * file.pic[0].w);
            file.N2 = (BU * file.pic[0].w - BW * file.pic[0].u) / (file.pic[0].u * file.pic[1].w - file.pic[1].u * file.pic[0].w);
        }

        public static void GetXYZ(ref FileHelper file)
        {
            file.X = file.pic[0].Xs + file.N1 * file.pic[0].u;
            file.Y = 0.5 * ((file.pic[0].Ys + file.N1 * file.pic[0].v) + (file.pic[1].Ys + file.N2 * file.pic[1].v));
            file.Z= file.pic[0].Zs + file.N1 * file.pic[0].w;
        }
    }
}
