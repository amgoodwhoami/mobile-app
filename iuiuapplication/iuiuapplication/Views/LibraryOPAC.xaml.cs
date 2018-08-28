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
    public partial class LibraryOPAC : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public LibraryOPAC()
        {
            InitializeComponent();
            txt_campus.SelectedIndex = 0;
            txt_SortOrder.SelectedIndex = 0;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await RefreshBookList();
        }

        private async void lv_opac_Refreshing(object sender, EventArgs e)
        {
            await RefreshBookList();
        }

        protected async Task RefreshBookList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                   
                    string grad_webaddress = Libraries.MobileConfig.GetWebAddress(txt_campus.SelectedItem.ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=opacsearch&txt={0}&sortorder={1}", txtSearch.Text, txt_SortOrder.SelectedItem.ToString());
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    if (txtSearch.Text.Length > 1)
                    {
                        var book_content = await _client.GetStringAsync(grad_webaddress);
                        var n = JsonConvert.DeserializeObject<List<Model.BookModel>>(book_content);
                        List<Model.BookModel> book_data = new List<Model.BookModel>(n);
                        await DisplayAlert("IUIU Mobile", book_data.Count+" Items Found", "OK");
                        lv_opac.ItemsSource = book_data;
                    }
                    else { lv_opac.ItemsSource = null;}
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
            await RefreshBookList();
            base.OnAppearing();
        }



       
    }
}
