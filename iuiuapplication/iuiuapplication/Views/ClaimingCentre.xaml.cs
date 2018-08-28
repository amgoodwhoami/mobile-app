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
    public partial class ClaimingCentre : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public ClaimingCentre()
        {
            InitializeComponent();
            try
            {
                int yr = DateTime.Today.Year;
                for (int i = 0; i < 5; i++)
                {
                    txt_year.Items.Add(string.Format("{0}/{1}", yr - i, (yr - i + 1).ToString()));
                }
                for (int i = 1; i < 4; i++)
                {
                    txt_semester.Items.Add("Semester "+i.ToString());
                }

                txt_semester.Title = "Sem|Quarter";
                txt_year.Title = "Academic Year";
            }
            catch (Exception) { }
        }

        private async void txt_semester_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await RefreshClaims();
            }
            catch (Exception ex)
            {
                lv_claims.ItemsSource = null;
                //await DisplayAlert("Error!", "" + ex.Message, "OK");
            }
        }
        protected async Task RefreshClaims()
        {
            MyDB DB = new MyDB();
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=myclaims&empcode={0}&acad={1}&sem={2}",
                        Application.Current.Properties["userno"], txt_year.SelectedItem.ToString(), txt_semester.SelectedItem.ToString().Substring(9,1));
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var content = await _client.GetStringAsync(webaddress);
                    //await DisplayAlert("Content ", "Sem " + content, "OK");
                    var n = JsonConvert.DeserializeObject<List<Model.TeachingClaimModel>>(content);
                    List<Model.TeachingClaimModel> TT_data = new List<Model.TeachingClaimModel>(n);
                    if (TT_data.Count > 0)
                    {
                        DB.resetClaims(txt_year.SelectedItem.ToString());
                        DB.AddClaims(txt_year.SelectedItem.ToString(),content);
                    }
                    else
                    {
                        DB.resetClaims(txt_year.SelectedItem.ToString());
                        lv_claims.ItemsSource = null;
                    }

                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
                catch (Exception ex)
                {

                    //await DisplayAlert("Error!", "" + ex.Message, "OK");
                    DB.resetClaims(txt_year.SelectedItem.ToString());
                    lv_claims.ItemsSource = null;
                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }

            var list = JsonConvert.DeserializeObject<List<Model.TeachingClaimModel>>(DB.GetAllClaims(txt_year.SelectedItem.ToString()));
            List<Model.TeachingClaimModel> display_data = new List<Model.TeachingClaimModel>(list);
            lv_claims.ItemsSource = display_data;
            lv_claims.EndRefresh();
        }

        private void lv_claims_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            int Semester = 0;
            if (txt_system.SelectedItem.ToString() == "Quarter")
            {
                Semester = int.Parse(txt_semester.SelectedItem.ToString().Substring(8, 1));
            }
            else if (txt_system.SelectedItem.ToString() == "Term")
            {
                Semester = int.Parse(txt_semester.SelectedItem.ToString().Substring(5, 1));
            }
            else
            {
                Semester = int.Parse(txt_semester.SelectedItem.ToString().Substring(9, 1));
            }
            TeachingClaimModel Item = (TeachingClaimModel)e.Item;
            Navigation.PushAsync(new ClaimLectures(Item.ID,Item.course_name,txt_year.SelectedItem.ToString(),Semester.ToString(),
                txt_system.SelectedItem.ToString()));
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            int Semester = 0;
            if (txt_system.SelectedItem.ToString() == "Quarter")
            {
                Semester = int.Parse(txt_semester.SelectedItem.ToString().Substring(8, 1));
            }
            else if (txt_system.SelectedItem.ToString() == "Term")
            {
                Semester = int.Parse(txt_semester.SelectedItem.ToString().Substring(5, 1));
            }
            else
            {
                Semester = int.Parse(txt_semester.SelectedItem.ToString().Substring(9, 1));
            }
            Navigation.PushAsync(new NewClaim(txt_year.SelectedItem.ToString(), Semester.ToString(),
                txt_system.SelectedItem.ToString()));
        }

        private async void txt_system_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txt_semester.ItemsSource = null;
                List<string> semlist = new List<string>(new string[]
                {
                txt_system.SelectedItem.ToString() + " 1",
                txt_system.SelectedItem.ToString()+" 2",
                txt_system.SelectedItem.ToString()+" 3" });
                txt_semester.ItemsSource = semlist;
                txt_semester.SelectedIndex = 0;
                txt_year.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "" + ex.Message, "OK");
            }
        }
        protected override void OnAppearing()
        {
            try
            {
                MyDB DB = new MyDB();
                var list = JsonConvert.DeserializeObject<List<Model.TeachingClaimModel>>(DB.GetAllClaims(txt_year.SelectedItem.ToString()));
                List<TeachingClaimModel> display_data = new List<Model.TeachingClaimModel>(list);
                lv_claims.ItemsSource = display_data;
                lv_claims.EndRefresh();
            }
            catch (Exception) { }
            base.OnAppearing();
        }
    }
}
