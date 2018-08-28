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
    public partial class CampusDirectory : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public CampusDirectory()
        {
            InitializeComponent();
            txt_status.SelectedIndex = 0;
            lbl_title.Text = Application.Current.Properties["campus"].ToString().ToUpper()+" DIRECTORY";
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await RefreshBookList();
        }

        private async void lv_directory_Refreshing(object sender, EventArgs e)
        {
            await RefreshBookList();
            lv_directory.EndRefresh();
        }

        protected async Task RefreshBookList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;

                    string grad_webaddress = Libraries.MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) +
                        string.Format("DataFinder.aspx?dataFormat=directory&txt={0}&cat={1}", txtSearch.Text, txt_status.SelectedItem.ToString());
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    if (txtSearch.Text.Length > 1)
                    {
                        var book_content = await _client.GetStringAsync(grad_webaddress);
                        var n = JsonConvert.DeserializeObject<List<Model.DirectoryModel>>(book_content);
                        List<Model.DirectoryModel> directory_data = new List<Model.DirectoryModel>(n);
                        await DisplayAlert("IUIU Mobile", directory_data.Count + " Found", "OK");
                        lv_directory.ItemsSource = directory_data;
                    }
                    else { lv_directory.ItemsSource = null; }
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
           // await RefreshBookList();
            base.OnAppearing();
        }

        private void lv_directory_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DirectoryModel Item = (DirectoryModel)e.Item;
            Navigation.PushAsync(new DirectoryProfile(Item.my_phone,Item.my_email,Item.my_name,Item.my_number));
        }
            
    }
}
