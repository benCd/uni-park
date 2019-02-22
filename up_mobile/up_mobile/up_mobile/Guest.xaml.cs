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
    /// Guest page
    /// </summary>
    public partial class Guest : TabbedPage
    {
        RestService requests = new RestService();
        /// <summary>
        /// Loads the Guest page <see cref="Guest.xaml"/>
        /// </summary>
        public Guest()
        {
            InitializeComponent();
        }

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
                Debug.Write(ph.Pins[i].GetPrettyFormat());
            }
        }

        private async void Button2_ClickedAsync(object sender, EventArgs e)
        {
            await requests.PostPinAsync();
            Position p = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
            ParkingPin pin = new ParkingPin(p.Latitude, p.Longitude, p.Accuracy);

            //Debug.Write(DateTime.Now.ToString());
           // Debug.Write(DateTime.Now.ToString());
        }
    }
}