using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace MyLib
{
    public class DataHelper
    {
        public List<MyPoint> points;
        public List<MyPoint> keypoints;
        public List<MyPoint> testpoints;
        public List<MyPoint> InP1;//纵断面插值点
        public List<List<MyPoint>> InP2;//横断面差值点
        public double H0;
        public DataHelper()
        {
            points = new List<MyPoint>();
            keypoints = new List<MyPoint>();
            testpoints = new List<MyPoint>();
            InP1 = new List<MyPoint>();
            InP2 = new List<List<MyPoint>>();
        }
        public DataTable ReadFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string line;
            string[] buf;
            line = reader.ReadLine();
            buf = line.Split(',');
            H0 = Convert.ToDouble(buf[1]);
            line = reader.ReadLine();
            buf = line.Split(',');
            for(int i=0;i<buf.Length;i++)
            {
                MyPoint keyp = new MyPoint();
                keyp.name = buf[i];
                keypoints.Add(keyp);
            }
            for(int i=0;i<2;i++)
            {
                MyPoint testp = new MyPoint();
                line = reader.ReadLine();
                buf = line.Split(',');
                testp.name = buf[0];
                testp.x = Convert.ToDouble(buf[1]);
                testp.y = Convert.ToDouble(buf[2]);
                testpoints.Add(testp);
            }
            reader.ReadLine();
            while(!reader.EndOfStream)
            {
                MyPoint p = new MyPoint();
                line = reader.ReadLine();
                buf = line.Split(',');
                p.name = buf[0];
                p.x = Convert.ToDouble(buf[1]);
                p.y = Convert.ToDouble(buf[2]);
                p.h = Convert.ToDouble(buf[3]);
                points.Add(p);
                for (int i = 0; i < keypoints.Count; i++)
                    keypoints[i] = keypoints[i].name == p.name ? p : keypoints[i];
            }
            reader.Close();
            return MakeTable();
        }
        private DataTable MakeTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("x", typeof(double));
            table.Columns.Add("y", typeof(double));
            table.Columns.Add("h", typeof(double));

            for(int i=0;i<points.Count;i++)
            {
                DataRow row = table.NewRow();
                row["name"] = points[i].name;
                row["x"] = points[i].x;
                row["y"] = points[i].y;
                row["h"] = points[i].h;
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
