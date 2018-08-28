using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iuiuapplication.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullProfile : ContentPage
    {
        string campus_value, regno_value, role_value;
        public FullProfile(string campus, string users_name, string phone, string email, string userno, string userrole)
        {
            InitializeComponent();

            if (userrole == "Student")
            {
                img_profilepic.Source = UriImageSource.FromUri(new Uri(Libraries.MobileConfig.GetPhotoAddress(campus) + Application.Current.Properties["userno"] + "_photo.jpg"));
            }
            else
            {
                img_profilepic.Source = "no_photo.png";
            }

            txt_profile_username.Text = "WELCOME  ::  " + users_name.ToUpper();

            campus_value = campus;
            regno_value = userno;
            role_value = userrole;

            txt_profile_username.Text = users_name.ToUpper();
            txt_profile_userno.Text = regno_value;
            txt_profile_phone.Text = phone;
            txt_profile_email.Text = email;

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Warning", "You will loose all saved info on the phone. Proceed with Logout?", "Yes", "No");
            if (answer)
            {
                Application.Current.Properties["userno"] = "-";
                Application.Current.Properties["username"] = "Guest";
                Application.Current.Properties["photo"] = "-";
                Application.Current.Properties["phone"] = "-";
                Application.Current.Properties["email"] = "-";
                Application.Current.Properties["role"] = "-";
                await Application.Current.SavePropertiesAsync();
                await Navigation.PopToRootAsync();
            }

        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
        protected override void OnAppearing()
        {
            if (role_value == "Student")
            {
                img_profilepic.Source = UriImageSource.FromUri(new Uri(Libraries.MobileConfig.GetPhotoAddress(campus_value) + Application.Current.Properties["userno"] + "_photo.jpg"));
            }
            else
            {
                img_profilepic.Source = "no_photo.png";
            }
            base.OnAppearing();
        }
    }

   
}
