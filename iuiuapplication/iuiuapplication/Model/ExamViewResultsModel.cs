using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class ExamViewResultsModel
    {
        public string stud_name { get; set; }
        public int examResultsID { get; set; }
        public int examSettingsID { get; set; }
        public string stud_reg_no { get; set; }
        public int Q1Mark { get; set; }
        public int Q2Mark { get; set; }
        public int Q3Mark { get; set; }
        public int Q4Mark { get; set; }
        public int Q5Mark { get; set; }
        public int Q6Mark { get; set; }
        public int Q7Mark { get; set; }
        public int Q8Mark { get; set; }
        public int totalMark { get; set; }
        public double examMark { get; set; }
        public int CWMark { get; set; }
        public int grandTotal { get; set; }
        public string studStatus { get; set; }
        public string stud_stream { get; set; }
        public int Q9Mark { get; set; }
        public int Q10Mark { get; set; }
        public string ExamStatus { get; set; }
        public int ApprovalStatus { get; set; }
        public string ExamComments { get; set; }


    }
}
