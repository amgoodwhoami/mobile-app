using iuiuapplication.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iuiuapplication
{
    public partial class MainPage : ContentPage
    {
        int tapCount = 0;
        public MainPage()
        {
            InitializeComponent();
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            tapCount++;

            var imageSender = (Image)sender;
            if (imageSender.Id == fb.Id)
            {
                DisplayAlert("IUIU Mobile", "FaceBook Activated", "OK");
            }
            else if (imageSender.Id == web.Id)
            {

            }
            else if (imageSender.Id == fb.Id)
            {
                DisplayAlert("IUIU Mobile", "FaceBook Activated", "OK");
            }
        }

        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

           await Navigation.PushAsync(new Login());
        }

        async private void WebIcon_Tapped(object sender, EventArgs e)
        {
            WebsiteViewer Web = new WebsiteViewer("http://www.iuiu.ac.ug");
            await Navigation.PushAsync(Web);
        }
        async private void FaceBook_Tapped(object sender, EventArgs e)
        {
            WebsiteViewer Web = new WebsiteViewer("https://web.facebook.com/pages/Islamic-University-in-Uganda/103155923058209");
            await Navigation.PushAsync(Web);
        }

        //

        private async void Call_Tapped(object sender, EventArgs e)
        {
            var official=await DisplayActionSheet("IUIU Instant Call", "Cancel","", "Academic Registrar", "University Secretary");
            if (official == "Academic Registrar")
            {
                Device.OpenUri(new Uri("tel:+256701883338"));
            }
            else if (official == "University Secretary")
            {
                Device.OpenUri(new Uri("tel:+256702325330"));
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Application.Current.SavePropertiesAsync();

        }

        private  async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProgrammeInformation());
        }
        private async void coming_soon(object sender, EventArgs e)
        {
            await DisplayAlert("IUIU Mobile", "Coming in Verion 2.0", "OK");
        }
        private async void GraduationListViewer(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GraduantionLists());
        }
        private async void OPACViewer(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LibraryOPAC());
        }

        private async void studentSearch(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StudentSearch());
        }
    }
}
