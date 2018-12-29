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
    
        private HttpClient _client = new HttpClient();
        public CourseworkResultSheet(int _csid,string headerText)
        {
            InitializeComponent();
            MobileConfig.save_coursework_settings_id("" + _csid);
            txt_stream.SelectedIndex = 0;
            lbl_header.Text = headerText;


        }
        protected async Task RefreshResults()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    string stream = txt_stream.SelectedItem.ToString();
                    //await DisplayAlert("CSID ", "CSID="+CSID, "OK");
                    try
                    {
                        App_activity_indicator.IsVisible = true;
                        App_activity_indicator.IsRunning = true;
                        string webaddress = MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.
                            Format("DataFinder.aspx?dataFormat=courseworkresults&CSID={0}&stream={1}", MobileConfig.get_coursework_settings_id(), stream);
                        var content = await _client.GetStringAsync(webaddress);
                        //await DisplayAlert("Content", "CSID=" + MobileConfig.get_coursework_settings_id(), "OK");
                        if (content != "[]")
                        {
                            MobileConfig.save_coursework_results_sheet(content);
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
                    await DisplayAlert("Error! ", "No Internet Connection", "OK");
                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
                await DisplayAlert("Final Error! ", e.Message, "OK");
                DisplayResults();
            }
            DisplayResults();
        }

        protected async override void OnAppearing()
        {
            await RefreshResults();
            base.OnAppearing();
        }



        void DisplayResults()
        {
            try
            {
                
                var n = JsonConvert.DeserializeObject<List<Model.CourseworkViewResultsModel>>(MobileConfig.get_coursework_results_sheet());
                List<Model.CourseworkViewResultsModel> coursework_data = new List<Model.CourseworkViewResultsModel>(n);
                //DisplayAlert("Stored Data","No Records: "+ coursework_data.Count, "OK");
                lv_view_students.ItemsSource = coursework_data;
            }
            catch (Exception ex)
            {
                DisplayAlert("Stored Data", "Error! : " + ex.Message, "OK");
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
                var n = JsonConvert.DeserializeObject<List<Model.CourseworkViewResultsModel>>(MobileConfig.get_exam_results_sheet());
                List<Model.CourseworkViewResultsModel> coursework_data = new List<Model.CourseworkViewResultsModel>(n);
                string searchText = txtSearch.Text;
                List<Model.CourseworkViewResultsModel> filteredList = coursework_data;
                filteredList = coursework_data.Where(c => c.stud_name.Contains(searchText.ToUpper()) || c.stud_reg_no.Contains(searchText.ToUpper())).ToList();
                lv_view_students.ItemsSource = filteredList;

            }
            catch (Exception)
            {
                //lv_view_students.ItemsSource = null;
                DisplayAlert("Display Error ", "Error! Try again", "OK");
            }
        }

        private void lv_view_students_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Model.CourseworkViewResultsModel cs = (Model.CourseworkViewResultsModel)e.Item;
            Navigation.PushAsync(new CourseWorkUpdates(cs.cw1, cs.cw2, cs.cw3, cs.cw4, cs.cw5, string.Format("COURSEWORK FOR {0} \n[{1}]", cs.stud_name, cs.stud_reg_no),
                cs.cs_resultsID));
        }
    }
}