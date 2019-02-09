using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace up_mobile
{
    /// <summary>
    /// MainPage page
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Loads the MainPage page <see cref="MainPage.xaml"/>
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the Log In button in <see cref="MainPage.xaml"/> is pressed, navigates to the Log In page <see cref="Login.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LoginPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Login());
        }

        /// <summary>
        /// When the Register button in <see cref="MainPage.xaml"/> is pressed, navigates to the Register page <see cref="Register.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void RegisterPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Register());
        }

    }
}