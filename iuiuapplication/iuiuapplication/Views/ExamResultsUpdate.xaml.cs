using iuiuapplication.Libraries;
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
    public partial class ExamResultsUpdate : ContentPage
    {
        private HttpClient _client = new HttpClient();
        int _EXID;
        public ExamResultsUpdate(double q1, double q2, double q3, double q4, double q5, double q6, double q7, double q8, double q9, double q10,int exid,string header)
        {
            InitializeComponent();
            txtAss1.Text = q1.ToString();
            txtAss2.Text = q2.ToString();
            txtAss3.Text = q3.ToString();
            txtAss4.Text = q4.ToString();
            txtAss5.Text = q5.ToString();
            txtAss6.Text = q6.ToString();
            txtAss7.Text = q7.ToString();
            txtAss8.Text = q8.ToString();
            txtAss9.Text = q9.ToString();
            txtAss10.Text = q10.ToString();
            _EXID = exid;
            lbl_header.Text = "EXAM MARKS FOR "+header;

        }

        private async void onSaveChangesTapped(object sender, EventArgs e)
        {
            await SaveExamUpdates();
        }

        private void CloseForm(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        public async Task SaveExamUpdates()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                App_activity_indicator.IsVisible = true;
                App_activity_indicator.IsRunning = true;

                var url = MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.
                            Format("DataFinder.aspx?dataFormat=UpdateExamMarks&q1={0}&q2={1}&q3={2}&q4={3}&q5={4}&q6={5}&q7={6}&q8={7}&q9={8}&q10={9}&typ={10}&exid={11}",
                            txtAss1.Text, txtAss2.Text, txtAss3.Text, txtAss4.Text, txtAss5.Text,txtAss6.Text,txtAss7.Text,txtAss8.Text,txtAss9.Text,txtAss10.Text,
                            Application.Current.Properties["examstat"], _EXID);
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