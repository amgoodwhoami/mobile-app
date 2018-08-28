using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class TeachingClaimModel
    {
        public int ID { get; set; }
        public string emp_Code { get; set; }
        public string formated_date { get; set; }
        public double total_amount { get; set; }
        public string acad_year { get; set; }
        public int semester { get; set; }
        public string course_id { get; set; }
        public string course_class { get; set; }
        public string course_name { get; set; }
        public int total_hrs { get; set; }

    }
}
