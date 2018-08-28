using iuiuapplication.Libraries;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iuiuapplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExamViewSheet : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public ExamViewSheet()
        {
            InitializeComponent();
           
        }


        protected async Task RefreshResults()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    string exid = MobileConfig.get_exam_settings_id();
                    string examstat = txt_status.Items[txt_status.SelectedIndex];
                    string stream = txt_stream.Items[txt_stream.SelectedIndex];

                    try
                    {
                        App_activity_indicator.IsVisible = true;
                        App_activity_indicator.IsRunning = true;
                        string webaddress = MobileConfig.KampalaCampuslink + string.
                            Format("DataFinder.aspx?dataFormat=examresults&EXID={0}&examStat={1}&stream={2}",
                           exid, examstat, stream);
                        var content = await _client.GetStringAsync(webaddress);

                        Debug.WriteLine("Exam Results DATA -> ", content);
                        await DisplayAlert("Conetnt! ", content, "OK");

                        if (content != "[]")
                        {
                            // save the data locally.
                           
                            MobileConfig.save_exam_results_sheet(content);
                        }
                        else
                        {

                            await DisplayAlert("Error! ", "No Results Found a :( ", "OK");
                        }
                        App_activity_indicator.IsVisible = false;
                        App_activity_indicator.IsRunning = false;
                    }
                    catch (Exception)
                    {
                        App_activity_indicator.IsVisible = false;
                        App_activity_indicator.IsRunning = false;
                    }

                }
                else
                {

                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
                Debug.WriteLine("am out of range : ",e);
                DisplayResults();
            }
        }

        async protected override void OnAppearing()
        {
            await RefreshResults();
            DisplayResults();
            base.OnAppearing();
        }



        void DisplayResults()
        {
            try
            {
                var n = JsonConvert.DeserializeObject<List<Model.ExamViewResultsModel>>(MobileConfig.get_exam_results_sheet());
                List<Model.ExamViewResultsModel> exams_data = new List<Model.ExamViewResultsModel>(n);
                lv_view_students.ItemsSource = exams_data;
            }
            catch (Exception)
            {
               
            }
        }

        private async void lv_view_students_Refreshing(object sender, EventArgs e)
        {
            await RefreshResults();
        }

        private async void onChangeStatus(object sender, EventArgs e)
        {
            await RefreshResults();
        }

        private async void onChangeStream(object sender, EventArgs e)
        {
            await RefreshResults();
        }
    }
}
