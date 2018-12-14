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
    public partial class RegistrationHistory : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public RegistrationHistory()
        {
            InitializeComponent();
            DisplayRegistration();
        }
        
        protected async Task RefreshPayments()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.Format("DataFinder.aspx?dataFormat=reghistory&regno={0}", Application.Current.Properties["userno"]);
                    var content = await _client.GetStringAsync(webaddress);
                    MyDB DB = new MyDB();

                    if (content != "[]")
                    {
                        DB.resetRegistration();
                        DB.AddRegistration(content);
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
            DisplayRegistration();
            base.OnAppearing();
        }

        void DisplayRegistration()
        {
            try
            {
                MyDB db = new MyDB();
                var n = JsonConvert.DeserializeObject<List<Model.RegistrationModel>>(db.GetAllRegistration());
                List<Model.RegistrationModel> reg_data = new List<Model.RegistrationModel>(n);
                List<Model.RegistrationModel> reg_list = new List<Model.RegistrationModel>();
                foreach (var reg_row in reg_data)
                {
                    Model.RegistrationModel studReg = new Model.RegistrationModel();
                    studReg.AcademicYr =reg_row.AcademicYr;
                    studReg.Sem = reg_row.studySys+" " + reg_row.Sem;
                    studReg.cyear = "YEAR: " + reg_row.cyear;
                    studReg.stat_name = reg_row.stat_name;
                    studReg.WardenRegStatus = reg_row.WardenRegStatus.ToUpper();
                    studReg.Clearance = reg_row.Clearance;
                    reg_list.Add(studReg);
                }

                lv_registration.ItemsSource = reg_list;
            }
            catch (Exception)
            {
                //DisplayAlert("Error ", "Error! " + ex.Message, "OK");
            }
        }

        private async void lv_registration_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Model.RegistrationModel studReg = (Model.RegistrationModel)e.Item;
           
            if(studReg.stat_name=="UNREGISTERED")
            {
                var official = await DisplayActionSheet("Self Registration", "Cancel", "", "Faculty Registration", "Register Course Units");
                if (official == "Register Course Units")
                {
                    await Navigation.PushAsync(new RegisterCourseUnits());
                }
                else
                {
                    await Navigation.PushModalAsync(new FacultyRegistration(studReg.AcademicYr, studReg.Sem));
                }
            }
            else
            {
                var official = DisplayActionSheet("Course Unit Management", "Close", "", "View Course Units");

            }
        }
    }
}
