using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;
using up_mobile.Helpers;
using System.Net;
using up_mobile.Models;
using Plugin.Geolocator;

namespace up_mobile.Backend
{
    /// <summary>
    /// the main class for consuming RESTful web services
    /// and generating HTTPS requests. Contains a reference to 
    /// one http client object through which all requests are 
    /// generated. DefaultBaseUri is used to refer to the url of the 
    /// main web API, so methods do not have to explicity state 
    /// the full url unless they require a different API
    /// </summary>
    public class RestService
    {
        HttpClient client;
        const string defaultBaseUri = "http://10.0.2.2:3000";

        public RestService()
        {
            var username = Settings.Username;
            var password = Settings.Password;
            var auth = string.Format("{0}:{1}", username, password);
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //for authenetication. still exploring other options
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        }

        /// <summary>
        /// Method for getting all parking pins from a mysql database using a baseUri
        /// and a serviceUri to form a full url which points to a web api. Generates an 
        /// HTTP GET request and parses parking pin object from JSON sent by the web api.
        /// Check project settings for both android/iOS to ensure current 
        /// http client implementation is capable of supporting TLS 1.2+
        /// </summary>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <param name="baseUri">uri of main web api</param>
        /// <returns>A pinholder object containing an array of parking pins</returns>
        public async Task<PinHolder> GetPinsAsync(string serviceUri = "/pins" , string baseUri = defaultBaseUri)
        {
            PinHolder ph = new PinHolder();
            baseUri += "{0}";
            var uri = new Uri(string.Format(baseUri, serviceUri));
            Debug.Write(uri);

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.Write("HERE IS THE RESPONSE" + content);
                    ph = JsonConvert.DeserializeObject<PinHolder>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR {0}", ex.Message);
            }

            return ph;
        }

        /// <summary>
        /// Method for posting a parking pin to a mysql database given a baseUri
        /// and a serviceUri which together form the url of a web api. Generates 
        /// A HTTP POST request after using the geolocator plugin to create a new
        /// parking pin instance, which is translated to json before being sent.
        /// Check project settings for both android/iOS to ensure current 
        /// http client implementation is capable of supporting TLS 1.2+
        /// </summary>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <param name="baseUri">uri of main web api</param>
        /// <returns>a bool indicating success of the POST</returns>
        public async Task<bool> PostPinAsync(string serviceUri = "/pins", string baseUri = defaultBaseUri)
        {
            Position p = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
            ParkingPin pin = new ParkingPin(p.Latitude, p.Longitude, p.Accuracy);

            baseUri += "{0}";
            var uri = new Uri(string.Format(baseUri, serviceUri)); 

            try
            {
                var json = JsonConvert.SerializeObject(pin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR {0}", ex.Message);
                return false;
            }
        }
    }
}
