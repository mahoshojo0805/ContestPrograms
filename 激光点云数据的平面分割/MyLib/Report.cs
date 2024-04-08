using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Report
    {
        public string[] report;
        public Report()
        {
            report = new string[9];
            for (int i = 0; i < 9; i++)
                report[i] = "";
        }
        public string CreateReport()
        {
            string res = "";
            for (int i = 0; i < 9; i++)
                res += report[i];
            return res;
        }
    }
}
