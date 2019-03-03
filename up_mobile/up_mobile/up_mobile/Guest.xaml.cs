using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using up_mobile.Models;
using up_mobile.Helpers;
using Xamarin.Forms;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;
using Newtonsoft.Json;
using System.Diagnostics;

namespace up_mobile
{
    /// <summary>
    /// Guest page - Landing page for non logged in users
    /// </summary>
    public partial class Guest : ContentPage
    {
        RestService requests = new RestService();

        /// <summary>
        /// Loads the Guest page <see cref="Guest.xaml"/>
        /// </summary>
        public Guest()
        {
            InitializeComponent();

            // Suppresses the back button at the top of the page
            NavigationPage.SetHasBackButton(this, false);
        }

        /// <summary>
        /// When the Log In button in <see cref="Guest.xaml"/> is pressed
        /// navigates to the Log In page <see cref="Login.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LoginPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Login());
        }

        /// <summary>
        /// When the Register button in <see cref="Guest.xaml"/> is pressed
        /// navigates to the Register page <see cref="Register.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void RegisterPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Register());
        }

        /// <summary>
        /// Override for OnBackButtonPressed - Preventing Android Hardware Back Button from 
        /// going back to User page <see cref="User.xaml"/> after logging out
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return true;
        }


        //DELETE ALL THIS LATER VVVV
        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            /*Position p = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
            ParkingPin pp = new ParkingPin(p.Latitude, p.Longitude, p.Accuracy);
            Console.WriteLine(pp.Time.ToString());
            Console.Write(JsonConvert.SerializeObject(pp).ToString());*/
            PinHolder ph = await requests.GetPinsAsync();

            Debug.Write(ph.Pins.ToString());
            for (int i = 0; i < ph.Pins.Length; i += 1)
            {
                //Debug.Write(ph.Pins[i].GetPrettyFormat());
            }
        }

        private async void Button2_ClickedAsync(object sender, EventArgs e)
        {
            await requests.PostPinAsync();
            Position p = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
            ParkingPin pin = new ParkingPin(p.Latitude, p.Longitude, DateTime.Now);

            //Debug.Write(DateTime.Now.ToString());
           // Debug.Write(DateTime.Now.ToString());
        }
    }
}