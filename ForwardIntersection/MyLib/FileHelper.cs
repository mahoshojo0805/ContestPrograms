using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyLib
{
    public class FileHelper
    {
        public MyPicture[] pic;
        public double N1, N2;//投影系数
        public double X, Y, Z;//地面坐标
        public void ReadFile(string filename)
        {
            pic = new MyPicture[2];
            StreamReader reader = new StreamReader(filename);
            for(int i=0;i<2;i++)
            {
                pic[i] = new MyPicture();
                string str = "";
                str = reader.ReadLine();
                pic[i].Xs = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].Ys = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].Zs = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].phi = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].omega = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].k = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].x = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].y = Convert.ToDouble(str);
                str = reader.ReadLine();
                pic[i].f = Convert.ToDouble(str);
            }
            reader.Close();
        }
        public void SavReport(string filename)
        {
            string report = "";
            report += Report.PrintReport1(this);
            report += Report.PrintReport2(this);
            report += Report.PrintfReport3(this);
            StreamWriter writer = new StreamWriter(filename);
            writer.Write(report);
            writer.Close();
        }
    }
}
