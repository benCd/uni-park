using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using up_mobile.Data_Sending;

namespace up_mobile.GPS
{
    public class GeoService
    {
        RestRequester rest;

        /// <summary>
        /// method for getting the current gps position
        /// and sending it via http post
        /// </summary>
        /// <returns>a bool indicating the http post succeded (true) or failed (false)</returns>
        async Task<bool> SendGPSAsync()
        {
            Task<Position> GPSTask = CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            Position position = await GPSTask;

            return await rest.PostItemAsync(position);
        }
    }
}
