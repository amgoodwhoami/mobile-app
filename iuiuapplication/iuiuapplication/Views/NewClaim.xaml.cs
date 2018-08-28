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
    public partial class NewClaim : ContentPage
    {
        string acad_year, semester,study_sys;
        private HttpClient _client = new HttpClient();
        private void Button_Clicked(object sender, EventArgs e)
        {
            AddClaim();
        }

        public NewClaim(string acad,string sem,string studysys)
        {
            InitializeComponent();
            acad_year = acad;
            semester = sem;
            study_sys = studysys;
            lbl_header.Text = string.Format("NEW CLAIM FOR {0}, {2} {1}",acad,sem,studysys).ToUpper();
            RefreshTimetables(int.Parse(sem));
        }

        
        protected async void RefreshTimetables(int Sem)
        {
            MyDB DB = new MyDB();
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;

                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=personaltimetable&code={0}&acad={1}&sem={2}&system={3}",
                        Application.Current.Properties["userno"], acad_year, Sem, study_sys);
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var content = await _client.GetStringAsync(webaddress);
                    //await DisplayAlert("Error!", ""+ content, "OK");
                    var n = JsonConvert.DeserializeObject<List<Model.TimetableModel>>(content);
                    List<Model.TimetableModel> TT_data = new List<Model.TimetableModel>(n);
                    if (TT_data.Count > 0)
                    {
                        DB.resetTimetables(acad_year,semester);
                        DB.AddTimetables(content, acad_year, semester);
                    }
                    else
                    {
                        DB.resetTimetables(acad_year, semester);
                        txtclass.ItemsSource = null;
                    }

                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error!", "" + ex.Message, "OK");
                    DB.resetTimetables(acad_year, semester);
                    txtclass.ItemsSource = null;
                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }

            var list = JsonConvert.DeserializeObject<List<TimetableModel>>(DB.GetAllTimetables(acad_year, semester));
            List<TimetableModel> display_data = new List<TimetableModel>(list);
            List<TimetableModel> combo_data = new List<TimetableModel>();
            foreach(TimetableModel item in display_data )
            {
                TimetableModel a = new TimetableModel();
                a.SID = item.SID;
                a.duration = string.Format("{0} - {2} {1}",item.course_name,item.duration,item.lecture_day);
                combo_data.Add(a);
            }
            txtclass.ItemsSource = combo_data;
        }
        protected async void AddClaim()
        {
            MyDB DB = new MyDB();
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {

                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    TimetableModel TT = new TimetableModel();
                    TT = (TimetableModel)txtclass.SelectedItem;

                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=newclaim&empcode={0}&acad={1}&sem={2}&SID={3}",
                        Application.Current.Properties["userno"], acad_year, semester, TT.SID);
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var content = await _client.GetStringAsync(webaddress);
                    
                    if(content.Contains("Created"))
                    {
                        await DisplayAlert("IUIU Mobile", content, "OK");
                        await Navigation.PushAsync(new ClaimingCentre());
                    }
                    else
                    {
                        await DisplayAlert("IUIU Mobile", "An Error Occurred. Try again", "OK");
                    }
                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error!", "" + ex.Message, "OK");
                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }

            
        }
    }
}
