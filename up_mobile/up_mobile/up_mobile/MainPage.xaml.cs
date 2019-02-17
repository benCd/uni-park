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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            /*Position p = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
            ParkingPin pp = new ParkingPin(p.Latitude, p.Longitude, p.Accuracy);
            Console.WriteLine(pp.Time.ToString());
            Console.Write(JsonConvert.SerializeObject(pp).ToString());*/
            RestService requests = new RestService();
            PinHolder ph = await requests.GetPinsAsync();

            Debug.Write(ph.Pins.ToString());
            Debug.Write(ph.Pins[0].getAsString());
            Debug.Write(ph.Pins[1].getAsString());
        }
    }
}
