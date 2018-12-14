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
    public partial class ProgrammeInformation : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public ProgrammeInformation()
        {
            InitializeComponent();
            txt_level.SelectedItem = "BACHELORS";

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
           await Search();
        }

        protected async Task RefreshProgrammes()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                try
                {
                    //App_activity_indicator.IsVisible = true;
                    //App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress("Main Campus") + string.Format("DataFinder.aspx?dataFormat=allprogrammes");
                    var content = await _client.GetStringAsync(webaddress);
                    //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + content, "OK");
                    var n = JsonConvert.DeserializeObject<List<Model.ProgrammeModel>>(content);
                    List<Model.ProgrammeModel> prog_data = new List<Model.ProgrammeModel>(n);
                    if (prog_data.Count>0)
                    {
                        //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + content, "OK");
                        // 

                        Application.Current.Properties["prog_json"] = content;

                    }
                    else
                    {

                        //await DisplayAlert("Error! ", "No Results Found", "OK");
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

        protected async override void OnAppearing()
        {
            await Search();
            await RefreshProgrammes();
            base.OnAppearing();
        }

        

        async Task Search()
        {
            List<Model.ProgrammeModel> programme_data=null;
            string level = "U";
            try
            {
               
                var n = JsonConvert.DeserializeObject<List<Model.ProgrammeModel>>(Application.Current.Properties["prog_json"].ToString());
                programme_data = new List<Model.ProgrammeModel>(n);
                string searchText = txtSearchText.Text;
                
                //if (txt_level.SelectedItem.ToString() == "UNDER GRADUATE") level = "U"; else if (txt_level.SelectedItem.ToString() == "GRADUATE") level = "P";
               // else if (txt_level.SelectedItem.ToString() == "DIPLOMA") level = "D"; else level = "C";


                List<Model.ProgrammeModel> filteredList = programme_data;
                if (searchText.Length > 0)
                {
                    //img_search.Source = "clear.png";
                    filteredList = programme_data.Where(c => (c.prog_name.Contains(searchText.ToUpper())) && c.level_name.Contains(txt_level.SelectedItem.ToString())).ToList();
                    lv_programmes.ItemsSource = filteredList;
                }
                else
                {
                    //img_search.Source = "search.png";
                    lv_programmes.ItemsSource = programme_data.Where(c => c.level_name.Contains(txt_level.SelectedItem.ToString())).ToList();
                }


            }
            catch (Exception ex)
            {
                try
                {
                    lv_programmes.ItemsSource = programme_data.Where(c => c.level_name.Contains(txt_level.SelectedItem.ToString())).ToList();
                }
                catch (Exception) { }
                //await DisplayAlert("Error ", "Error! " + ex.Message, "OK");
            }
        }

        private async void lv_programmes_Refreshing(object sender, EventArgs e)
        {
            await Search();
            lv_programmes.EndRefresh();
        }

        private void lv_programmes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ProgrammeModel Item = (ProgrammeModel)e.Item;
            Navigation.PushAsync(new ProgrammeProfile(Item.prog_id, Item.prog_name, Item.faculty,Item.requirements,
                Item.CourseLength));
        }

        private void lv_programmes_ChildAdded(object sender, ElementEventArgs e)
        {
        }
    }
}
