using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using up_mobile.Helpers;
using up_mobile.Models;
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

            var loginSuccess = RestService.service.LoginUser(LoginEmail.Text, LoginPassword.Text);

            if (await loginSuccess)
            {
                Helpers.Settings.IsLoggedIn = true;
                var surveyTaken = RestService.service.SeeSurveyStatus();

                //new stuff for permissions
                var permissionGranted = await Permissions.RequestStoragePermission(this);
                if (!permissionGranted)
                    return;
                //

                Helpers.Settings.TookNewUserSurvey = await surveyTaken;

                if (await surveyTaken)
                    await Navigation.PushAsync(new User());
                else
                    await Navigation.PushAsync(new NewUserSurvey());
            } else {
                await DisplayAlert("Incorrect email and password combination", "Please try again", "OK");
            }
        }
    }
}