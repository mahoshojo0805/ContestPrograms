using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Algorithm
    {
        public static string CeilDividePoints(ref DataHelper data)
        {
            string report = "";
            #region 栅格化数据
            double dx = 10;
            double dy = 10;
            data.ceils = new List<List<MyPoint>>();//分割成10X10
            for(int i=0;i<100;i++)
            {
                List<MyPoint> list = new List<MyPoint>();
                data.ceils.Add(list);
            }
            for(int k=0;k<data.points.Count;k++)
            {
                data.points[k].i = (int)Math.Floor(data.points[k].y / dy);
                data.points[k].j = (int)Math.Floor(data.points[k].x / dx);
                data.ceils[data.points[k].i * 10 + data.points[k].j].Add(data.points[k]);
            }
            #endregion
            report += string.Format("P5 点的所在栅格的行 i:{0:d}\n" +
                "P5 点的所在栅格的列 j:{1:d}\n" +
                "栅格 C 中的点的数量:{2:d}\n", data.points[4].i, data.points[4].j, data.ceils[3 * 10 + 2].Count);
            #region 计算栅格几何特征信息
            data.average_z = new double[10 * 10];
            data.Cmaxz = new double[10 * 10];
            data.Cminz = new double[10 * 10];
            data.sigmaz = new double[10 * 10];
            for(int i=0;i<10;i++)
            {
                for(int j=0;j<10;j++)
                {
                    data.average_z[i * 10 + j] = 0;
                    data.Cmaxz[i * 10 + j] = data.ceils[i * 10 + j][0].z;
                    data.Cminz[i * 10 + j] = data.ceils[i * 10 + j][0].z;
                    for (int k=0;k<data.ceils[i*10+j].Count;k++)
                    {
                        data.average_z[i * 10 + j] = data.average_z[i * 10 + j] * k / (k + 1) + data.ceils[i * 10 + j][k].z / (k + 1);
                        data.Cmaxz[i * 10 + j] = data.Cmaxz[i * 10 + j] > data.ceils[i * 10 + j][k].z ? data.Cmaxz[i * 10 + j] : data.ceils[i * 10 + j][k].z;
                        data.Cminz[i * 10 + j] = data.Cminz[i * 10 + j] < data.ceils[i * 10 + j][k].z ? data.Cminz[i * 10 + j] : data.ceils[i * 10 + j][k].z;
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    data.sigmaz[i * 10 + j] = 0;
                    for (int k = 0; k < data.ceils[i * 10 + j].Count; k++)
                    {
                        data.sigmaz[i * 10 + j] += Math.Pow(data.ceils[i * 10 + j][k].z - data.average_z[i * 10 + j], 2) / data.ceils[i * 10 + j].Count;
                    }
                }
            }
            #endregion
            report += string.Format("栅格 C 中的平均高度:{0:f3}\n" +
                "栅格 C 中高度的最大值:{1:f3}\n" +
                "栅格 C 中的高度差:{2:f3}\n" +
                "栅格 C 中的高度方差:{3:f3}\n", data.average_z[3 * 10 + 2], data.Cmaxz[3 * 10 + 2],
                data.Cmaxz[3 * 10 + 2] - data.Cminz[3 * 10 + 2], data.sigmaz[3 * 10 + 2]);
           return report;
        }
        public static string RANSAC(ref DataHelper data)
        {
            string report = "";
            double A1 = 0, B1 = 0, C1 = 0, D1 = 0;
            int idx = -1;
            int maxinter = 0;
            report += PlaneFit(ref data.points, 300, out A1, out B1, out C1, out D1, out idx, out maxinter);
            report += string.Format("最佳分割平面 J1 的参数 A:{0:f6}\n" +
                "最佳分割平面 J1 的参数 B:{1:f6}\n" +
                "最佳分割平面 J1 的参数 C:{2:f6}\n" +
                "最佳分割平面 J1 的参数 D:{3:f6}\n" +
                "最佳分割平面 J1 的内部点数量:{4:d}\n" +
                "最佳分割平面 J1 的外部点数量:{5:d}\n", A1, B1, C1, D1, maxinter, data.points.Count - 3 - maxinter);
            int idx1 = idx > data.points.Count ? idx : idx % data.points.Count;
            int idx2 = idx + 1 > data.points.Count ? (idx + 1) : (idx + 1) % data.points.Count;
            int idx3 = idx + 2 > data.points.Count ? (idx + 2) : (idx + 2) % data.points.Count;
            List <MyPoint> newpoints = new List<MyPoint>();
            for(int i=0;i<data.points.Count; i++)
            {
                if (i == idx1 || i == idx2 || i == idx3) continue;
                double d = Math.Abs(A1 * data.points[i].x + B1 * data.points[i].y + C1 * data.points[i].z + D1)
                    / Math.Sqrt(A1 * A1 + B1 * B1 + C1 * C1);
                if (d < 0.1)
                {
                    data.points[i].flag = 1;
                    continue;
                }
                newpoints.Add(data.points[i]);
            }
            double A2 = 0, B2 = 0, C2 = 0, D2 = 0;
            int idx_J2 = -1;
            int maxinter_J2 = 0;
            PlaneFit(ref newpoints, 80, out A2, out B2, out C2, out D2, out idx_J2, out maxinter_J2);
            for (int i = 0; i < newpoints.Count; i++)
            {
                if (i == idx1 || i == idx2 || i == idx3) continue;
                double d = Math.Abs(A2 * newpoints[i].x + B2 * newpoints[i].y + C2 * newpoints[i].z + D2)
                    / Math.Sqrt(A2 * A2 + B2 * B2 + C2 * C2);
                if (d < 0.1)
                {
                    data.points[i].flag = 2;
                    continue;
                }
            }
            report += string.Format("最佳分割平面 J2 的参数 A:{0:f6}\n" +
                "最佳分割平面 J2 的参数 B:{1:f6}\n" +
                "最佳分割平面 J2 的参数 C:{2:f6}\n" +
                "最佳分割平面 J2 的参数 D:{3:f6}\n" +
                "最佳分割平面 J2 的内部点数量:{4:d}\n" +
                "最佳分割平面 J2 的外部点数量:{5:d}\n", A2, B2, C2, D2, maxinter_J2, newpoints.Count - 3 - maxinter_J2);

            #region 计算投影
            for(int i=0;i<data.points.Count;i++)
            {
                switch(data.points[i].flag)
                {
                    case 0:
                        data.points[i].xt = data.points[i].x;
                        data.points[i].yt = data.points[i].y;
                        data.points[i].zt = data.points[i].z;
                        break;
                    case 1:
                        data.points[i].xt = ((B1 * B1 + C1 * C1) * data.points[i].x - A1 * (B1 * data.points[i].y + C1 * data.points[i].z + D1))
                            / (A1 * A1 + B1 * B1 + C1 * C1);
                        data.points[i].yt = ((A1 * A1 + C1 * C1) * data.points[i].y - B1 * (A1 * data.points[i].x + C1 * data.points[i].z + D1))
                            / (A1 * A1 + B1 * B1 + C1 * C1);
                        data.points[i].zt = ((A1 * A1 + B1 * B1) * data.points[i].z - C1 * (A1 * data.points[i].x + B1 * data.points[i].y + D1))
                            / (A1 * A1 + B1 * B1 + C1 * C1);
                        break;
                    case 2:
                        data.points[i].xt = ((B2 * B2 + C2 * C2) * data.points[i].x - A2 * (B2 * data.points[i].y + C2 * data.points[i].z + D2))
                            / (A2 * A2 + B2 * B2 + C2 * C2);
                        data.points[i].yt = ((A2 * A2 + C2 * C2) * data.points[i].y - B2 * (A2 * data.points[i].x + C2 * data.points[i].z + D2))
                            / (A2 * A2 + B2 * B2 + C2 * C2);
                        data.points[i].zt = ((A2 * A2 + B2 * B2) * data.points[i].z - C2 * (A2 * data.points[i].x + B2 * data.points[i].y + D2))
                            / (A2 * A2 + B2 * B2 + C2 * C2);
                        break;
                }
            }
            double p5xt = ((B1 * B1 + C1 * C1) * data.points[4].x - A1 * (B1 * data.points[4].y + C1 * data.points[4].z + D1))
                            / (A1 * A1 + B1 * B1 + C1 * C1);
            double p5yt = ((A1 * A1 + C1 * C1) * data.points[4].y - B1 * (A1 * data.points[4].x + C1 * data.points[4].z + D1))
                / (A1 * A1 + B1 * B1 + C1 * C1);
            double p5zt = ((A1 * A1 + B1 * B1) * data.points[4].z - C1 * (A1 * data.points[4].x + B1 * data.points[4].y + D1))
                / (A1 * A1 + B1 * B1 + C1 * C1);
            double p800xt = ((B2 * B2 + C2 * C2) * data.points[799].x - A2 * (B2 * data.points[799].y + C2 * data.points[799].z + D2))
                / (A2 * A2 + B2 * B2 + C2 * C2);
            double p800yt = ((A2 * A2 + C2 * C2) * data.points[799].y - B2 * (A2 * data.points[799].x + C2 * data.points[799].z + D2))
                / (A2 * A2 + B2 * B2 + C2 * C2);
            double p800zt = ((A2 * A2 + B2 * B2) * data.points[799].z - C2 * (A2 * data.points[799].x + B2 * data.points[799].y + D2))
                / (A2 * A2 + B2 * B2 + C2 * C2);
            report += string.Format("P5 点到最佳分割面（J1）的投影坐标 xt:{0:f3}\n" +
                "P5 点到最佳分割面（J1）的投影坐标 yt:{1:f3}\n" +
                "P5 点到最佳分割面（J1）的投影坐标 zt:{2:f3}\n" +
                "P800 点到最佳分割面（J2）的投影坐标 xt:{3:f3}\n" +
                "P800 点到最佳分割面（J2）的投影坐标 yt:{4:f3}\n" +
                "P800 点到最佳分割面（J2）的投影坐标 zt:{5:f3}\n",
                p5xt, p5yt, p5zt,
                p800xt, p800yt, p800zt);
            #endregion
            return report;
        }
        private static string PlaneFit(ref List<MyPoint> points, int iter_n,
            out double A,out double B, out double C, out double D,
            out int idx, out int maxinter)
        {
            string report = "";
            maxinter = 0;
            A = B = C = D = 0;
            idx = -1;
            for (int i = 0; i < iter_n; i++)//迭代iter_n次
            {
                #region 平面拟合
                int idx1 = i * 3 < points.Count ? i * 3 : (i * 3) % points.Count;
                int idx2 = i * 3 + 1 < points.Count ? i * 3 + 1 : (i * 3 + 1) % points.Count;
                int idx3 = i * 3 + 2 < points.Count ? i * 3 + 2 : (i * 3 + 2) % points.Count;
                double S = GetTriangleS(points[idx1], points[idx2], points[idx3]);
                if (S < 0.1)
                    continue;//三点共线
                double x1 = points[idx1].x; double y1 = points[idx1].y; double z1 = points[idx1].z;
                double x2 = points[idx2].x; double y2 = points[idx2].y; double z2 = points[idx2].z;
                double x3 = points[idx3].x; double y3 = points[idx3].y; double z3 = points[idx3].z;
                double A1 = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
                double B1 = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
                double C1 = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
                double D1 = -A1 * x1 - B1 * y1 - C1 * z1;
                if (i == 0)
                {
                    report += string.Format("P1-P2-P3 构成三角形的面积:{0:f6}\n" +
                        "拟合平面 S1 的参数 A:{1:f6}\n" +
                        "拟合平面 S1 的参数 B:{2:f6}\n" +
                        "拟合平面 S1 的参数 C:{3:f6}\n" +
                        "拟合平面 S1 的参数 D:{4:f6}\n", S, A1, B1, C1, D1);
                }
                #endregion
                #region 统计内部点和外部点
                int inter = 0;
                int outer = 0;
                for (int j = 0; j < points.Count; j++)
                {
                    if (j == idx1 || j == idx2 || j == idx3) continue;
                    double d = Math.Abs(A1 * points[j].x + B1 * points[j].y + C1 * points[j].z + D1)
                        / Math.Sqrt(A1 * A1 + B1 * B1 + C1 * C1);
                    if (d < 0.1) inter++;
                    else outer++;
                }
                if (inter > maxinter)
                {
                    A = A1; B = B1; C = C1; D = D1;
                    maxinter = inter;
                    idx = idx1;
                }
                if (i == 0 && points.Count>=1000)
                {
                    double d1000 = Math.Abs(A1 * points[999].x + B1 * points[999].y + C1 *points[999].z + D1)
                        / Math.Sqrt(A1 * A1 + B1 * B1 + C1 * C1);
                    double d5 = Math.Abs(A1 * points[4].x + B1 * points[4].y + C1 * points[4].z + D1)
                        / Math.Sqrt(A1 * A1 + B1 * B1 + C1 * C1);
                    report += string.Format("P1000 到拟合平面 S1 的距离:{0:f3}\n" +
                        "P5 到拟合平面 S1 的距离:{1:f3}\n" +
                        "拟合平面 S1 的内部点数量:{2:d}\n" +
                        "拟合平面 S1 的外部点数量:{3:d}\n", d1000, d5, inter, outer);
                }
                #endregion
            }
            return report;
        }
        private static double GetTriangleS(MyPoint p1,MyPoint p2,MyPoint p3)
        {
            double a = GetDistance(p1, p2);
            double b = GetDistance(p2, p3);
            double c = GetDistance(p3, p1);
            double p = (a + b + c) / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
        private static double GetDistance(MyPoint p1,MyPoint p2)
        {
            return Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z));
        }
    }
}
