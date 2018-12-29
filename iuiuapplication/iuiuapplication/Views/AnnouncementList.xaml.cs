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
    public partial class AnnouncementList : ContentPage
    {
        private HttpClient _client = new HttpClient();

        public AnnouncementList()
        {
            InitializeComponent();
        }
        protected async Task RefreshAnnoucementList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;

                    string grad_webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=announcements&role={0}", Application.Current.Properties["role"]);
                    _client.Timeout = TimeSpan.Parse("00:00:15");

                    var announcement_content = await _client.GetStringAsync(grad_webaddress);
                    var n = JsonConvert.DeserializeObject<List<Model.AnnouncementModel>>(announcement_content);
                    List<Model.AnnouncementModel> book_data = new List<Model.AnnouncementModel>(n);
                    lv_announcements.ItemsSource = book_data;

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

        async protected override void OnAppearing()
        {
            await RefreshAnnoucementList();
            base.OnAppearing();
        }
    }
}