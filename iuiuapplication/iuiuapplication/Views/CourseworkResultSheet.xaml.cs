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
    public partial class CourseworkResultSheet : ContentPage
    {
        int CSID;
        private HttpClient _client = new HttpClient();
        public CourseworkResultSheet(int csid)
        {
            InitializeComponent();
            CSID = csid;
        }
        protected async Task RefreshResults()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    string stream = txt_stream.SelectedItem.ToString();
                    //await DisplayAlert("EXID ", exid, "OK");
                    try
                    {
                        App_activity_indicator.IsVisible = true;
                        App_activity_indicator.IsRunning = true;
                        string webaddress = MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.
                            Format("DataFinder.aspx?dataFormat=courseworkresults&CSID={0}", CSID);
                        var content = await _client.GetStringAsync(webaddress);

                        // Debug.WriteLine("Exam Results DATA -> ", content);
                        //await DisplayAlert("Content! ", content, "OK");

                        if (content != "[]")
                        {
                            // save the data locally.

                            MobileConfig.save_exam_results_sheet(content);
                            App_activity_indicator.IsRunning = false;
                            DisplayResults();

                        }
                        else
                        {

                            await DisplayAlert("Content Error! ", "No Results Found", "OK");
                        }
                        App_activity_indicator.IsVisible = false;
                        App_activity_indicator.IsRunning = false;
                        DisplayResults();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("General Error! ", ex.Message, "OK");
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
                await DisplayAlert("Final Error! ", e.Message, "OK");
                DisplayResults();
            }
        }

        protected async override void OnAppearing()
        {
            await RefreshResults();
            DisplayResults();
            base.OnAppearing();
        }



        void DisplayResults()
        {
            try
            {
                //DisplayAlert("Stored Data", MobileConfig.get_exam_results_sheet(), "OK");
                var n = JsonConvert.DeserializeObject<List<Model.ExamViewResultsModel>>(MobileConfig.get_exam_results_sheet());
                List<Model.ExamViewResultsModel> exams_data = new List<Model.ExamViewResultsModel>(n);
                //DisplayAlert("Stored Data", ""+n.Count, "OK");
                lv_view_students.ItemsSource = exams_data;
            }
            catch (Exception)
            {

            }
        }

        private async void lv_view_students_Refreshing(object sender, EventArgs e)
        {
            await RefreshResults();

            App_activity_indicator.IsVisible = false;
            App_activity_indicator.IsRunning = false;
        }

        private async void onChangeStatus(object sender, EventArgs e)
        {
            await RefreshResults();
        }

        private async void onChangeStream(object sender, EventArgs e)
        {
            await RefreshResults();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        void Search()
        {
            try
            {
                var n = JsonConvert.DeserializeObject<List<Model.ExamViewResultsModel>>(MobileConfig.get_exam_results_sheet());
                List<Model.ExamViewResultsModel> exams_data = new List<Model.ExamViewResultsModel>(n);
                string searchText = txtSearch.Text;
                List<Model.ExamViewResultsModel> filteredList = exams_data;
                filteredList = exams_data.Where(c => c.stud_name.Contains(searchText.ToUpper()) || c.stud_reg_no.Contains(searchText.ToUpper())).ToList();
                lv_view_students.ItemsSource = filteredList;

            }
            catch (Exception)
            {
                //lv_view_students.ItemsSource = null;
                DisplayAlert("Display Error ", "Error! Try again", "OK");
            }
        }
    }
}