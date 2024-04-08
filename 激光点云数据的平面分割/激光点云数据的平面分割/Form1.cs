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

namespace 激光点云数据的平面分割
{
    public partial class Form1 : Form
    {
        DataHelper data;
        Report report = new Report();
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt|*.txt";
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                data = new DataHelper();
                report = new Report();
                DataTable table = new DataTable();
                report.report[0] = data.ReadFile(openFileDialog1.FileName, out table);
                dataGridView1.DataSource = table;
                richTextBox1.Text = report.CreateReport();
            }
        }

        private void ceilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            report.report[1] = Algorithm.CeilDividePoints(ref data);
            richTextBox1.Text = report.CreateReport();
            MessageBox.Show("栅格投影点云分割成功！");
        }

        private void rANSACToolStripMenuItem_Click(object sender, EventArgs e)
        {
            report.report[2] = Algorithm.RANSAC(ref data);
            richTextBox1.Text = report.CreateReport();
            MessageBox.Show("RANSAC分割成功！");
        }

        private void saveReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt|*.txt";
            if(saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                data.SaveResult(saveFileDialog1.FileName);
                MessageBox.Show("Succeeded!");
            }
        }
    }
}
