using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Report
    {
        public static string PrintReport1(in FileHelper file)
        {
            string report = "Input Data:\n";
            for(int i=0;i<2;i++)
            {
                report += ("Picture " + i.ToString() + " :\n");
                report += (" 像片主距：" + file.pic[i].f.ToString() + "\n");
                report += (" 像点坐标(x,y)：(" + file.pic[i].x.ToString() + "," + file.pic[i].y.ToString() + ")\n");
                report += (" 模型基线分量(Xs,Ys,Zs):(" + file.pic[i].Xs.ToString() + "," + file.pic[i].Ys.ToString() + "," + file.pic[i].Zs.ToString() + ")\n");
                report += (" 偏角:" + file.pic[i].phi.ToString() + "\n");
                report += (" 倾角:" + file.pic[i].omega.ToString() + "\n");
                report += (" 旋角:" + file.pic[i].k.ToString() + "\n");
            }
            report += "\n";
            return report;
        }
        public static string PrintReport2(in FileHelper file)
        {
            string report = "Auxiliary Coordinates:\n";
            for (int i = 0; i < 2; i++)
            {
                report += ("Picture " + i.ToString() + " :\n");
                report += (" 空间辅助坐标(u,v,w)：(" + file.pic[i].u.ToString() + "," + file.pic[i].v.ToString() + "," + file.pic[i].w.ToString() + ")\n");
            }
            report += "\n";
            return report;
        }
        public static string PrintfReport3(in FileHelper file)
        {
            string report = "投影系数:\n";
            report += " N1=" + file.N1.ToString() + "    N2=" + file.N2.ToString() + "\n\n";
            report += "地面坐标(X,Y,Z):(" + file.X.ToString() + "," + file.Y.ToString() + "," + file.Z.ToString() + ")\n";
            return report;
        }
    }
}
