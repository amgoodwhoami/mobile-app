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
    public partial class SalarySummary : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public SalarySummary()
        {
            InitializeComponent();
            int yr = DateTime.Today.Year;
            for (int i = 0; i < 5; i++)
            {
                txt_year.Items.Add((yr - i).ToString());
            }
        }

        protected async Task RefreshSalary()
        {
            //if (CrossConnectivity.Current.IsConnected)
            //{

                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=salarysummary&empcode={0}&years={2}&months={1}",
                        Application.Current.Properties["userno"], txt_month.SelectedItem, txt_year.SelectedItem);
                    var content = await _client.GetStringAsync(webaddress);
                    MyDB DB = new MyDB();

                    if (content != "[]")
                    {
                        DB.resetSalarySummary(Application.Current.Properties["userno"].ToString(), txt_month.SelectedItem.ToString(), txt_year.SelectedItem.ToString());
                        var n = JsonConvert.DeserializeObject<List<Model.SalarySummaryModel>>(content);
                        List<Model.SalarySummaryModel> salary_data = new List<Model.SalarySummaryModel>(n);
                        string more_img = "-";
                        foreach (var salary in salary_data)
                        {
                            if (salary.Item.Contains("Total") || salary.Item.Contains("Deductions"))
                            {
                                more_img = "more_info.png";
                            }
                            else
                            {
                                more_img = "-";
                            }
                            DB.AddSalarySummary(salary.Item, salary.amount, salary.months, salary.Years, salary.empCodes, more_img);
                        }
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

            //}
            //else
            //{
            //    await DisplayAlert("IUIU Mobile ", "No Connection. Saved Data will be used", "OK");
            //}

        }

        void DisplayResults()
        {

            try
            {
                MyDB db = new MyDB();
                lv_salary_summary.ItemsSource = db.GetMonthlySalary(Application.Current.Properties["userno"].ToString(), txt_month.SelectedItem.ToString(), txt_year.SelectedItem.ToString());
            }
            catch (Exception)
            {
                //DisplayAlert("Error ", "Error! " + ex.Message, "OK");
            }
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
        }

        private async void txt_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            await RefreshSalary();
            DisplayResults();
        }

        private void lv_salary_summary_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            SalarySummaryModel item = (SalarySummaryModel)e.Item;
            String Category;
            if(item.Item.Contains("Allowance")){ Category = "Allowances"; } else { Category = "Deductions"; }
            if (item.Item.Contains("Allowance") || item.Item.Contains("Deductions"))
            {
                Navigation.PushAsync(new SalaryDetails(Category, item.months, item.Years));
            }
        }
    }
}
