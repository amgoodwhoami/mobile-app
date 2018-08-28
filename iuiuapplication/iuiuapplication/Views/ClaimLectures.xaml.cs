using iuiuapplication.DB;
using iuiuapplication.Model;
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
    public partial class ClaimLectures : ContentPage
    {
        private HttpClient _client = new HttpClient();
        int claimID;string course_name, acad_year, semester, study_sys;
        public ClaimLectures(int CID,string cs_name, string acad, string sem, string studysys)
        {
            InitializeComponent();
            claimID = CID;
            course_name = cs_name;
            acad_year = acad;
            semester = sem;
            study_sys = studysys;
            lbl_lectures_header.Text = string.Format("LECTURES FOR CLAIM NO: {0} \n[{1}]",CID,course_name);
        }
        protected async Task RefreshClaimLectures()
        {
            MyDB DB = new MyDB();
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=myclaimslectures&CID={0}", claimID);
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var content = await _client.GetStringAsync(webaddress);

                    var n = JsonConvert.DeserializeObject<List<Model.LectureModel>>(content);
                    List<Model.LectureModel> L_data = new List<Model.LectureModel>(n);
                   // await DisplayAlert("Content ", "Items " + L_data.Count, "OK");
                    if (L_data.Count > 0)
                    {
                       
                        DB.resetLectures();
                        DB.AddLectures(content);
                    }
                    else
                    {
                        //await DisplayAlert("Content ", "Sem " + content, "OK");
                        DB.resetLectures();
                        lv_claimlectures.ItemsSource = null;
                    }

                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
                catch (Exception)
                {

                    //await DisplayAlert("Error!", "" + ex.Message, "OK");
                    DB.resetLectures();
                    lv_claimlectures.ItemsSource = null;
                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }
            // await DisplayAlert("Content ", "Content " + DB.GetAllLectures(), "OK");
            var list = JsonConvert.DeserializeObject<List<Model.LectureModel>>(DB.GetAllLectures());
            List<Model.LectureModel> display_data = new List<Model.LectureModel>(list);
            lv_claimlectures.ItemsSource = display_data;
            lv_claimlectures.EndRefresh();
        }
        async protected override void OnAppearing()
        {
            try
            {
                await RefreshClaimLectures();
            }
            catch (Exception ex)
            { await DisplayAlert("Error!", ""+ ex.Message, "OK"); }
            base.OnAppearing();
        }

        private async void lv_claimlectures_Refreshing(object sender, EventArgs e)
        {
            try
            {
                await RefreshClaimLectures();
            }
            catch (Exception ex)
            { await DisplayAlert("Error!", "" + ex.Message, "OK"); }

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewLecture(acad_year,semester,study_sys, claimID.ToString()));
        }
    }
    
}
