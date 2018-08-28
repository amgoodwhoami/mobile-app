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
    public partial class WebsiteViewer : ContentPage
    {
        public WebsiteViewer(String Address)
        {
            InitializeComponent();
            try
            {
                var browser = new WebView
                {
                    Source = Address,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                this.Content = new StackLayout
                {
                    Children = {
                     browser
                }
                };
            }
            catch (Exception ex) { DisplayAlert("Error", ex.Message, "OK"); }

        }
    }
}
