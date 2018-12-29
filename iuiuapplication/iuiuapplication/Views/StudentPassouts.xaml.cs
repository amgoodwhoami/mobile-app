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
    public partial class StudentPassouts : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public StudentPassouts()
        {
            InitializeComponent();
            try
            {
                int yr = DateTime.Today.Year;
                for (int i = 0; i < 5; i++)
                {
                    txt_acadyear.Items.Add(string.Format("{0}/{1}", yr - i, (yr - i + 1).ToString()));
                }
                txt_acadyear.SelectedIndex = 0;
                txt_semester.SelectedIndex = 0;
            }
            catch (Exception) { }
        }
        private void lv_mycourses_Refreshing(object sender, EventArgs e)
        {

        }
        protected async Task RefreshCourses()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=student_courses&regno={0}&acad={1}&sem={2}",
                        Application.Current.Properties["userno"], txt_acadyear.SelectedItem.ToString(), txt_semester.SelectedItem.ToString().Substring(9, 1));
                    var content = await _client.GetStringAsync(webaddress);

                    if (content != "[]")
                    {
                        Application.Current.Properties["mycourses"] = content;
                    }
                    else
                    {

                        await DisplayAlert("Error! ", "No Course Units Found", "OK");
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
            await RefreshCourses();
            DisplayCourses();
            base.OnAppearing();
        }

        void DisplayCourses()
        {
            try
            {
                var n = JsonConvert.DeserializeObject<List<Model.StudentCourseModel>>(Application.Current.Properties["mycourses"].ToString());
                List<Model.StudentCourseModel> courses_data = new List<Model.StudentCourseModel>(n);
                lv_mycourses.ItemsSource = courses_data;
            }
            catch (Exception)
            {
                //DisplayAlert("Error ", "Error! " + ex.Message, "OK");
            }
        }

        private async void lv_courses_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Model.StudentCourseModel studReg = (Model.StudentCourseModel)e.Item;
            await DisplayAlert("IUIU Mobile ", "Content Display Initiated", "OK");
        }
        private async void txt_semester_SelectedIndexChanged(object sender, EventArgs e)
        {
            await RefreshCourses();
            DisplayCourses();
        }
    }
}