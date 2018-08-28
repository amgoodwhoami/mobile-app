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
    public partial class TeachingAllocation : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public TeachingAllocation()
        {
            InitializeComponent();
            try
            {
                int yr = DateTime.Today.Year;
                
                for (int i = 0; i < 5; i++)
                {
                    txt_year.Items.Add(string.Format("{0}/{1}", yr - i, (yr - i + 1).ToString()));
                }
                //txt_year.SelectedIndex = 0;

               
                List<string> semlist = new List<string>(new string[]
                {
                 "Semester 1",
                 "Semester 2",
                 "Semester 3" });
                txt_semester.ItemsSource = semlist;
                //txt_semester.SelectedIndex = 0;

                txt_semester.Title = "Semester";
                txt_year.Title = "Academic Year";
                RefreshData();
            }
            catch (Exception) { }

        }
        private void txt_semester_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                lv_timetable.ItemsSource = null;
                //await DisplayAlert("Error!", "" + ex.Message, "OK");
            }
        }
        protected async Task RefreshAllocations()
        {
            MyDB DB = new MyDB();
            //if (CrossConnectivity.Current.IsConnected)
            //{
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=mycourses&empcode={0}&acad={1}&sem={2}",
                        Application.Current.Properties["userno"], txt_year.SelectedItem.ToString(), txt_semester.SelectedItem.ToString().Substring(9, 1));
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var content = await _client.GetStringAsync(webaddress);

                    var n = JsonConvert.DeserializeObject<List<Model.CourseAllocationModel>>(content);
                    List<Model.CourseAllocationModel> TT_data = new List<Model.CourseAllocationModel>(n);
                    if (TT_data.Count > 0)
                    {
                        DB.resetCourseAllocations();
                        DB.AddCourseAllocation(Application.Current.Properties["userno"].ToString(),content);
                    }
                    else
                    {
                        DB.resetCourseAllocations();
                        lv_timetable.ItemsSource = null;
                    }

                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
                catch (Exception ex)
                {

                    //await DisplayAlert("Error!", "" + ex.Message, "OK");
                   // DB.resetCourseAllocations();
                    lv_timetable.ItemsSource = null;
                    App_activity_indicator.IsVisible = false;
                    App_activity_indicator.IsRunning = false;
                }
            //}
            //else
            //{
            //    await DisplayAlert("Warning", "No Internet Connection", "OK");
            //}

            var list = JsonConvert.DeserializeObject<List<Model.CourseAllocationModel>>(DB.GetMyCourseALlocations(Application.Current.Properties["userno"].ToString()));
            List<Model.CourseAllocationModel> display_data = new List<Model.CourseAllocationModel>(list);
            display_data = display_data.Where(c => c.sem == int.Parse(txt_semester.SelectedItem.ToString().Substring(9, 1))
                  && c.acad_year == txt_year.SelectedItem.ToString()).ToList();
            lv_timetable.ItemsSource = display_data;
        }
        async void RefreshData()
        {
            try
            {
                await RefreshAllocations();
            }
            catch (Exception)
            {
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
                var n = JsonConvert.DeserializeObject<List<Model.CourseAllocationModel>>(db.GetMyCourseALlocations(Application.Current.Properties["userno"].ToString()));
                List<Model.CourseAllocationModel> grad_data = new List<Model.CourseAllocationModel>(n);
                string searchText = txtSearch.Text;
                List<Model.CourseAllocationModel> filteredList = grad_data;
                filteredList = grad_data.Where(c => c.course_name.Contains(searchText.ToUpper()) 
                && c.sem== int.Parse(txt_semester.SelectedItem.ToString().Substring(9, 1)) 
                && c.acad_year==txt_year.SelectedItem.ToString()).ToList();
                lv_timetable.ItemsSource = filteredList;

            }
            catch (Exception)
            {
                lv_timetable.ItemsSource = null;
                DisplayAlert("Error ", "Error! Try again", "OK");
            }
        }

        private void lv_timetable_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CourseAllocationModel Item = (CourseAllocationModel)e.Item;
            Navigation.PushAsync(new StaffResultsCentre(int.Parse(Item.ID),Item.course_id,Item.course_name,Item.sem.ToString(),
                Item.cyear.ToString(),Item.acad_year,Item.progname,Item.lect_session, Item.prog_id));
        }
    }
}
