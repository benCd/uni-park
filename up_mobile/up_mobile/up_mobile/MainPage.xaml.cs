using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace up_mobile
{
    public partial class MainPage : ContentPage
    {
        // Constructor for MainPage class - Initializes the page on load
        public MainPage()
        {
            InitializeComponent();
        }

        // LoginPage function - When the Log In button in MainPage.xaml is clicked this function navigates the user to the Log In page
        async void LoginPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Login());
        }

        // RegisterPage function - When the Register button in MainPage.xaml is clicked this function navigates the user to the Register page
        async void RegisterPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Register());
        }

    }
}