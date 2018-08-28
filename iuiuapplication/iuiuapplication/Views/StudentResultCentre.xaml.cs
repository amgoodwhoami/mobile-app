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
    public partial class StudentResultCentre : TabbedPage
    {
        public StudentResultCentre(string campus)
        {
            InitializeComponent();
            this.Children.Add(new StudentResults(campus));
            this.Children.Add(new CourseWorkResults(campus));
        }
    }
}
