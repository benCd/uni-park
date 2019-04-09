using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleCalendarAuth : ContentPage
    {
        /// <summary>
        /// GoogleCalenderAuth page constructor - needs to be passed the desired URL as a String when it is called for navigation
        /// </summary>
        /// <param name="URL"></param>
        public GoogleCalendarAuth(string URL)
        {
            InitializeComponent();

            // Creates a WebView
            var CalendarAuthWebView = new WebView();
            CalendarAuthWebView.HorizontalOptions = LayoutOptions.FillAndExpand;
            CalendarAuthWebView.VerticalOptions = LayoutOptions.FillAndExpand;

            // Creates a Stack Layout for the WebView
            var WebViewStackLayout = new StackLayout();
            this.Content = WebViewStackLayout;
            WebViewStackLayout.Children.Add(CalendarAuthWebView);
            WebViewStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            WebViewStackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

            // Sets the URL of the WebView
            CalendarAuthWebView.Source = URL;

            // Suppresses the back button at the top of the page
            NavigationPage.SetHasBackButton(this, false);

            // Navigates to the User page when the WebView reaches a specific url
            CalendarAuthWebView.Navigating += (object sender, WebNavigatingEventArgs e) =>
            {
                var url = e.Url;
                if (url == "https://unipark.space/redirect")
                {
                    //System.Diagnostics.Debug.WriteLine("Adam's WebView debug statement worked");
                    Navigation.PushAsync(new User());
                }
            };
        }

        /// <summary>
        /// Override for OnBackButtonPressed - Preventing Android Hardware Back Button from 
        /// going back to Login page <see cref="Login.xaml"/> after logging in
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}