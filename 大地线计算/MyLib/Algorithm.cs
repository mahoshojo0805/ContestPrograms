using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Algorithm
    {
        public static double GetGeodeticLine(in FileHelper file,in Param param)
        {
            //辅助计算
            double B1 = FileHelper.DDMMSSSSToRad(file.B[0]);
            double B2 = FileHelper.DDMMSSSSToRad(file.B[1]);
            double L1 = FileHelper.DDMMSSSSToRad(file.L[0]);
            double L2 = FileHelper.DDMMSSSSToRad(file.L[1]);
            double u1 = Math.Atan(Math.Sqrt(1 - param.e2) * Math.Tan(B1));
            double u2 = Math.Atan(Math.Sqrt(1 - param.e2) * Math.Tan(B2));
            double l = L2 - L1;
            double a1 = Math.Sin(u1) * Math.Sin(u2);
            double a2 = Math.Cos(u1) * Math.Cos(u2);
            double b1 = Math.Cos(u1) * Math.Sin(u2);
            double b2 = Math.Sin(u1) * Math.Cos(u2);

            //计算起点大地方位角
            double A1;
            double delta = 0;
            double lastdelta;
            double lamda = l;
            double theta;
            double sinA0;
            double cos2A0;
            double theta1;
            do
            {
                lastdelta = delta;
                double p = Math.Cos(u2) * Math.Sin(lamda);
                double q = b1 - b2 * Math.Cos(lamda);
                A1 = Math.Atan(p / q);
                if (p > 0)
                {
                    if (q > 0) A1 = Math.Abs(A1);
                    else if (q < 0) A1 = Math.PI - Math.Abs(A1);
                }
                else if (p < 0)
                {
                    if (q > 0) A1 = 2 * Math.PI - Math.Abs(A1);
                    else if (q < 0) A1 = Math.PI + Math.Abs(A1);
                }
                if (A1 < 0) A1 += 2 * Math.PI;
                if (A1 > 2 * Math.PI) A1 -= 2 * Math.PI;

                double sintheta = p * Math.Sin(A1) + q * Math.Cos(A1);
                double costheta = a1 + a2 * Math.Cos(lamda);
                theta = Math.Atan(sintheta / costheta);

                if (costheta > 0) theta = Math.Abs(theta);
                else if (costheta < 0) theta = Math.PI - Math.Abs(theta);

                sinA0 = Math.Cos(u1) * Math.Sin(A1);
                theta1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
                double e2 = param.e2;
                double e4 = e2 * e2;
                double e6 = e4 * e2;
                cos2A0 = 1 - sinA0 * sinA0;
                double cos4A0 = cos2A0 * cos2A0;
                double alpha = (e2 / 2 + e4 / 8 + e6 / 16) - (e4 / 16 + e6 / 16) * cos2A0 + e6 * 3 / 128 * cos4A0;
                double beta = (e4 / 16 + e6 / 16) * cos2A0 - e6 / 32 * cos4A0;
                double gama = e6 / 256 * cos4A0;
                delta = (alpha * theta + beta * Math.Cos(2 * theta1 + theta) * sintheta + gama * 2 * sintheta * costheta * Math.Cos(4 * theta1 + 2 * theta)) * sinA0;
                lamda = l + delta;
            } while (Math.Abs(delta - lastdelta) > 1e-10);
            
            //计算大地线长度S
            double k2 = param.edot2 * cos2A0;
            double k4 = k2 * k2;
            double k6 = k4 * k2;
            double A = (1.0 - k2 / 4 + 7 * k4 / 64 - 15.0 * k6 / 256) / param.b;
            double B = k2 / 4 - k4 / 8 + 37.0 * k6 / 512;
            double C = k4 / 128 - k6 / 128;

            double xs = C * Math.Sin(2 * theta) * Math.Cos(4.0 * theta1 + 2.0 * theta);
            double S = (theta - B * Math.Sin(theta) * Math.Cos(2.0 * theta1 + theta) - xs) / A;
            file.S = S;
            return S;
        }
    }
}
