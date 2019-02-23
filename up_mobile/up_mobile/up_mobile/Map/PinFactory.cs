using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using up_mobile.Models;
using Xamarin.Forms.Maps;

namespace up_mobile.Map
{
    class PinFactory
    {
        public static async Task<List<Pin>> GetPinsFor(int LotId)
        {
            var Rest = new RestService();

            var RawPins = await Rest.GetPinsAsync();

            var Pins = new List<Pin>();

            if(RawPins != null)
                foreach(ParkingPin Pin in RawPins.Pins)
                {
                    Pins.Add(new Pin()
                    {
                        Position = new Position(Pin.Latitude, Pin.Longtitude),
                        Label = Pin.User + "\n@" + Pin.Date + "\n" + Pin.Percentage
                    });
                }
            return Pins;
        }
    }
}
