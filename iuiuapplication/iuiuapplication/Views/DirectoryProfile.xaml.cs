using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iuiuapplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectoryProfile : ContentPage
    {
        string phone, email,p_names,p_number;
        public DirectoryProfile(string p_hone,string e_mail,string names,string number)
        {
            InitializeComponent();
            phone = p_hone;
            email = e_mail;
            p_names = names;
            p_number = number;
            try
            {
                if (phone.StartsWith("0"))
                    phone = "+256" + phone.Substring(1, 9);
                    
                else
                    phone = "+"+phone.Substring(0, 10);
            }
            catch { DisplayAlert("Warning", "Phone Number is not valid. No calling is possible.", "OK"); }

            txt_profile_username.Text = names;
            txt_profile_userno.Text = number;
            txt_profile_phone.Text = phone;
            txt_profile_email.Text = e_mail;
        }

        private void Call_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("tel:+"+phone));
        }
        private void Emailer_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("IUIU Mobile", "Email Activated", "OK");
        }
       
    }
}
