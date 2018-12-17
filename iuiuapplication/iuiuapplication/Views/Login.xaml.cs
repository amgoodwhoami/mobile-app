using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Diagnostics;

namespace iuiuapplication.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private HttpClient _client = new HttpClient();
        string stat = "Back";

        public Login(string status)
        {
            InitializeComponent();
            stat = status;
            txt_campus.SelectedIndex = 0;
            

        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("IUIU Mobile", "Login Initiated @ "+txt_campus.SelectedItem, "OK");
            try
            {
                await Verificication();
            }
            catch (Exception ey)
            {
                await DisplayAlert("Error!  ", ey.Message, "Ok");

            }
        }

        protected async Task Verificication()
        {
            //if (CrossConnectivity.Current.IsConnected)
            //{
                string webaddress="-";

                try
                {
                    App_activity_indicator.IsVisible = true;
                    App_activity_indicator.IsRunning = true;

                    //Activity indicator visibility on

                    //news_activity_indicator.IsRunning = true;
                    webaddress = Libraries.MobileConfig.GetWebAddress(txt_campus.SelectedItem.ToString()) + string.Format("remote_login.aspx?dataFormat=Login&unm={0}&passwd={1}",
                        txtusername.Text, txtpassword.Text);
                    //await DisplayAlert("IUIU Mobile ", "Accessing Web Location: " + webaddress, "OK");
                    var content = await _client.GetStringAsync(webaddress);
                     if (content != "[]")
                      {
                          var n = JsonConvert.DeserializeObject<List<Model.UserModel>>(content);
                          List<Model.UserModel> userprofile = new List<Model.UserModel>(n);
                          foreach (var user in userprofile)
                          {
                              if (user.user_role == "Student")
                              {
                                  //await DisplayAlert("IUIU Mobile ", "Access Granted as Student", "OK");
                                  UserSettings(user.user_no, user.user_names, user.user_photo, user.user_phone, user.user_email, user.user_role);
                                  await Navigation.PushAsync(new UserProfile(txt_campus.SelectedItem.ToString(), user.user_names, user.user_phone, user.user_email, user.user_no, user.user_role));

                              }
                              else if (user.user_role == "Staff")
                              {
                                 // await DisplayAlert("IUIU Mobile ", "Access Granted as Staff", "OK");
                                  UserSettings(user.user_no, user.user_names, user.user_photo, user.user_phone, user.user_email, user.user_role);
                                  await Navigation.PushAsync(new UserProfile(txt_campus.SelectedItem.ToString(), user.user_names, user.user_phone, user.user_email, user.user_no, user.user_role));

                              }
                              else
                              {
                                  await DisplayAlert("IUIU Mobile ", "Permission Denied. Check and Try Again ", "OK");

                              }
                          }
                          App_activity_indicator.IsRunning = false;
                      }
                      else
                      {
                          App_activity_indicator.IsRunning = false;
                          App_activity_indicator.IsVisible = false;
                          await DisplayAlert("IUIU Mobile ", "Permission Denied. Check and Try Again ", "OK");
                      }


                }
                catch (Exception ey)
                {
                    App_activity_indicator.IsRunning = false;
                    App_activity_indicator.IsVisible = false;

                    await DisplayAlert("General Error!  ", "" + ey.InnerException.Message+"["+ webaddress+"]", "Ok");

                }

            //}

            //else
            //{
            //    App_activity_indicator.IsRunning = false;
            //    App_activity_indicator.IsVisible = false;
            //    // newslist.IsVisible = false;
            //    await DisplayAlert("Alert ", "No internet Connection Please ", "Ok");

            //}
        }


        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Warning", "Do you want to RESET your password?", "Yes", "No");
            if (answer)
            {
                await DisplayAlert("IUIU Mobile", "Password Reset Completed. Await SMS", "OK");
            }
        }

        void UserSettings(string userno, string username, string photo, string phone, string email, string role)
        {
            Application.Current.Properties["userno"] = userno;
            Application.Current.Properties["username"] = username;
            Application.Current.Properties["photo"] = photo;
            Application.Current.Properties["phone"] = phone;
            Application.Current.Properties["email"] = email;
            Application.Current.Properties["role"] = role;
            Application.Current.Properties["campus"] = txt_campus.SelectedItem;
            Application.Current.SavePropertiesAsync();
        }

        async protected override void OnAppearing()
        {
            try
            {
                if (stat=="Back")
                {
                    await Navigation.PopToRootAsync();
                }
                else if (Application.Current.Properties["userno"].ToString().Length > 1)
                {
                    
                    await Navigation.PushAsync(new UserProfile(Application.Current.Properties["campus"].ToString(), Application.Current.Properties["username"].ToString(),
                    Application.Current.Properties["phone"].ToString(), Application.Current.Properties["email"].ToString(), Application.Current.Properties["userno"].ToString(),
                        Application.Current.Properties["role"].ToString()));
                    //
                }
            }
            catch (Exception) { }
            base.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.PopToRootAsync();
            return base.OnBackButtonPressed();
        }

    }
}
