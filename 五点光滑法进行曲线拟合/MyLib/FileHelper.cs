using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace MyLib
{
    public class FileHelper
    {
        public List<MyPoint> points;
        public List<MyCurve> curves;
        public FileHelper()
        {
            points = new List<MyPoint>();
            curves = new List<MyCurve>();
        }
        public DataTable ReadFile(string filename)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID",typeof(string));
            table.Columns.Add("X", typeof(double));
            table.Columns.Add("Y", typeof(double));
            StreamReader reader = new StreamReader(filename);
            string line;
            string[] buf;
            while(!reader.EndOfStream)
            {
                line = reader.ReadLine();
                buf = line.Split(',');
                MyPoint p = new MyPoint();
                p.ID = buf[0];
                p.x = Convert.ToDouble(buf[1]);
                p.y = Convert.ToDouble(buf[2]);
                DataRow row = table.NewRow();
                row["ID"] = p.ID;
                row["X"] = p.x;
                row["Y"] = p.y;
                table.Rows.Add(row);
                points.Add(p);
            }
            reader.Close();
            return table;
        }
        public void SaveReport(string filename,bool is_closed)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.Write(Report.GetReport(this, is_closed));
            writer.Close();
        }
    }
}
