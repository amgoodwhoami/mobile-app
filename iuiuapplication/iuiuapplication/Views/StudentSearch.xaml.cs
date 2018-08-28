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
    public partial class StudentSearch : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public StudentSearch()
        {
            InitializeComponent();
        }

        private async void lv_students_Refreshing(object sender, EventArgs e)
        {
            await RefreshStudentList();
        }

        protected async Task RefreshStudentList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;

                    string grad_webaddress = Libraries.MobileConfig.GetWebAddress(txt_campus.SelectedItem.ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=studentsearch&searchtext={0}", txtSearch.Text);
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    if (txtSearch.Text.Length > 1)
                    {
                        var student_list = await _client.GetStringAsync(grad_webaddress);
                        var n = JsonConvert.DeserializeObject<List<Model.StudentSearchModel>>(student_list);
                        List<Model.StudentSearchModel> student_data = new List<Model.StudentSearchModel>(n);
                        await DisplayAlert("IUIU Mobile", student_data.Count + " Students Found", "OK");
                        lv_students.ItemsSource = student_data;
                    }
                    else { lv_students.ItemsSource = null; }
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
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }
        }

        private async void cmdSearchStudent_Clicked(object sender, EventArgs e)
        {
            await RefreshStudentList();
        }
    }
}
