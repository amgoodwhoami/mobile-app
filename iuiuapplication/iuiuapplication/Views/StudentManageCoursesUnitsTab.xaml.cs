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
    public partial class StudentManageCoursesUnitsTab : TabbedPage
    {
        public StudentManageCoursesUnitsTab()
        {
            InitializeComponent();
            this.Children.Add(new RegisterCourseUnits());
            this.Children.Add(new RegisterRetakes());
        }
    }


}
