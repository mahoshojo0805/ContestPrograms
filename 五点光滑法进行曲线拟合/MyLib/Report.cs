using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Report
    {
        public static string GetReport(in FileHelper file,bool is_closed)
        {
            string report = "==== REPORT ====\n";
            report += "点数:" + (file.points.Count - 4).ToString() + "\n";
            report += "是否闭合:" + (is_closed ? "是\n" : "否\n");
            report += "======================\n";
            report += string.Format("起点ID\t起点x\t起点y\t终点ID\t终点x\t终点y\tE0\tE1\tE2\tE3\tF0\tF1\tF2\tF3\n");
            for(int i=0;i<file.curves.Count;i++)
            {
                report += string.Format("{0}\t{1:f3}\t{2:f3}\t{3:f3}\t{4:f3}\t{5:f3}\t", file.points[i + 2].ID, file.points[i + 2].x, file.points[i + 2].y,
                    file.points[i + 3].ID, file.points[i + 3].x, file.points[i + 2].y);
                for (int j = 0; j < 4; j++)
                    report += string.Format("{0:f3}\t", file.curves[i].E[j]);
                for (int j = 0; j < 4; j++)
                    report += string.Format("{0:f3}\t", file.curves[i].F[j]);
                report += "\n";
            }
            return report;
        }
    }
}
