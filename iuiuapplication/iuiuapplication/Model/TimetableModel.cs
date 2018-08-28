using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class TimetableModel
    {
        public string SID { set; get; }
        public string course_id { set; get; }
        public string course_name { set; get; }
        public string roomNo { set; get; }
        public string lect_session { set; get; }
        public string stream { set; get; }
        public string prog_name { set; get; }
        public string duration { set; get; }
        public string lecture_day { set; get; }
        public string lect_name { set; get; }
        public string dayCode { set; get; }
        public string lecture_status { set; get; }
    }
}
