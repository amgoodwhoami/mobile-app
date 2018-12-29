using iuiuapplication.Libraries;
using Newtonsoft.Json;
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
    public partial class CourseWorkUpdates : ContentPage
    {
        private HttpClient _client = new HttpClient();
        long CWID;
        public CourseWorkUpdates(double cw1, double cw2, double cw3, double cw4, double cw5,string header,long cwid)
        {
            InitializeComponent();
            txtAss1.Text = cw1.ToString();
            txtAss2.Text = cw2.ToString();
            txtAss3.Text = cw3.ToString();
            txtAss4.Text = cw4.ToString();
            txtAss5.Text = cw5.ToString();
            lbl_header.Text = header;
            CWID = cwid;

        
        }

        private async void SaveChanges(object sender, EventArgs e)
        {
            await SaveCourseworkUpdates();
        }
        private void CloseForm(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        public async Task SaveCourseworkUpdates()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                App_activity_indicator.IsVisible = true;
                App_activity_indicator.IsRunning = true;

                var url = MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.
                            Format("DataFinder.aspx?dataFormat=UpdateCourseWork&cw1={0}&cw2={1}&cw3={2}&cw4={3}&cw5={4}&compFormat={5}&cwid={6}", 
                            txtAss1.Text,txtAss2.Text,txtAss3.Text,txtAss4.Text,txtAss5.Text,MobileConfig.get_coursework_compForm(),CWID);
                var response = await _client.PostAsync(url, null);
                var responseContent = await response.Content.ReadAsStringAsync();
                // set the server reply a message to the Display Alert
                await DisplayAlert("IUIU Mobile", "" + responseContent, "Ok");
                App_activity_indicator.IsVisible = false;
                App_activity_indicator.IsRunning = false;
            }
            else
            {
                App_activity_indicator.IsVisible = false;
                App_activity_indicator.IsRunning = false;
                await DisplayAlert("No internet Connection", "Sorry, please first connect to the internet.", "Ok");

            }
        }
    }
}