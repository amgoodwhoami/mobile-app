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
        Label ai = new Label();
        public WebsiteViewer(String Address)
        {
            InitializeComponent();
            var webView = new WebView
            {
                Source = Address,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,

            };



            // toolbar
            ToolbarItems.Add(new ToolbarItem("< Back", null, () =>
            {
                webView.GoBack();
            }));


            ai.TextColor = Xamarin.Forms.Color.Green;
            ai.Text = "Loading...Please wait";
            ai.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;
            ai.VerticalTextAlignment = Xamarin.Forms.TextAlignment.Center;
            ai.HeightRequest = 50;

            webView.Navigated += WebView_Navigated;
            webView.Navigating += WebView_Navigating;

            Content = new StackLayout
            {

                Children = { ai, webView }
            };

        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            ai.IsVisible = true;
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            ai.IsVisible = false;
            //throw new NotImplementedException();
        }
    }
}
