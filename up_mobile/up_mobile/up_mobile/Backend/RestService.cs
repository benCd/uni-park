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

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        }

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

        /// <summary>
        /// Generic method for getting a dictionary representation of an
        /// object from a web service using a url and object id. Uses HTTP GET.
        /// Check project settings for both android/iOS to ensure current 
        /// http client implementation is capable of supporting TLS 1.2+
        /// </summary>
        /// <param name="id">id of object to be acquired</param>
        /// <param name="url">url of web API</param>
        /// <returns>dictionary of varnames and values representing an object</returns>
        public async Task<Dictionary<string, object>> GetItemValuesAsync(string id = "", string url = defaultBaseUri)
        {
            Dictionary<string, object> itemValues = new Dictionary<string, object>();

            url += "{0}";
            var uri = new Uri(string.Format(url, id));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    itemValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR {0}", ex.Message);
            }

            return itemValues;
        }

        /// <summary>
        /// Generic method for sending a single object as its vars in
        /// json. Uses HTTP POST, or HTTP PUT in case of existing objects.
        /// Check project settings for both android/iOS to ensure current 
        /// http client implementation is capable of supporting TLS 1.2+
        /// </summary>
        /// <param name="item">object to be sent. only required param</param>
        /// <param name="isNewItem">bool determining if object is new. If no, 
        /// use PUT. If yes, use POST.</param>
        /// <param name="url">url of web API</param>       
        /// <returns>true if object was sent successfully, false if not</returns>
        public async Task<bool> PostItemAsync(object item, string uri = defaultBaseUri)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
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

        /// <summary>
        /// Simple method for sending current position via HTTP POST
        /// </summary>
        /// <returns>bool indicating success/failure of HTTP POST</returns>
        //public async Task<bool> PostCurrentPositionAsync()
        //{
            //Position position = await GeoProvider.GetCurrentPositionAsync();
            //return await PostItemAsync(position);
        //}
    }
}
