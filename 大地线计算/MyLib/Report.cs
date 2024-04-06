using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Report
    {
        public static string GetReport(in FileHelper file,in Param param)
        {
            string report = "";
            report += "==========REPORT==========\n";
            report+="参考椭球：" + (param.ep == Param.Tuoqiu.K ? "克拉索夫斯基椭球" :
                param.ep == Param.Tuoqiu.CGCS2000 ? "CGCS2000" : "Unknown");
            report += "\n";
            report += string.Format("点1：\n    B:{0:f4}dd.mmss    L:{1:f4}dd.mmss\n\n", file.B[0], file.L[0]);
            report += string.Format("点2：\n    B:{0:f4}dd.mmss    L:{1:f4}dd.mmss\n\n", file.B[1], file.L[1]);
            report += string.Format("大地线长度：{0:f6}m\n", file.S);
            return report;
        }
    }
}
