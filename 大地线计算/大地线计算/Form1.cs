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

namespace 大地线计算
{
    public partial class Form1 : Form
    {
        FileHelper file = new FileHelper();
        Param param = new Param();
        bool is_cal = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt|*.txt";
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                //file = new FileHelper();
                file.ReadFile(openFileDialog1.FileName);
                textBox1.Text = file.B[0].ToString();
                textBox2.Text = file.B[1].ToString();
                textBox3.Text = file.L[0].ToString();
                textBox4.Text = file.L[1].ToString();
            }
        }

        private void calcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.B[0] = Convert.ToDouble(textBox1.Text);
            file.B[1] = Convert.ToDouble(textBox2.Text);
            file.L[0] = Convert.ToDouble(textBox3.Text);
            file.L[1] = Convert.ToDouble(textBox4.Text);
            double S = Algorithm.GetGeodeticLine(file, param);
            is_cal = true;
            richTextBox1.Text = Report.GetReport(file, param);
        }

        private void 克拉索夫斯基椭球ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            param.SetParam(Param.Tuoqiu.K);
        }

        private void cGCS2000ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            param.SetParam(Param.Tuoqiu.CGCS2000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            file.B[0] = Convert.ToDouble(textBox1.Text);
            file.B[1] = Convert.ToDouble(textBox2.Text);
            file.L[0] = Convert.ToDouble(textBox3.Text);
            file.L[1] = Convert.ToDouble(textBox4.Text);
            double S = Algorithm.GetGeodeticLine(file, param);
            is_cal = true;
            richTextBox1.Text = Report.GetReport(file, param);
        }

        private void saveReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt|*.txt";
            if (is_cal)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    file.SaveReport(saveFileDialog1.FileName, param);
                }
            }
            else MessageBox.Show("请先计算!");
        }
    }
}
