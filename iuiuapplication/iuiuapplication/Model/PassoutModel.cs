using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class PassoutModel
    {
        public int _id { get; set; }
        public string stud_reg_no { get; set; }
        public DateTime passoutDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string PassoutStatus { get; set; }
        public DateTime ExitDate { get; set; }
        public string PassoutComments { get; set; }
        public string passoutType { get; set; }
        public string reason { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public string stud_name { get; set; }
        public string acadyear { get; set; }
        public int semester { get; set; }
        public string ExitPeriod { get; set; }
    }
}
