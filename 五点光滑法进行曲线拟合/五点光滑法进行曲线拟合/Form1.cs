using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLib;

namespace 五点光滑法进行曲线拟合
{
    public partial class Form1 : Form
    {
        FileHelper file;
        double zoom = 1;
        bool is_file_read = false;
        bool is_closed = false;
        bool is_cal = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt文本文件|*.txt";
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                file = new FileHelper();
                MyPoint additionalp1 = new MyPoint();
                MyPoint additionalp2 = new MyPoint();
                //两个补充点放在链表头部，便于之后修改
                file.points.Add(additionalp1);
                file.points.Add(additionalp2);
                dataGridView1.DataSource = file.ReadFile(openFileDialog1.FileName);
                //放在尾部
                file.points.Add(additionalp1);
                file.points.Add(additionalp2);
                is_file_read = true;
            }
        }
        private void calculateToolStripMenuItem_Click()
        {
            if (is_file_read)
            {
                file.curves.Clear();
                file.points.RemoveAt(file.points.Count - 1);
                file.points.RemoveAt(file.points.Count - 1);
                Algorithm.GetAdditionalPoints(ref file, is_closed);
                Algorithm.GetGrads(ref file);
                Algorithm.CalCurveParam(ref file, is_closed);
                int m = Convert.ToInt32(textBox1.Text);
                pictureBox1.Image = Algorithm.DrawCurve(file, m, pictureBox1.Width, pictureBox1.Height, zoom);
                tabControl1.SelectedTab = tabPage2;
                richTextBox1.Text = Report.GetReport(file, is_closed);
                is_cal = true;
            }
            else
                MessageBox.Show("请先打开文件");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int m = Convert.ToInt32(textBox1.Text);
            zoom *= 2;
            pictureBox1.Image = Algorithm.DrawCurve(file, m, pictureBox1.Width, pictureBox1.Height, zoom);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int m = Convert.ToInt32(textBox1.Text);
            zoom /= 2;
            pictureBox1.Image = Algorithm.DrawCurve(file, m, pictureBox1.Width, pictureBox1.Height, zoom);
        }

        private void 闭合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            is_closed = true;
            calculateToolStripMenuItem_Click();
        }

        private void 非闭合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            is_closed = false;
            calculateToolStripMenuItem_Click();
        }

        private void saveReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(is_cal)
            {
                saveFileDialog1.Filter = "txt文本文件|*.txt";
                if(saveFileDialog1.ShowDialog()==DialogResult.OK)
                {
                    file.SaveReport(saveFileDialog1.FileName, is_closed);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
