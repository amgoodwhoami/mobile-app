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
    public partial class NavSlider : ContentPage
    {
        public NavSlider()
        {
            InitializeComponent();
            MenuPopulator();
        }

        void MenuPopulator()
        {
            List<Model.MenuItemModel> nav_menu = new List<Model.MenuItemModel>().ToList();
            //Model.MenuItemModel item_1 = new Model.MenuItemModel("home.png", "Home");
            //nav_menu.Add(item_1);
            Model.MenuItemModel item_2 = new Model.MenuItemModel( "login.png", "Academica ERP");
            nav_menu.Add(item_2);
            //Model.MenuItemModel item_3 = new Model.MenuItemModel("restaurants.png", "Restaurants");
            //nav_menu.Add(item_3);
            Model.MenuItemModel item_4 = new Model.MenuItemModel("library.png", "Library Catalog");
            nav_menu.Add(item_4);
            Model.MenuItemModel item_5 = new Model.MenuItemModel("logout.png", "Logout");
            nav_menu.Add(item_5);
            lv_navmenu.ItemsSource = nav_menu;
        }

        protected override void OnAppearing()
        {
            try
            {
                txt_nav_username.Text = Application.Current.Properties["username"].ToString().ToUpper();
                if (Application.Current.Properties["role"].ToString() == "Student")
                {
                    try
                    {
                        if (Application.Current.Properties["userno"].ToString().Length > 1)
                        {

                            img_nav_photo.Source = UriImageSource.FromUri(new Uri(Libraries.MobileConfig.GetPhotoAddress(Application.Current.Properties["campus"].ToString()) +
                                Application.Current.Properties["userno"] + "_photo.jpg"));
                        }
                        else
                        {
                            img_nav_photo.Source = "no_photo.png";
                        }
                    }
                    catch (Exception)
                    {

                        img_nav_photo.Source = "no_photo.png";
                    }
                }
                else
                {
                    img_nav_photo.Source = "no_photo.png";
                }
            }
            catch (Exception) { }
            base.OnAppearing();
        }

        private  async void lv_navmenu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Model.MenuItemModel item = (Model.MenuItemModel)e.Item;
            if(item.menu_label== "Academica ERP")
            { 
                await ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new Login("New"));
                (App.Current.MainPage as MasterDetailPage).IsPresented = false;
            }
            else if (item.menu_label == "Library Catalog")
            {

                await ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new LibraryOPAC());
                (App.Current.MainPage as MasterDetailPage).IsPresented = false;

            }
            else if (item.menu_label == "Logout")
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
                }
                await ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new MainPage());
                (App.Current.MainPage as MasterDetailPage).IsPresented = false;


            }
        }
    }
}
