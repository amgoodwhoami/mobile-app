using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class AnnouncementModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string author { get; set; }
        public string authorid { get; set; }
        public DateTime dat { get; set; }
        public int archive_stat { get; set; }
        public string TargetCategory { get; set; }
    }
}
