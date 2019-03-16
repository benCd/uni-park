using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace up_mobile.Backend
{
    public class GeoProvider
    {
        public static async Task<Position> GetCurrentPositionAsync()
        {
            return await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
        }
    }
}
