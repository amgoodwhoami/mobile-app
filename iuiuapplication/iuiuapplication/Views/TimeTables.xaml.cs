using iuiuapplication.DB;
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
    public partial class TimeTables : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public TimeTables()
        {
            InitializeComponent();
            try
            {
                int yr = DateTime.Today.Year;
                for (int i = 0; i < 5; i++)
                {
                    txt_year.Items.Add(string.Format("{0}/{1}", yr-i, (yr - i+1).ToString()));
                }
                txt_system.Title="Study System";
                txt_semester.Title = "Sem|Quarter";
                txt_year.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private  void txt_semester_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)   {
                lv_timetable.ItemsSource = null;
                //await DisplayAlert("Error!", "" + ex.Message, "OK");
            }
        }
        protected async Task RefreshTimetables(int semester)
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
                        Application.Current.Properties["userno"], txt_year.SelectedItem.ToString(),
                        semester, txt_system.SelectedItem.ToString());
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var content = await _client.GetStringAsync(webaddress);
                   
                    var n = JsonConvert.DeserializeObject<List<Model.TimetableModel>>(content);
                    List<Model.TimetableModel> TT_data = new List<Model.TimetableModel>(n);
                    if(TT_data.Count>0)
                    {
                        DB.resetTimetables(txt_year.SelectedItem.ToString(),txt_semester.SelectedItem.ToString());
                        DB.AddTimetables(content, txt_year.SelectedItem.ToString(), txt_semester.SelectedItem.ToString());
                    }
                    else
                    {
                        DB.resetTimetables(txt_year.SelectedItem.ToString(), txt_semester.SelectedItem.ToString());
                        lv_timetable.ItemsSource = null;
                    }

                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Error!", ""+ex.Message, "OK");
                    DB.resetTimetables(txt_year.SelectedItem.ToString(), txt_semester.SelectedItem.ToString());
                    lv_timetable.ItemsSource = null;
                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }

            var list = JsonConvert.DeserializeObject<List<Model.TimetableModel>>(DB.GetAllTimetables(txt_year.SelectedItem.ToString(), txt_semester.SelectedItem.ToString()));
            List<Model.TimetableModel> display_data = new List<Model.TimetableModel>(list);
            lv_timetable.ItemsSource = display_data;
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
                RefreshData();
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error!", "" + ex.Message, "OK");
            }
        }
        async void RefreshData()
        {
            try
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
                await RefreshTimetables(Semester);
            }
            catch (Exception) {
                lv_timetable.ItemsSource = null;
            }

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        void Search()
        {
            try
            {
               
                    MyDB db = new MyDB();
                    var n = JsonConvert.DeserializeObject<List<Model.TimetableModel>>(db.GetAllTimetables(txt_year.SelectedItem.ToString(), txt_semester.SelectedItem.ToString()));
                    List<Model.TimetableModel> grad_data = new List<Model.TimetableModel>(n);
                    string searchText = txtSearch.Text;
                    List<Model.TimetableModel> filteredList = grad_data;
                    filteredList = grad_data.Where(c => c.course_name.Contains(searchText.ToUpper())).ToList();
                    lv_timetable.ItemsSource = filteredList;
                
            }
            catch (Exception)
            {
                lv_timetable.ItemsSource = null;
                DisplayAlert("Error ", "Error! Try again", "OK");
            }
        }
    }
}
