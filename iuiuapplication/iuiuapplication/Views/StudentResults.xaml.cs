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
    public partial class StudentResults : ContentPage
    {
        private HttpClient _client = new HttpClient();
        string campus_value;
        public StudentResults(string campus)
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
                    string webaddress = Libraries.MobileConfig.GetWebAddress(campus_value) + string.Format("DataFinder.aspx?dataFormat=studexamresults&regno={0}", Application.Current.Properties["userno"]);
                    var content = await _client.GetStringAsync(webaddress);
                    MyDB DB = new MyDB();

                    if (content != "[]")
                    {
                        //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + content, "OK");
                        // 
                        DB.resetResults();
                        DB.AddResults(content);

                    }
                    else
                    {

                        await DisplayAlert("Error! ", "No Results Found for "+ campus_value, "OK");
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
                var n = JsonConvert.DeserializeObject<List<Model.StudentResultModel>>(db.GetAllResults());
                List<Model.StudentResultModel> results_data = new List<Model.StudentResultModel>(n);
                List<Model.StudentResultModel> sem_results = new List<Model.StudentResultModel>();
                foreach (var res_data in results_data)
                {
                    if (res_data.ex_semester == txt_semester.SelectedItem.ToString().Substring(9, 1)
                        && res_data.c_year == txt_year.SelectedItem.ToString().Substring(5, 1) && res_data.stud_reg_no == Application.Current.Properties["userno"].ToString())
                    {
                        lbl_Cgpa.Text = res_data.cgpa;
                        lbl_degclass.Text = res_data.degclass;
                        lbl_gpa.Text = res_data.gpa;

                        Model.StudentResultModel studRes = new Model.StudentResultModel();
                        studRes.course_id = res_data.course_id;
                        studRes.course_name = res_data.course_name;
                        studRes.ex_mark = "Score: " + res_data.ex_mark;
                        studRes.grade = res_data.grade;
                        studRes.gradept = res_data.gradept;
                        studRes.gpa = res_data.gpa;
                        studRes.ex_acad_year = res_data.ex_acad_year;
                        studRes.cunits = "Credit Units: " + res_data.cunits;
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

        private void txt_semester_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayResults();
        }
    }
}
