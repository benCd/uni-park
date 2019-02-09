using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace up_mobile
{
    /// <summary>
    /// the main class for consuming RESTful web services
    /// and generating HTTP requests. Contains a reference to 
    /// one http client object through which all requests are 
    /// generated. defaultUrl is used to refer to the url of the 
    /// main web API, so methods do not have to explicity state 
    /// a url unless they require a different API
    /// </summary>
    class RestRequester
    {
        HttpClient client;
        const string defaultUrl = "https://";

        public RestRequester()
        {
            var auth = string.Format("{0}:{1}", "Username", "Password");
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
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
        public async Task<Dictionary<string, object>> GetItemValuesAsync(string id = "", string url = defaultUrl)
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
        /// <param name="id">id of object to be sent</param>/// 
        /// <param name="url">url of web API</param>       
        /// <returns>true if object was sent successfully, false if not</returns>
        public async Task<bool> PostItemAsync(object item, bool isNewItem = true, 
            string id = "", string url = defaultUrl)
        {
            url += "{0}";
            var uri = new Uri(string.Format(url, id));

            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }

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
