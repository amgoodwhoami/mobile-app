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
    public partial class SalaryDetails : ContentPage
    {
        private HttpClient _client = new HttpClient();
        string _Category, _month, _year;
        public SalaryDetails(string Category, string month, string year)
        {
            InitializeComponent();
            _Category = Category;
            _month = month;
            _year = year;
            lbl_header.Text = string.Format("{0} DETAILS FOR {1} {2}",Category.ToUpper(), month,year);

        }

        protected async Task RefreshSalaryDetails()
        {
            //if (CrossConnectivity.Current.IsConnected)
            //{

                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=salarydetails&category={1}&years={2}&months={3}&empcode={0}",
                        Application.Current.Properties["userno"], _Category, _year, _month);
                    var content = await _client.GetStringAsync(webaddress);
                    //await DisplayAlert("Error ", "Error! " + content, "OK");
                    MyDB DB = new MyDB();

                    if (content != "[]")
                    {
                        DB.resetSalaryDetails(Application.Current.Properties["userno"].ToString(), _month, _year, _Category);
                        var n = JsonConvert.DeserializeObject<List<Model.SalaryDetailsModel>>(content);
                        List<Model.SalaryDetailsModel> salary_data = new List<Model.SalaryDetailsModel>(n);
                        //await DisplayAlert("Error ", "Counter! " + salary_data.Count, "OK");
                        foreach (var salary in salary_data)
                        {

                            DB.AddSalaryDetail(salary.item, salary.amount, salary.months, salary.Years, Application.Current.Properties["userno"].ToString(), _Category);
                        }
                    }
                    else
                    {
                        DB.resetSalaryDetails(Application.Current.Properties["userno"].ToString(), _month, _year, _Category);
                        await DisplayAlert("Error! ", "No Results Found", "OK");
                        lv_salary_summary.ItemsSource = null;
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
        //}
        void DisplaySalaryInfo()
        {

            try
            {
                MyDB db = new MyDB();
                lv_salary_summary.ItemsSource = db.GetMonthlySalaryDetails(Application.Current.Properties["userno"].ToString(), _month, _year, _Category);
            }
            catch (Exception)
            {
                //DisplayAlert("Error ", "Error! " + ex.Message, "OK");
            }
        }
        async protected override void OnAppearing()
        {
            await RefreshSalaryDetails();
            DisplaySalaryInfo();
            base.OnAppearing();
        }
        
    }
}
