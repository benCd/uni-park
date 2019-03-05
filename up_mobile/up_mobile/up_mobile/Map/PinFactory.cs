using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
//using up_mobile.Map.Utils;
using up_mobile.Models;
using Xamarin.Forms.Maps;

namespace up_mobile
{
    /// <summary>
    /// Factory to create <see cref="Pin"/> from <see cref="ParkingPin"/>
    /// objects retrieved from the server.
    /// All implemented methods must be static!
    /// </summary>
    class PinFactory
    {
        /// <summary>
        /// Factory method, which asynchronously retrieves the information for <see cref="Pin"/> objects.
        /// 
        /// Please note to use <code>await</code> when calling this method in order to ensure proper data
        /// retrieval!
        /// </summary>
        /// <param name="LotId">ID for the parking lot, whose pins are supposed to be retrieved</param>
        /// <returns></returns>
        public static async Task<List<Pin>> GetPinsFor(int LotId)
        {
            var RawPins = await RestService.service.GetLotPinsAsync(LotId);

            var Pins = new List<Pin>();

            if(RawPins != null)
                foreach(ParkingPin Pin in RawPins.Pins)
                {
                    Pins.Add(new Pin()
                    {
                        Position = new Position(Pin.Latitude, Pin.Longitude),
                        Label = Pin.User_id + "\n@" + Pin.Timestamp + "\n" + Pin.Percentage
                    });
                }
            return Pins;
        }
    }
}
