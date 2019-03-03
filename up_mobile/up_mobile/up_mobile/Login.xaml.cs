using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    /// <summary>
    /// Login page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        /// <summary>
        /// Loads the Login page <see cref="Login.xaml"/>
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When Login button on the Login page <see cref="Login.xaml"/> is pressed
        /// it attempts to log the user in. If it is their first time logging in they 
        /// are redirected to the New User Survey <see cref="NewUserSurvey.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LoginAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            /// <remarks>
            /// Storing these values in <see cref="Settings.cs"/>
            /// </remarks>
            Helpers.Settings.Username = LoginEmail.Text;
            Helpers.Settings.Password = LoginPassword.Text;
            Helpers.Settings.IsLoggedIn = true;

            if (Helpers.Settings.TookNewUserSurvey == false)
            {
                await Navigation.PushAsync(new NewUserSurvey());
            }

            else if (Helpers.Settings.TookNewUserSurvey == true)
            {
                await Navigation.PushAsync(new User());
            }
        }
    }
}