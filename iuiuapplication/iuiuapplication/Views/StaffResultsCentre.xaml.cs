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
        public StaffResultsCentre(string cs_code, string course_nm, string sem, string yr,string acad,string prog,string sess, string prog_id)
        {
            InitializeComponent();
            string campus = Application.Current.Properties["campus"].ToString();
            this.Children.Add(new StaffCourseworkResults(Application.Current.Properties["userno"].ToString(), cs_code, course_nm, sem, yr,acad,prog,sess,campus));
            this.Children.Add(new StaffExamResults(cs_code, course_nm, sem, yr, acad, prog_id, sess));
        }
    }
}
