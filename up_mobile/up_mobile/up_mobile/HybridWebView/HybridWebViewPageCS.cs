
using System;
using Xamarin.Forms;

namespace up_mobile
{
    public class HybridWebViewPageCS : ContentPage
    {
        public HybridWebViewPageCS()
        {
            var CalendarAuthWebView = new HybridWebView()
            {
                Uri = "https://www.google.com",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            CalendarAuthWebView.RegisterAction(data => DisplayAlert("Alert", "Hello " + data, "OK"));

            Padding = new Thickness(0, 20, 0, 0);
            Content = CalendarAuthWebView;
        }
    }
}