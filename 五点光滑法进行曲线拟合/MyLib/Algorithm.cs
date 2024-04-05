using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyLib
{
    public class Algorithm
    {
        public static void GetAdditionalPoints(ref FileHelper file, bool is_closed)
        {
            if(is_closed)//首尾闭合
            {
                file.points[0] = file.points[file.points.Count - 2];
                file.points[1] = file.points[file.points.Count - 1];
                file.points.Add(file.points[2]);
                file.points.Add(file.points[3]);
            }
            else//首尾不闭合
            {
                file.points[1].x = file.points[4].x - 3 * file.points[3].x + 3 * file.points[2].x;
                file.points[1].y = file.points[4].y - 3 * file.points[3].y + 3 * file.points[2].y;
                file.points[0].x = file.points[3].x - 3 * file.points[2].x + 3 * file.points[1].x;
                file.points[0].y = file.points[3].y - 3 * file.points[2].y + 3 * file.points[1].y;

                MyPoint pc = new MyPoint();
                MyPoint pd = new MyPoint();
                pc.x = 3 * file.points[file.points.Count - 1].x - 3 * file.points[file.points.Count - 2].x + file.points[file.points.Count - 3].x;
                pc.y = 3 * file.points[file.points.Count - 1].y - 3 * file.points[file.points.Count - 2].y + file.points[file.points.Count - 3].y;
                pd.x = 3 * pc.x - 3 * file.points[file.points.Count - 1].x + file.points[file.points.Count - 2].x;
                pd.y = 3 * pc.y - 3 * file.points[file.points.Count - 1].y + file.points[file.points.Count - 2].y;
                file.points.Add(pc);
                file.points.Add(pd);
            }
        }
        public static void GetGrads(ref FileHelper file)
        {
            for(int i=2;i<file.points.Count-3;i++)
            {
                GetGrad(file.points[i - 2], file.points[i - 1], file.points[i + 1], file.points[i + 2], file.points[i]);
            }
        }
        public static void CalCurveParam(ref FileHelper file, bool is_closed)
        {
            int end_idx = file.points.Count - 3;
            if (is_closed)
            {
                end_idx += 1;
            }
            for (int i = 2; i < end_idx; i++)
            {
                MyCurve curve = new MyCurve();
                double r = MyPoint.GetDistance(file.points[i], file.points[i + 1]);
                curve.E[0] = file.points[i].x;
                curve.E[1] = r * file.points[i].gx;
                curve.E[2] = 3 * (file.points[i + 1].x - file.points[i].x) - r * (file.points[i + 1].gx + 2 * file.points[i].gx);
                curve.E[3] = -2 * (file.points[i + 1].x - file.points[i].x) + r * (file.points[i + 1].gx + file.points[i].gx);

                curve.F[0] = file.points[i].y;
                curve.F[1] = r * file.points[i].gy;
                curve.F[2] = 3 * (file.points[i + 1].y - file.points[i].y) - r * (file.points[i + 1].gy + 2 * file.points[i].gy);
                curve.F[3] = -2 * (file.points[i + 1].y - file.points[i].y) + r * (file.points[i + 1].gy + file.points[i].gy);
                file.curves.Add(curve);
            }
        }
        public static Image DrawCurve(in FileHelper file,in int m,int width,int height,double zoom)
        {
            Bitmap bmp = new Bitmap(width, height);
            Image img = bmp;
            Graphics graphics = Graphics.FromImage(img);
            double maxx = file.points[2].x, maxy = file.points[2].y;
            double minx = file.points[2].x, miny = file.points[2].y;
            for(int i=2;i< file.points.Count - 2;i++)
            {
                maxx = maxx > file.points[i].x ? maxx : file.points[i].x;
                minx = minx < file.points[i].x ? minx : file.points[i].x;
                maxy = maxy > file.points[i].y ? maxy : file.points[i].y;
                miny = miny < file.points[i].x ? miny : file.points[i].y;
            }
            double centerx = (maxx + minx) / 2;
            double centery = (maxy + miny) / 2;
            double scale = Math.Max((maxy - miny)/width, (maxx - minx)/height);
            scale /= zoom;
            for(int i=0;i<file.curves.Count;i++)
            {
                PointF[] pointFs= new PointF[m];
                for(int j=0;j<m;j++)
                {
                    double z = 1.0 / m * j;
                    double x1 = 0, y1 = 0;
                    double t = 1;
                    for(int k=0;k<4;k++)
                    {
                        x1 += file.curves[i].E[k] * t;
                        y1 += file.curves[i].F[k] * t;
                        t *= z;
                    }
                    PointF p = new PointF((float)((x1 - centerx) / scale + 0.5 * width), (float)((y1 - centery) / scale + 0.5 * height));
                    pointFs[j] = p;
                }
                graphics.DrawLines(Pens.Black, pointFs);
            }
            for(int i=2;i<file.points.Count-2;i++)
            {
                graphics.FillEllipse(Brushes.Red, (float)((file.points[i].x - centerx) / scale + 0.5 * width), (float)((file.points[i].y - centery) / scale + 0.5 * height), 3, 3);
            }
            return img;
        }
        private static void GetGrad(in MyPoint p0, in MyPoint p1, in MyPoint p3, in MyPoint p4, MyPoint p2)
        {
            double a1 = p1.x - p0.x;double b1 = p1.y - p0.y;
            double a2 = p2.x - p1.x;double b2 = p2.y - p1.y;
            double a3 = p3.x - p2.x;double b3 = p3.y - p2.y;
            double a4 = p4.x - p3.x;double b4 = p4.y - p3.y;
            double w2 = Math.Abs(a3 * b4 - a4 * b3);
            double w3 = Math.Abs(a1 * b2 - a2 * b1);
            double a0 = w2 * a2 + w3 * a3;
            double b0 = w2 * b2 + w3 * b3;
            p2.gx = a0 / Math.Sqrt(a0 * a0 + b0 * b0);
            p2.gy = b0 / Math.Sqrt(a0 * a0 + b0 * b0);
        }
    }
}
