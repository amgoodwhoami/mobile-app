using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace iuiuapplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffResultsCentre : TabbedPage
    {
        public StaffResultsCentre(int ID, string cs_code, string course_nm, string sem, string yr,string acad,string prog,string sess, string prog_id)
        {
            InitializeComponent();
            string campus = Application.Current.Properties["campus"].ToString();
            this.Children.Add(new StaffCourseworkResults(ID, cs_code, course_nm, sem, yr,acad,prog,sess));
            this.Children.Add(new StaffExamResults(ID, cs_code, course_nm, sem, yr, acad, prog_id, sess));
        }
    }
}
