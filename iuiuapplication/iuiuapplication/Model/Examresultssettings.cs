using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class Examresultssettings
    {
        public int examSettingsID { get; set; }
        public string course_id { get; set; }
        public string progid { get; set; }
        public string acadyear { get; set; }
        public int semester { get; set; }
        public string lecturerID { get; set; }
        public int no_qns { get; set; }
        public int TotalMaxMark { get; set; }
        public int maxMark1 { get; set; }
        public int maxMark2 { get; set; }
        public int maxMark3 { get; set; }
        public int maxMark4 { get; set; }
        public int maxMark5 { get; set; }
        public int maxMark6 { get; set; }
        public int maxMark7 { get; set; }
        public int maxMark8 { get; set; }
        public int examRatio { get; set; }
        public int cwRatio { get; set; }
        public string studSession { get; set; }
        public string studIntake { get; set; }
        public int cyear { get; set; }
        public string progname { get; set; }
        public object ExamDate { get; set; }
        public object ExamDuration { get; set; }
        public object courseName { get; set; }
        public int lockStatus { get; set; }
        public int maxMark9 { get; set; }
        public int maxMark10 { get; set; }
        public string examFormat { get; set; }
    }
}
