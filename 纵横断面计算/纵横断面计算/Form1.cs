using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MyLib;

namespace 纵横断面计算
{
    public partial class Form1 : Form
    {
        DataHelper data;
        string report = "";
        bool is_data_read = false;
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
                data = new DataHelper();
                dataGridView1.DataSource = data.ReadFile(openFileDialog1.FileName);
                is_data_read = true;
            }
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (is_data_read)
            {
                report = Algorithm.CalculateAndReport(ref data);
                richTextBox1.Text = report;
                is_cal = true;
                tabControl1.SelectedTab = tabPage2;
            }
            else MessageBox.Show("请先读取文件!");
        }

        private void saveReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(is_cal)
            {
                saveFileDialog1.Filter = "txt|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                    writer.Write(report);
                    writer.Close();
                }
                else MessageBox.Show("请先计算！");
            }
        }
    }
}
