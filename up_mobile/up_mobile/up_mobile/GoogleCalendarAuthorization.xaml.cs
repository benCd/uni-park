using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleCalenderAuthorization : ContentPage
    {
        /// <summary>
        /// GoogleCalenderAuthorization page constructor - needs to be passed the desired URL as a String when it is called for navigation
        /// </summary>
        /// <param name="URL"></param>
        public GoogleCalenderAuthorization(string URL)
        {
            InitializeComponent();

            // Suppresses the back button at the top of the page
            NavigationPage.SetHasBackButton(this, false);
            //CalendarAuthWebView.
            CalendarAuthWebView.Source = URL;
            CalendarAuthWebView.Source.PropertyChanged += WebViewChanged;
            //CalendarAuthWebView.RegisterAction(data => DisplayAlert("Alert", "Hello " + data, "OK"));
        }

        private void WebViewChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.Write(e.PropertyName.ToString());
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