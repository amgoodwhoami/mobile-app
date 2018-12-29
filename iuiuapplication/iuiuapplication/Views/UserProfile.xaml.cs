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
                img_profilepic.Source = UriImageSource.FromUri(new Uri(Libraries.MobileConfig.MainCampusStaffPhotolink + Application.Current.Properties["userno"] + "_photo.jpg"));

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
                    new MenuItemModel("thumb_notifications.png","Announcements"),
                    new MenuItemModel("thumb_results.png","My Results"),
                    new MenuItemModel("thumb_register.png","My Registration"),
                    new MenuItemModel("thumb_feespay.png","Fees Payments"),
                    new MenuItemModel("thumb_timetables.png","My Timetables"),
                    new MenuItemModel("thumb_attendance.png","My Attendance"),
                    new MenuItemModel("thumb_passout.png","My Passouts"),
                    new MenuItemModel("thumb_coursematerial.png","Course Materials"),
                    new MenuItemModel("thumb_directory.png","Campus Directory"),
                    new MenuItemModel("thumb_mob_opac.png","Library Centre")
            };
            }
            else
            {
                myList.FlowItemsSource = new List<MenuItemModel>
            {
                new MenuItemModel("thumb_notifications.png","Announcements"),
                new MenuItemModel("thumb_feespay.png","Salary Info"),
                new MenuItemModel("thumb_timetables.png","My Timetables"),
                new MenuItemModel("thumb_results.png","Results Centre"),
                new MenuItemModel("thumb_coursematerial.png","Course Materials"),
                new MenuItemModel("thumb_directory.png","Campus Directory"),
                new MenuItemModel("thumb_financials.png","My Claims"),
                new MenuItemModel("thumb_mob_opac.png","Library Centre")
            };

            }
            myList.FlowUseAbsoluteLayoutInternally = true;
            myList.FlowColumnCount = 2;
            myList.FlowRowBackgroundColor = Color.Transparent;
            myList.FlowColumnExpand = FlowColumnExpand.Proportional;

        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Login("Back"));
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

        private async void FlowListView_OnFlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            //DisplayAlert("Tapped", "Item Tapped"+ .ToString(), "ok");
            MenuItemModel Item = (MenuItemModel)e.Item;
            if (Item.menu_label == "My Results")
            {
               await Navigation.PushAsync(new StudentResultCentre(campus_value));
            }
            else if (Item.menu_label == "Salary Info")
            {
                await Navigation.PushAsync(new SalarySummary());
            }
            else if (Item.menu_label == "Fees Payments")
            {
                await Navigation.PushAsync(new FeesPaymentInfo());
            }
            else if (Item.menu_label == "My Registration")
            {
                await Navigation.PushAsync(new RegistrationHistory());
            }
            else if (Item.menu_label == "My Timetables")
            {
                await Navigation.PushAsync(new TimeTables());
            }
            else if (Item.menu_label == "Library Centre")
            {
                var official = await DisplayActionSheet("IUIU Mobile", "Cancel", "", "View Profile", "Search Catalog");
                if (official == "View Profile")
                {
                    await Navigation.PushAsync(new LibraryOPAC());
                }
                else if (official == "Search Catalog")
                {
                    await Navigation.PushAsync(new LibraryOPAC());
                }
                
            }
            else if (Item.menu_label == "Campus Directory")
            {
                await Navigation.PushAsync(new CampusDirectory());
            }
            else if (Item.menu_label == "My Claims")
            {
                await Navigation.PushAsync(new ClaimingCentre());
            }
            else if (Item.menu_label == "Results Centre")
            {
                await Navigation.PushAsync(new TeachingAllocation());
            }
            else if (Item.menu_label == "Course Materials")
            {
                await Navigation.PushAsync(new CourseContent());
            }
            else if (Item.menu_label == "My Passouts")
            {
                await Navigation.PushAsync(new StudentPassouts());
            }
            else if (Item.menu_label == "Announcements")
            {
                await Navigation.PushAsync(new AnnouncementList());
            }
            else
            {
                await DisplayAlert("IUIU Mobile","Coming in next Version","OK");
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
                img_profilepic.Source = UriImageSource.FromUri(new Uri(Libraries.MobileConfig.MainCampusStaffPhotolink + Application.Current.Properties["userno"] + "_photo.jpg"));

            }
            base.OnAppearing();
        }
    }
}
