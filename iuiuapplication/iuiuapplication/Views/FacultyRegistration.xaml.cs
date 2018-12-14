using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iuiuapplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacultyRegistration : ContentPage
    {
        private HttpClient _client = new HttpClient();
        string acad="-", sem="0";
        public FacultyRegistration(string acadyr,string sems)
        {
            InitializeComponent();
            acad = acadyr;
            if(sems.Contains("Semester"))
            {
                sem = sems.Substring(9,1);
            }
            else if(sems.Contains("Term"))
            {
                sem = sems.Substring(5, 1);
            }
            else
            {
                sem = sems.Substring(8, 1);
            }
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
           string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                string.Format("DataFinder.aspx?dataFormat=self_reg&reg={0}&acad={1}&sem={2}", Application.Current.Properties["userno"], acad, sem);
            //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + webaddress, "OK");
            var content = await _client.GetStringAsync(webaddress);
            await DisplayAlert("IUIU Mobile", content, "OK");
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CrossConnectivity.Current.IsConnected)
            {
                string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                string.Format("DataFinder.aspx?dataFormat=check_reg&reg={0}&acad={1}&sem={2}", Application.Current.Properties["userno"], acad, sem);
                //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + webaddress, "OK");
                var content = await _client.GetStringAsync(webaddress);
                lbl_regcomment.Text = content;
                if (content.Contains("DENIED"))
                {
                    cmdRegister.BackgroundColor = Color.Red;
                    lbl_regcomment.TextColor = Color.Red;
                    cmdRegister.Text = "REGISTRATION DENIED!";
                    // cmdRegister.IsEnabled = false;
                }
            }
            else
            {
                await DisplayAlert("IUIU Mobile", "Connection Error! Check and try again", "OK");
                await Navigation.PopModalAsync();
            }
                
            
        }
    }
}