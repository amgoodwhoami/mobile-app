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
    public partial class FeesPaymentInfo : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public FeesPaymentInfo()
        {
            InitializeComponent();
            int yr = DateTime.Today.Year;
           
            //txt_acadyear.SelectedIndex = 0;
            DisplayAlert("IUIU Mobile ", "Payments are coded Green and Billings coded Black", "OK");
            DisplayResults();
        }

        private void txt_acadyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayResults();
        }

        protected async Task RefreshPayments()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.Format("DataFinder.aspx?dataFormat=studentpayments&regno={0}", Application.Current.Properties["userno"]);
                    var content = await _client.GetStringAsync(webaddress);
                    //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + content, "OK");
                    MyDB DB = new MyDB();

                    if (content != "[]")
                    {
                        //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + content, "OK");
                        // 
                        DB.resetFeesPayment();
                        DB.AddFeesPayment(content);

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
            await RefreshPayments();
            DisplayResults();
            base.OnAppearing();
        }

        void DisplayResults()
        {
            try
            {
                MyDB db = new MyDB();
                var n = JsonConvert.DeserializeObject<List<Model.FeesPaymentModel>>(db.GetAllFeesPayments());
                List<Model.FeesPaymentModel> payment_data = new List<Model.FeesPaymentModel>(n);
                List<Model.FeesPaymentModel> acad_payments = new List<Model.FeesPaymentModel>();
                lbl_currentbalance.Text = "CURRENT BALANCE: -";
                foreach (var pay_data in payment_data)
                {
                    
                        Model.FeesPaymentModel studPay = new Model.FeesPaymentModel();
                        studPay.formated_date = pay_data.formated_date;
                        string transtype = "Billing";
                        string LabelColor = "Black";
                        if (pay_data.transactionType == "CR")
                        {
                            transtype = "Payment";
                            LabelColor = "Green";
                        }
                        studPay.transactionType = transtype;
                        studPay.transaction_amount = int.Parse(pay_data.transaction_amount).ToString("0,0");
                        studPay.particulars = pay_data.particulars;
                        studPay.curr_balance = "Running Balance: " + pay_data.curr_balance;


                        studPay.label_color = LabelColor;
                        acad_payments.Add(studPay);
                        lbl_currentbalance.Text = "CURRENT BALANCE: " + pay_data.curr_balance;
                    
                }

                lv_results.ItemsSource = acad_payments;
            }
            catch (Exception)
            {
                //DisplayAlert("Error ", "Error! " + ex.Message, "OK");
            }
        }
    }
}
