using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Algorithm
    {
        public static string CalculateAndReport(ref DataHelper data)
        {
            string report = "====REPORT====\n";
            double azim_AB = GetAzim(data.testpoints[0], data.testpoints[1]);
            azim_AB = azim_AB * 180.0 / Math.PI;
            int dd = (int)azim_AB;
            azim_AB = (azim_AB - dd) * 60;
            int mm = (int)azim_AB;
            azim_AB = (azim_AB - mm) * 60;
            double ss = azim_AB;
            report += string.Format("AB的方位角：{0}°{1}′{2:f4}″\n=============\n", dd, mm, ss);

            MyPoint K1 = data.keypoints[1];
            MyPoint[] closest = InterpolateH(K1, data);
            report += string.Format("K1内插高程：{0}\n距离K1最近的点:\n", K1.h);

            for(int i=0;i<5;i++)
            {
                report += string.Format("点名:{0}\t距离:{1:f3}\n", closest[i].name, MyPoint.GetDistance2D(K1, closest[i]));
            }
            report += "\n=============\n";

            report += string.Format("K0,K1为端点的梯形面积：{0:f3}\n=============\n", CalculateS(data.keypoints[0], data.keypoints[1], data.H0));

            report += CalculateVertical(ref data);
            report += "=============\n";

            report += CalculateHorizon(ref data);
            report += "=============\n";
            return report;
        }
        public static string CalculateHorizon(ref DataHelper data)//横断面
        {
            string report = "";
            double delta = 5;
            for(int i=0;i<data.keypoints.Count-1;i++)
            {
                List<MyPoint> Inps = new List<MyPoint>();
                double xm = (data.keypoints[i].x + data.keypoints[i + 1].x) / 2;
                double ym = (data.keypoints[i].y + data.keypoints[i + 1].y) / 2;
                report += string.Format("{0}和{1}之间的横断面内插点：\nx\ty\th\n",data.keypoints[i].name,data.keypoints[i+1].name);
                double Azim = GetAzim(data.keypoints[i], data.keypoints[i + 1]) + Math.PI / 2;
                double S = 0;
                for(int j=-5;j<=5;j++)
                {
                    MyPoint p = new MyPoint();
                    p.x = xm + delta * j * Math.Cos(Azim);
                    p.y = ym + delta * j * Math.Sin(Azim);
                    MyPoint[] ip=InterpolateH(p, data);
                    report += string.Format("{0:f3}\t{1:f3}\t{2:f3}\t最近的五个点号：", p.x, p.y, p.h);
                    for (int k = 0; k < 5; k++) report += string.Format("{0}\t", ip[k].name);
                    report += "\n";
                    Inps.Add(p);
                }
                for (int j = 0; j < Inps.Count - 1; j++)
                    S += CalculateS(Inps[j], Inps[j + 1], data.H0);
                report += string.Format("横断面面积：{0:f3}\n\n",S);
                data.InP2.Add(Inps);
            }
            return report;
        }
        public static string CalculateVertical(ref DataHelper data)//纵断面
        {
            data.InP1.Clear();
            string report = "";
            double D = 0;
            for(int i=0;i<data.keypoints.Count - 1;i++)
            {
                D += MyPoint.GetDistance2D(data.keypoints[i], data.keypoints[i + 1]);
            }
            report += string.Format("纵断面总长度:{0:f3}\n", D);
            int n = 0;
            double deltaL = 10;
            double L = 0, D0 = 0, Li = MyPoint.GetDistance2D(data.keypoints[n], data.keypoints[n + 1]);
            double Azim = GetAzim(data.keypoints[n], data.keypoints[n + 1]);
            //data.VerInP.Add(data.keypoints[n]);
            while (L<=D)
            {
                if (L>=Li+D0)
                {
                    n++;
                    D0 += Li;
                    Li = MyPoint.GetDistance2D(data.keypoints[n], data.keypoints[n + 1]);
                    Azim = GetAzim(data.keypoints[n], data.keypoints[n + 1]);
                    data.InP1.Add(data.keypoints[n]);
                    if (L == D0)
                    {
                        L += deltaL;
                        continue;
                    }
                }
                MyPoint inP = new MyPoint();
                inP.name = "InterpolatedPoint";
                inP.x = data.keypoints[n].x + (L - D0) * Math.Cos(Azim);
                inP.y = data.keypoints[n].y + (L - D0) * Math.Sin(Azim);
                InterpolateH(inP, data);
                data.InP1.Add(inP);
                L += deltaL;
            }
            data.InP1.Add(data.keypoints.Last());
            report += string.Format("内插点：\nx\ty\th\t\n");
            double S = 0;
            for (int i=0;i<data.InP1.Count;i++)
            {
                report += string.Format("{0:f3}\t{1:f3}\t{2:f3}\t\n", data.InP1[i].x, data.InP1[i].y, data.InP1[i].h);
            }
            for(int i=0;i<data.InP1.Count-1;i++)
            {
                S += CalculateS(data.InP1[i], data.InP1[i + 1], data.H0);
            }
            report += string.Format("纵断面面积：{0:f3}\n", S);
            return report;
        }
        public static double CalculateS(MyPoint P1,MyPoint P2,double H0)//计算梯形面积
        {
            return (P1.h + P2.h - 2 * H0) * MyPoint.GetDistance2D(P1, P2) / 2;
        }
        public static MyPoint[] InterpolateH(MyPoint P,in DataHelper data)
        {
            MyPoint[] SortP = data.points.ToArray();
            /*冒泡法排序，找出距离最小的5个点*/
            for(int i=0;i<5;i++)
            {
                for(int j=SortP.Length - 1; j>0; j--)
                {
                    if (MyPoint.GetDistance2D(SortP[j - 1], P) < MyPoint.GetDistance2D(SortP[j], P))
                        continue;
                    MyPoint temp = SortP[j - 1];
                    SortP[j - 1] = SortP[j];
                    SortP[j] = temp;
                }
            }
            double h = 0;
            double sum = 0;
            for(int i=0;i<5;i++)
            {
                double d = MyPoint.GetDistance2D(SortP[i], P);
                if (d == 0) 
                {
                    h = SortP[i].h;
                    break;
                }
                d = 1.0 / d;
                h = sum / (sum + d) * h + d / (sum + d) * SortP[i].h;
                sum += d;
            }
            P.h = h;
            return SortP;
        }
        public static double GetAzim(in MyPoint p1,in MyPoint p2)
        {
            double ans = Math.Atan2(p2.y - p1.y, p2.x - p1.x);
            if (ans < 0) ans += 2 * Math.PI;
            return ans;
        }
    }
}
