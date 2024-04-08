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
        public List<List<MyPoint>> ceils;//栅格数据
        public double[] average_z;
        public double[] Cmaxz, Cminz;
        public double[] sigmaz;
        public double maxx, minx;
        public double maxy, miny;
        public double maxz, minz;
        public string ReadFile(string filename, out DataTable table)
        {
            string report = "";
            points = new List<MyPoint>();
            table = new DataTable();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("x", typeof(string));
            table.Columns.Add("y", typeof(string));
            table.Columns.Add("z", typeof(string));
            StreamReader reader = new StreamReader(filename);
            string str;
            string[] buf;
            str = reader.ReadLine();
            int pnum = Convert.ToInt32(str);
            for(int i=0; i<pnum;i++)
            {
                str = reader.ReadLine();
                buf = str.Split(',');
                MyPoint p = new MyPoint();
                p.name = buf[0];
                p.x = Convert.ToDouble(buf[1]);
                p.y = Convert.ToDouble(buf[2]);
                p.z = Convert.ToDouble(buf[3]);
                points.Add(p);
                DataRow row = table.NewRow();
                row["name"] = p.name;
                row["x"] = string.Format("{0:f3}", p.x);
                row["y"] = string.Format("{0:f3}", p.y);
                row["z"] = string.Format("{0:f3}", p.z);
                table.Rows.Add(row);
            }
            reader.Close();
            maxx = minx = points[0].x;
            maxy = miny = points[0].y;
            maxz = minz = points[0].z;
            for (int i=0;i<pnum;i++)
            {
                maxx = maxx > points[i].x ? maxx : points[i].x;
                minx = minx < points[i].x ? minx : points[i].x;
                maxy = maxy > points[i].y ? maxy : points[i].y;
                miny = miny < points[i].y ? miny : points[i].y;
                maxz = maxz > points[i].z ? maxz : points[i].z;
                minz = minz < points[i].z ? minz : points[i].z;
            }

            report += string.Format("P5 的坐标分量 x:{0:f3}\n" +
                "P5 的坐标分量 y:{1:f3}\n" +
                "P5 的坐标分量 z:{2:f3}\n" +
                "坐标分量 x 的最小值:{3:f3}\n" +
                "坐标分量 x 的最大值:{4:f3}\n" +
                "坐标分量 y 的最小值:{5:f3}\n" +
                "坐标分量 y 的最大值:{6:f3}\n" +
                "坐标分量 z 的最小值:{7:f3}\n" +
                "坐标分量 z 的最大值:{8:f3}\n",
                points[4].x, points[4].y, points[4].z,
                minx, maxx, miny, maxy, minz, maxz);
            return report;
        }
        public void SaveResult(string filename)
        {
            string res = "";
            for(int i=0;i<points.Count;i++)
            {
                string Plane = "0";
                if (points[i].flag == 0) Plane = "0";
                else if (points[i].flag == 1) Plane = "J1";
                else if (points[i].flag == 2) Plane = "J2";
                res += string.Format("{0},{1:f3},{2:f3},{3:f3},{4}\n",
                    points[i].name, points[i].xt, points[i].yt, points[i].zt, Plane);
            }
            StreamWriter writer = new StreamWriter(filename);
            writer.Write(res);
            writer.Close();
        }
    }
}
