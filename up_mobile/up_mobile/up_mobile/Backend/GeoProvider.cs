using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
//old plugin using statements
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

        public static async Task<Location> GetCurrentLocationAsync()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }

            return location; 
        }
    }
}
