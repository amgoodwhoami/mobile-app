﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iuiuapplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMaster : MasterDetailPage
    {
        public MainMaster()
        {
            InitializeComponent();
            Detail = new NavigationPage(new MainPage());
            
        }
    }
}
