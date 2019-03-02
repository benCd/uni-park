using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Models;

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
        static CookieContainer cookies = new CookieContainer();
        static HttpClient client = new HttpClient();
        const string defaultBaseUri = "http://10.0.2.2:3000";

        //IGNORE CONSTRUCTOR, WILL BE REMOVED LATER
        public RestService()
        {
            /*var username = Settings.Username;
            var password = Settings.Password;
            var auth = string.Format("{0}:{1}", username, password);
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));*/

            //client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //for authenetication. still exploring other options
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        }

        /// <summary>
        /// Registers a new user with the given email and password. Posts the fields to the server 
        /// and creates a new user to log in as provided that the request goes through successfully
        /// and no users already exist with the same email.
        /// </summary>
        /// <param name="email">new user email</param>
        /// <param name="password">new user password</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <param name="baseUri">uri of main web api</param>
        /// <returns>A bool indicating if a new user was successfully created</returns>
        public static async Task<bool> RegisterUser(string email, string password, string serviceUri = "/register", string baseUri = defaultBaseUri)
        {
            baseUri += "{0}";
            var uri = new Uri(string.Format(baseUri, serviceUri));
            string json = string.Format("{{ \"email\" : \"{0}\", \"password\" : \"{1}\" }}", email, password);

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.Write(response.Content.ToString());
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Logs a user in given their email and password. Creates a new unique session and saves necessary
        /// session info inside a cookie which is added to the static httpclient instance of this class. All
        /// requests after this method is called contain this cookie, allowing access to user restricted routes
        /// </summary>
        /// <param name="email">user email login information</param>
        /// <param name="password">user password login information</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <param name="baseUri">uri of main web api</param>
        /// <returns>bool indicating registration success</returns>
        /// 
        public static async Task<bool> LoginUser(string email, string password, string serviceUri = "/login", string baseUri = defaultBaseUri)
        {
            //must create a new client with a handler and a blank cookiecontainer set to get cookies later
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);

            baseUri += "{0}";
            var uri = new Uri(string.Format(baseUri, serviceUri));
            string json = string.Format("{{ \"email\" : \"{0}\", \"password\" : \"{1}\" }}", email, password);

            //testing purposes only
            //Debug.Write(json);

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                //get cookies
                IEnumerable<Cookie> resCookies = cookies.GetCookies(uri).Cast<Cookie>();
                Cookie myCookie = resCookies.First();
                //save cookie
                cookies.Add(myCookie);

                //testing authentication
                //HttpResponseMessage response2 = await client.GetAsync(new Uri(defaultBaseUri + "/auth"));
                //Debug.Write(myCookie.ToString());

                if (myCookie != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Determines the current lot based on the latitude, longtitude, and accuracy of a given GPS measurement.
        /// All parameters are passed into a json string which is used to make an http post request to the server.
        /// </summary>
        /// <param name="latitude">gps latitude</param>
        /// <param name="longtitude">gps longtitude</param>
        /// <param name="accuracy">the radius of a circle indicating potential error in given GPS measurement.
        /// 0 == perfect accuracy.</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <param name="baseUri">uri of main web api</param>
        /// <returns>The parking lot object corresponding to the lot the user is in, or null if no lot was found</returns>
        public static async Task<ParkingLot> GetLotFromGPS(double latitude, double longtitude, double accuracy, string serviceUri = "/findlot", string baseUri = defaultBaseUri)
        {
            ParkingLot pl = null;
            baseUri += "{0}";
            var uri = new Uri(string.Format(baseUri, serviceUri));
            string json = string.Format("{{ \"latitude\" : \"{0}\", \"longtitude\" : \"{1}\"," +
                " \"accuracy\" : \"{1}\" }}", latitude, longtitude, accuracy);

            try
            {
                var reqcontent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, reqcontent);

                if (response.IsSuccessStatusCode)
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    pl = JsonConvert.DeserializeObject<ParkingLot>(rescontent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return pl;
        }

        /// <summary>
        /// Returns a pinholder object containing all the gps pins for a given lot_id. Makes an http post
        /// request to the user with json contanining the lot_id passed into this function.
        /// </summary>
        /// <param name="lot_id">lot id for the lot that pins are wanted from</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <param name="baseUri">uri of main web api</param>
        /// <returns>A pinholder object</returns>
        public async Task<PinHolder> GetLotPinsAsync(int lot_id, string serviceUri = "/lotpins", string baseUri = defaultBaseUri)
        {
            PinHolder ph = new PinHolder();
            baseUri += "{0}";
            var uri = new Uri(string.Format(baseUri, serviceUri));
            string json = string.Format("{{ \"lot_id\" : \"{0}\"}}", lot_id);

            try
            {
                var reqcontent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, reqcontent);

                if (response.IsSuccessStatusCode)
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    Debug.Write("HERE IS THE RESPONSE" + rescontent);
                    ph = JsonConvert.DeserializeObject<PinHolder>(rescontent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return ph;
        }

        //NOTE: the rest of the methods may not be functional, they are preexisting testing methods
        //the rest will most likely be refactored or removed

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
                Debug.WriteLine(ex.Message);
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
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
