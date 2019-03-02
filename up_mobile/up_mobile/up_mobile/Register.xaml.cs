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
        /// When the Register button on the Register page <see cref="Register.xaml"/> is clicked it
        /// checks that the email and password entered meet requirements for registration, then registers
        /// the user, sends them a confirmation email, and then navigates to the Login page <see cref="Login.xaml"/>
        /// 
        /// FOR NOW IT JUST GOES TO THE LOGIN PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void RegisterAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            var RegEmail = RegisterEmail.Text;
            var RegPassword = RegisterPassword.Text;

            //to test getting lot from gps... 
            /*
            Position p = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
            ParkingLot pl = await RestService.GetLotFromGPS(p.Latitude, p.Longitude);
            Debug.WriteLine(pl.GetPrettyFormat());*/

            var created = await RestService.RegisterUser(RegEmail, RegPassword); 

            if (created == true)
                await Navigation.PushAsync(new Login());
            else
                await DisplayAlert("Email Already Registered", "Please enter a different email", "OK");
        }
    }
}