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
    /// the main class for consuming RESTful web services and generating HTTPS requests. 
    /// Contains a reference to one http client object through which all requests are 
    /// generated. 
    /// </summary>
    public class RestService
    {
        static CookieContainer cookies = new CookieContainer();
        static HttpClient client = new HttpClient();
        const string defaultBaseUri = "http://10.0.2.2:3000";

        public static RestService service = new RestService();

        //Constructors will be altered when this class is converted to static
        /*private RestService()
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            HttpClient client = new HttpClient(handler);
        }*/
        
        /*public RestService()
        {
            /*var username = Settings.Username;
            var password = Settings.Password;
            var auth = string.Format("{0}:{1}", username, password);
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));*/

            //client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;
            //for authenetication. still exploring other options
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        //}*/
        
    
        /// <summary>
        /// Registers a new user with the given email and password. Posts the fields to the server 
        /// and creates a new user to log in as provided that the request goes through successfully
        /// and no users already exist with the same email.
        /// </summary>
        /// <param name="email">new user email</param>
        /// <param name="password">new user password</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>A bool indicating if a new user was successfully created</returns>
        public static async Task<bool> RegisterUser(string in_email, string in_password, string serviceUri = "/register")
        {
            string json = JsonConvert.SerializeObject(new
            {
                email = in_email,
                password = in_password
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            //debugging
            Debug.Write(response.Content.ToString());
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Logs a user in given their email and password. Creates a new unique session and saves necessary
        /// session info inside a cookie which is added to the static httpclient instance of this class. All
        /// requests after this method is called contain this cookie, allowing access to user restricted routes
        /// </summary>
        /// <param name="email">user email login information</param>
        /// <param name="password">user password login information</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>bool indicating registration success</returns>
        /// 
        public static async Task<bool> LoginUser(string in_email, string in_password, string serviceUri = "/login")
        {
            //must create a new client with a handler and a blank cookiecontainer set to get cookies later
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);

            string json = JsonConvert.SerializeObject(new
            {
                email = in_email,
                password = in_password
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode){
                //get cookies
                IEnumerable<Cookie> resCookies = cookies.GetCookies(uri).Cast<Cookie>();
                Cookie myCookie = resCookies.First();
                //save cookie
                cookies.Add(myCookie);

                if (myCookie != null)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes the current user's session server side by making an http get request. Invalidates
        /// the client's current cookie, making routes which require authenticated users inaccesible.
        /// </summary>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns></returns>
        public static async Task LogoutUser(string serviceUri = "/logout")
        {
            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformGET(uri);
        }

        /// <summary>
        /// Returns a pinholder object containing all the gps pins for a given lot_id. Makes an http post
        /// request to the user with json contanining the lot_id passed into this function.
        /// </summary>
        /// <param name="lot_id">lot id for the lot that pins are wanted from</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>A pinholder object</returns>
        public async Task<PinHolder> GetLotPinsAsync(int in_lot_id, string serviceUri = "/lotpins") //MAKE STATIC!!
        {
            PinHolder ph = new PinHolder();

            string json = JsonConvert.SerializeObject(new
            {
                lot_id = in_lot_id
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                ph = JsonConvert.DeserializeObject<PinHolder>(rescontent);
            }

            return ph;
        }

        /// <summary>
        /// Posts a new gps pin to the database with the given gps data and lot id. Requires a logged in user
        /// to generate the associated user id for the new pin, will fail to post the new pin otherwise. 
        /// </summary>
        /// <param name="in_longitude">gps data input</param>
        /// <param name="in_latitude">gpa data pinput</param>
        /// <param name="in_lot_id">id of the lot the pin is in</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>a bool indicating the success of posting the new pin</returns>
        public static async Task<bool> PostNewPinAsync(double in_longitude, double in_latitude, int in_lot_id, string serviceUri = "/newpin")
        {
            string json = JsonConvert.SerializeObject(new
            {
                longitude = in_longitude,
                latitude = in_latitude,
                lot_id = in_lot_id
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
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
        public static async Task<ParkingLot> FindLot(double in_latitude, double in_longtitude, double in_accuracy, string serviceUri = "/findlot")
        {
            ParkingLot pl = null;

            string json = JsonConvert.SerializeObject(new
            {
                latitude = in_latitude,
                longitude = in_longtitude,
                accuracy = in_accuracy
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                pl = JsonConvert.DeserializeObject<ParkingLot>(rescontent);
            }

            return pl;
        }

        /// <summary>
        /// Returns a reportholder containing all the parking reports for a lot with a given id. Performs an
        /// http post and returns lotdata rows which match the lot id in the form of parkingreport objects
        /// contained inside an array on the reportholder object.
        /// </summary>
        /// <param name="in_lot_id">id of the lot where reports are wanted</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>a reportholder object</returns>
        public async Task<ReportHolder> GetLotInfo(int in_lot_id, string serviceUri = "/getlotinfo")
        {
            ReportHolder rh = null;

            string json = JsonConvert.SerializeObject(new
            {
                lot_id = in_lot_id
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                rh = JsonConvert.DeserializeObject<ReportHolder>(rescontent);
            }

            return rh;
        }

        /// <summary>
        /// Returns a university object associated with the university for a user with a given
        /// user id. Performs an http post request.
        /// </summary>
        /// <param name="in_user_id">id of the user where university information is wanted</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>a university object</returns>
        public async Task<University> GetUni(int in_user_id, string serviceUri = "/getuni")
        {
            University uni = null;

            string json = JsonConvert.SerializeObject(new
            {
                user_id = in_user_id
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                uni = JsonConvert.DeserializeObject<University>(rescontent);
            }

            return uni;
        }

        /// <summary>
        /// Returns a university object associated with the current user's university. Assumes a user is
        /// currently logged in and will fail otherwise. Performs an http get.
        /// </summary>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>a university object</returns>
        public async Task<University> GetMyUni(string serviceUri = "/myuni")
        {
            University uni = null;

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformGET(uri);

            if (response.IsSuccessStatusCode)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                uni = JsonConvert.DeserializeObject<University>(rescontent);
            }

            return uni;
        }

        /// <summary>
        /// Generic method for performing an http post.
        /// </summary>
        /// <param name="uri">uri to access</param>
        /// <param name="json">json content to be sent</param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> PerformPOST(Uri uri, string json)
        {
            HttpResponseMessage response = null;
            try
            {
                var reqcontent = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(uri, reqcontent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Generic method for performing an http get.
        /// </summary>
        /// <param name="uri">uri to access</param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> PerformGET(Uri uri)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Takes two strings and forms a uri object for later web requests. Helpful for treating
        /// the uri as nothing more than two strings which can be changed through this function.
        /// </summary>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <param name="baseUri">url of a web api to be accessed</param>
        /// <returns></returns>
        public static Uri makeUri(string serviceUri, string baseUri = defaultBaseUri)
        {
            baseUri += "{0}";
            return new Uri(string.Format(baseUri, serviceUri));
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
            ParkingPin pin = new ParkingPin(p.Latitude, p.Longitude, DateTime.Now);

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
