using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static async Task<List<Map.Utils.ParkingPin>> GetPinsFor(int LotId)
        {
            Debug.Write("Entering PinFactory!");

            var RawPins = await RestService.service.GetLotPinsAsync(LotId);

            var Pins = new List<Map.Utils.ParkingPin>();

            if(RawPins != null)
                foreach(ParkingPin Pin in RawPins.Pins)
                {
                    Debug.Write("Adding new pin!");

                    Pins.Add(new Map.Utils.ParkingPin()
                    {
                        Position    = new Position(Pin.Latitude, Pin.Longitude),
                        Label       = Pin.Timestamp.ToLongDateString(),
                        Volume      = (int)(Pin.Volume * 100.00),
                        ID          = Pin.Id,
                        Downvotes   = Pin.Downvotes,
                        Upvotes     = Pin.Upvotes,
                        OwnerId     = Pin.User_id
                    });
                }
            Debug.Write("Exiting PinFactory!");
            return Pins;
        }
    }
}
