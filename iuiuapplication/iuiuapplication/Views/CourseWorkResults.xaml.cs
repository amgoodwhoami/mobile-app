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
    public partial class CourseWorkResults : ContentPage
    {
        private HttpClient _client = new HttpClient();
        string campus_value;
        public CourseWorkResults(string campus)
        {
            InitializeComponent();
            campus_value = campus;
            txt_year.SelectedIndex = 0;
            txt_semester.SelectedIndex = 0;

        }

        protected async Task RefreshResults()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(campus_value) + string.Format("DataFinder.aspx?dataFormat=studcourseworkresults&regno={0}", Application.Current.Properties["userno"]);
                    var content = await _client.GetStringAsync(webaddress);
                    MyDB DB = new MyDB();
                    //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + content, "OK");

                    if (content != "[]")
                    {
                        // await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + content, "OK");
                        // 
                        DB.resetCourseworkResults();
                        DB.AddCourseworkResults(content);

                    }
                    else
                    {

                        await DisplayAlert("Error! ", "No Results Found", "OK");
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
                //await DisplayAlert("IUIU Mobile ", "No Connection. Saved Data will be used", "OK");
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
                MyDB db = new MyDB();
                var n = JsonConvert.DeserializeObject<List<Model.StudentCourseworkModel>>(db.GetAllCourseworkResults());
                List<Model.StudentCourseworkModel> results_data = new List<Model.StudentCourseworkModel>(n);
                List<Model.StudentCourseworkModel> sem_results = new List<Model.StudentCourseworkModel>();
                foreach (var res_data in results_data)
                {
                    if (res_data.sem == txt_semester.SelectedItem.ToString().Substring(9, 1)
                        && res_data.cyear == txt_year.SelectedItem.ToString().Substring(5, 1) && res_data.stud_reg_no == Application.Current.Properties["userno"].ToString())
                    {

                        Model.StudentCourseworkModel studRes = new Model.StudentCourseworkModel();
                        studRes.course_id = res_data.course_id;
                        studRes.course_name = res_data.course_name;
                        studRes.cw1 = "CW1: " + res_data.cw1;
                        studRes.cw2 = "CW2: " + res_data.cw2;
                        studRes.cw3 = "CW3: " + res_data.cw3;
                        studRes.cw4 = "CW4: " + res_data.cw4;
                        studRes.cw5 = "CW5: " + res_data.cw5;
                        studRes.total = "FINAL: " + res_data.total;
                        sem_results.Add(studRes);
                    }
                }

                lv_results.ItemsSource = sem_results;
            }
            catch (Exception)
            {
                //DisplayAlert("Error ", "Error! " + ex.Message, "OK");
            }
        }

        private void txt_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            //await RefreshResults();
            DisplayResults();
        }
    }
}
