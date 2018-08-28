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
    public partial class ProgrammeProfile : ContentPage
    {
        public ProgrammeProfile(string progcode,string progname,string faculty,string requirements,string duration)
        {
            InitializeComponent();
            txt_profile_progcode.Text = progcode;
            txt_profile_progname.Text = progname;
            txt_profile_faculty.Text = faculty;
            txt_profile_requirements.Text = requirements;
            txt_profile_progduration.Text = duration + " Years";
            
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
