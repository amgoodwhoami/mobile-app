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
    public partial class GraduantionLists : ContentPage
    {
        private HttpClient _client = new HttpClient();
        
        public GraduantionLists()
        {
            InitializeComponent();
            int yr = DateTime.Today.Year;
            for (int i = 0; i < 5; i++)
            {
                txt_year.Items.Add((yr - i).ToString());
            }
            txt_year.SelectedIndex = 0;
           
        }
        protected async Task RefreshProgrammes()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    string webaddress = Libraries.MobileConfig.GetWebAddress("Main Campus") + string.Format("DataFinder.aspx?dataFormat=gradprogrammelist");
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var content = await _client.GetStringAsync(webaddress);
                    MyDB DB = new MyDB();
                    var prg = JsonConvert.DeserializeObject<List<Model.ProgrammeModel>>(content);
                    List<Model.ProgrammeModel> prgList = new List<Model.ProgrammeModel>(prg);
                    
                    if (prgList.Count>0)
                    {
                        DB.resetProgrammes();
                        DB.AddProgrammes(content);
                    }
                    else
                    {
                        await DisplayAlert("IUIU Mobile", "No Data Found. Check Your Connection", "OK");
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
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }
            MyDB db = new MyDB();
            var n = JsonConvert.DeserializeObject<List<Model.ProgrammeModel>>(db.GetAllProgrammes());
            List<Model.ProgrammeModel> programme_data = new List<Model.ProgrammeModel>(n);
            txtProgramme.ItemsSource = programme_data;
        }

        protected async Task RefreshGraduands()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;
                    ProgrammeModel prog = (ProgrammeModel)txtProgramme.SelectedItem;
                    string grad_webaddress = Libraries.MobileConfig.GetWebAddress("Main Campus") + 
                        string.Format("DataFinder.aspx?dataFormat=graduationlist&acad={0}&prog={1}", txt_year.SelectedItem.ToString(), prog.prog_id);
                    _client.Timeout = TimeSpan.Parse("00:00:15");
                    var grad_content = await _client.GetStringAsync(grad_webaddress);
                    MyDB DB = new MyDB();

                    var n = JsonConvert.DeserializeObject<List<Model.GraduandModel>>(grad_content);
                    List<Model.GraduandModel> grad_data = new List<Model.GraduandModel>(n);

                    if (grad_data.Count>0)
                    {
                       //await DisplayAlert("IUIU Mobile",grad_content, "OK");
                        DB.resetGraduands(prog.prog_id);
                        DB.AddGraduands(grad_content,prog.prog_id);
                    }
                    else
                    {
                         await DisplayAlert("IUIU Mobile", "No Data Found. Check Your Connection", "OK");
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
                await DisplayAlert("Warning", "No Internet Connection", "OK");
            }
        }

        async protected override void OnAppearing()
        {
            await RefreshProgrammes();     
            base.OnAppearing();
        }

      

        void DisplayGraduation(string prog)
        {
            try
            {
                if (prog != null)
                {
                    MyDB db = new MyDB();
                    var n = JsonConvert.DeserializeObject<List<Model.GraduandModel>>(db.GetAllGraduands(prog));
                    List<Model.GraduandModel> grad_data = new List<Model.GraduandModel>(n);
                    lv_graduands.ItemsSource = grad_data;
                    DisplayAlert("Summary ", grad_data.Count+ " Graduands Found", "OK");
                }
            }
            catch (Exception)
            {
                lv_graduands.ItemsSource = null;
                DisplayAlert("Error ", "No Data Available. Connect and try again", "OK");
            }
        }

        void Search(string prog)
        {
            try
            {
                if (prog != null)
                {
                    MyDB db = new MyDB();
                    var n = JsonConvert.DeserializeObject<List<Model.GraduandModel>>(db.GetAllGraduands(prog));
                    List<Model.GraduandModel> grad_data = new List<Model.GraduandModel>(n);
                    string searchText = txtSearch.Text;
                    List<Model.GraduandModel> filteredList = grad_data;
                    filteredList = grad_data.Where(c => c.stud_name.Contains(searchText.ToUpper())).ToList();
                    lv_graduands.ItemsSource = filteredList;
                }
            }
            catch (Exception)
            {
                lv_graduands.ItemsSource = null;
                DisplayAlert("Error ", "Error! Try again", "OK");
            }
        }

        private  async void lv_programmes_Refreshing(object sender, EventArgs e)
        {
            try
            {
                await RefreshGraduands();
                ProgrammeModel prog = (ProgrammeModel)txtProgramme.SelectedItem;
                DisplayGraduation(prog.prog_id);
                lv_graduands.EndRefresh();
            }
            catch (Exception) {
                lv_graduands.ItemsSource = null;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ProgrammeModel prog = (ProgrammeModel)txtProgramme.SelectedItem;
                Search(prog.prog_id);
            }
            catch (Exception) { }
        }


    }

}
