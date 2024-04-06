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
        public double[] B;
        public double[] L;
        public double S = 0;
        public FileHelper()
        {
            B = new double[2];
            L = new double[2];
        }
        public void ReadFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string line;
            string[] buf;

            for(int i=0;i<2;i++)
            {
                line = reader.ReadLine();
                buf = line.Split(' ');
                B[i] = Convert.ToDouble(buf[0]);
                L[i] = Convert.ToDouble(buf[1]);
            }
            reader.Close();
        }
        public void SaveReport(string filename, in Param param)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.Write(Report.GetReport(this, param));
            writer.Close();
        }
        public static double DDMMSSSSToRad(double d)
        {
            double temp = d;
            double dd = (int)temp;
            temp = (temp - dd) * 100;
            double mm = (int)temp;
            temp = (temp - mm) * 100;
            double ssss = temp;
            return (dd + mm / 60 + ssss / 3600) * Math.PI / 180;
        }
    }
}
