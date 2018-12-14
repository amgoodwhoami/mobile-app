using DLToolkit.Forms.Controls;
using iuiuapplication.Model;
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
    public partial class UserProfile : ContentPage
    {
        string campus_value, regno_value, role_value,phone_value,email_value,users_name_value;
        public UserProfile(string campus, string users_name, string phone, string email, string userno, string userrole)
        {
            InitializeComponent();

            if (userrole == "Student")
            {
                img_profilepic.Source = "no_photo.png";
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
            phone_value = phone;
            email_value = email;
            users_name_value = users_name;

            txt_profile_username.Text = users_name.ToUpper();
            txt_profile_userno.Text = regno_value;
            txt_profile_phone.Text = phone;
            txt_profile_email.Text = email;

            if (userrole == "Student")
            {
                myList.FlowItemsSource = new List<MenuItemModel>
            {
                    new MenuItemModel("thumb_notifications.png","Notifications"),
                    new MenuItemModel("thumb_results.png","My Results"),
                    new MenuItemModel("thumb_register.png","My Registration"),
                    new MenuItemModel("thumb_feespay.png","Fees Payments"),
                    new MenuItemModel("thumb_timetables.png","My Timetables"),
                    new MenuItemModel("thumb_coursematerial.png","Course Materials"),
                    new MenuItemModel("thumb_directory.png","Campus Directory"),
                    new MenuItemModel("thumb_mob_opac.png","Library Catalog")
            };
            }
            else
            {
                myList.FlowItemsSource = new List<MenuItemModel>
            {
                new MenuItemModel("thumb_notifications.png","Notifications"),
                new MenuItemModel("thumb_feespay.png","Salary Info"),
                new MenuItemModel("thumb_timetables.png","My Timetables"),
                new MenuItemModel("thumb_results.png","Results Centre"),
                new MenuItemModel("thumb_coursematerial.png","Course Materials"),
                new MenuItemModel("thumb_directory.png","Campus Directory"),
                new MenuItemModel("thumb_financials.png","My Claims"),
                new MenuItemModel("thumb_mob_opac.png","Library Catalog")
            };

            }
            myList.FlowUseAbsoluteLayoutInternally = true;
            myList.FlowColumnCount = 2;
            myList.FlowRowBackgroundColor = Color.Transparent;
            myList.FlowColumnExpand = FlowColumnExpand.Proportional;

        }

        protected override bool OnBackButtonPressed()
        {
           // Navigation.PopToRootAsync();
            return base.OnBackButtonPressed();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new StudentResultCentre(campus_value));
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

        private void FlowListView_OnFlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            //DisplayAlert("Tapped", "Item Tapped"+ .ToString(), "ok");
            MenuItemModel Item = (MenuItemModel)e.Item;
            if (Item.menu_label == "My Results")
            {
               Navigation.PushAsync(new StudentResultCentre(campus_value));
            }
            else if (Item.menu_label == "Salary Info")
            {
               Navigation.PushAsync(new SalarySummary());
            }
            else if (Item.menu_label == "Fees Payments")
            {
              Navigation.PushAsync(new FeesPaymentInfo());
            }
            else if (Item.menu_label == "My Registration")
            {
                Navigation.PushAsync(new RegistrationHistory());
            }
            else if (Item.menu_label == "My Timetables")
            {
                Navigation.PushAsync(new TimeTables());
            }
            else if (Item.menu_label == "Library Catalog")
            {
                Navigation.PushAsync(new LibraryOPAC());
            }
            else if (Item.menu_label == "Campus Directory")
            {
                Navigation.PushAsync(new CampusDirectory());
            }
            else if (Item.menu_label == "My Claims")
            {
                Navigation.PushAsync(new ClaimingCentre());
            }
             else if (Item.menu_label == "Results Centre")
            {
                Navigation.PushAsync(new TeachingAllocation());
            }
            else
            {
                DisplayAlert("IUIU Mobile","Coming in next Version","OK");
            }

        }

        private void GetFullProfile(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FullProfile(campus_value,users_name_value,phone_value,email_value,regno_value,role_value));
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
