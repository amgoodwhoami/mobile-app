using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class RegistrationModel
    {
        //stat_name, Clearance, retakes, stud_reg_no, Sem, AcademicYr, RegRemarks, reg_status, cyear, clear_stat, Teller, DateClear, regid, CardPrintStatus, IDCardStatus, WardenRegStatus
        public string stat_name { get; set; }
        public string retakes { get; set; }
        public string stud_reg_no { get; set; }
        public string Sem { get; set; }
        public string AcademicYr { get; set; }
        public string cyear { get; set; }
        public string Clearance { get; set; }
        public string IDCardStatus { get; set; }
        public string WardenRegStatus { get; set; }
        public string studySys { get; set; }
    }
}
