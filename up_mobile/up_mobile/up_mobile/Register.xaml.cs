using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using up_mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    /// <summary>
    /// Register page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        /// <summary>
        /// Loads the Register page <see cref="Register.xaml"/>
        /// </summary>
        public Register()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the Register button on the Register page <see cref="Register.xaml"/> 
        /// is clicked it checks that the email and password entered meet requirements 
        /// for registration, then registers the user, sends them a confirmation email, 
        /// and then navigates to the Login page <see cref="Login.xaml"/>
        /// 
        /// FOR NOW IT JUST GOES TO THE LOGIN PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void RegisterAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            // How to get the information from those fields to Register with
            // RegisterEmail.Text;
            // RegisterPassword.Text;

            Helpers.Settings.Username  = RegisterEmail.Text;
            Helpers.Settings.Password = RegisterPassword.Text;

            var regStatus = await RestService.service.RegisterUser(RegisterEmail.Text, RegisterPassword.Text);

            if (regStatus == "Success")
            {
                await Navigation.PushAsync(new Login());
            }
            else
            {
                await DisplayAlert(regStatus, "Please try again", "OK");
            }
        }
    }
}