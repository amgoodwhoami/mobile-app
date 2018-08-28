//using DLToolkit.Forms.Controls;
using DLToolkit.Forms.Controls;
using iuiuapplication.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace iuiuapplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            FlowListView.Init();
            //MainPage = new  MainMaster();
            MainPage = new MasterDetailPage()
            {
                Master = new NavigationPage(new Views.NavSlider()) { Title = "Main Page" },
                Detail = new NavigationPage(new MainPage())
            };


        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
