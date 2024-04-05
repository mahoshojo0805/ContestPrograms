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

namespace ForwardIntersection
{
    public partial class Form1 : Form
    {
        FileHelper file;
        bool is_file_opened = false;
        bool is_calculated = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file = new FileHelper();
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                file.ReadFile(openFileDialog1.FileName);
                richTextBox1.AppendText(Report.PrintReport1(in file));
                is_file_opened = true;
            }
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (is_file_opened)
            {
                Algorithm.GetAuxiliaryCoor(ref file);
                richTextBox1.AppendText(Report.PrintReport2(in file));
                Algorithm.GetCoef(ref file);
                Algorithm.GetXYZ(ref file);
                richTextBox1.AppendText(Report.PrintfReport3(in file));
                is_calculated = true;
            }
            else
            {
                richTextBox1.AppendText("Please Open File First!\n");
            }
        }

        private void saveResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(is_calculated)
            {
                saveFileDialog1.Filter = "*.txt|文本文件";
                if(saveFileDialog1.ShowDialog()==DialogResult.OK)
                {
                    file.SavReport(saveFileDialog1.FileName);
                }
            }
            else
            {
                richTextBox1.AppendText("Please Calculate First!\n");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
