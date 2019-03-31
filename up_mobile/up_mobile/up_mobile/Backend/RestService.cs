using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Models;
using Xamarin.Forms;

namespace up_mobile.Backend
{
    /// <summary>
    /// the main class for consuming RESTful web services and generating HTTPS requests. 
    /// Contains a reference to one http client object through which all requests are 
    /// generated. 
    /// </summary>
    public class RestService
    {
        public CookieContainer cookies { set; get; }
        static HttpClient client = new HttpClient();
        const string defaultBaseUri = "http://35.207.3.28:8080"; 

        public static RestService service = new RestService();

        private RestService()
        {
            if (Application.Current.Properties.ContainsKey("Cookies") && Application.Current.Properties["Cookies"] != null)
                cookies = (CookieContainer)Application.Current.Properties["Cookies"];
            else if(!Application.Current.Properties.ContainsKey("Cookies"))
            {
                cookies = new CookieContainer();
                Application.Current.Properties.Add("Cookies", cookies);
            }
            else if (Application.Current.Properties["Cookies"] == null)
            {
                cookies = new CookieContainer();
                Application.Current.Properties["Cookies"] = cookies;
            }

            //DEBUG ------------------------------------------------
            foreach (Cookie c in ((CookieContainer)Application.Current.Properties["Cookies"]).GetCookies(new Uri(defaultBaseUri)))
                Debug.Write(c.Name + " -> " + c.Value);
            //!DEBUG ------------------------------------------------

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
        }

        /// <summary>
        /// Registers a new user with the given email and password. Posts the fields to the server 
        /// and creates a new user to log in as provided that the request goes through successfully
        /// and no users already exist with the same email.
        /// </summary>
        /// <param name="email">new user email</param>
        /// <param name="password">new user password</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>A bool indicating if a new user was successfully created</returns>
        public async Task<string> RegisterUser(string in_email, string in_password, string serviceUri = "/register")
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
                return "Success";
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return "Not a valid university email";
            if (response.StatusCode == HttpStatusCode.Conflict)
                return "Email is already registered";

            return "error";
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
        public async Task<bool> LoginUser(string in_email, string in_password, string serviceUri = "/login")
        {
            Cookie myCookie = null;

            string json = JsonConvert.SerializeObject(new
            {
                email = in_email,
                password = in_password
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode) {
                //get cookies
                IEnumerable<Cookie> resCookies = cookies.GetCookies(uri).Cast<Cookie>();
                //save cookie
                if (resCookies.Any()) {
                    myCookie = resCookies.First();
                    cookies.Add(myCookie);
                }

                Debug.Write("Cookie:");
                //Debug.Write(myCookie.Value);
                /*foreach (Cookie c in ((CookieContainer)Application.Current.Properties["Cookies"]).GetCookies(new Uri(defaultBaseUri)))
                    Debug.Write(c.Name + " -> " + c.Value);
                */
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
        public async Task LogoutUser(string serviceUri = "/logout")
        {
            //DEBUG ------------------------------------------------
            foreach (Cookie c in cookies.GetCookies(new Uri(defaultBaseUri)))
                Debug.Write(c.Name + " + " + c.Value);
            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformGET(uri);
            //cookies = null; //remove cookies
        }

        /// <summary>
        /// Returns a pinholder object containing all the gps pins for a given lot_id. Makes an http post
        /// request to the user with json contanining the lot_id passed into this function.
        /// </summary>
        /// <param name="lot_id">lot id for the lot that pins are wanted from</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>A pinholder object</returns>
        public async Task<PinHolder> GetLotPinsAsync(int in_lot_id, string serviceUri = "/lotpins") 
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
        /// Uses the id of the currently logged in user to determine university id and then uses that to return
        /// a lotholder object filled with the lots associated with the user's university. If the university
        /// id is wrong or the university has no lots, the lotholder object will be empty.
        /// </summary>
        /// <param name="serviceUri"></param>
        /// <returns>lotholder object with an array of parking lot objects</returns>
        public async Task<LotHolder> GetMyUniLots(string serviceUri = "/getmyunilots")
        {
            LotHolder lh = new LotHolder();

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformGET(uri);

            if (response.IsSuccessStatusCode)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                lh = JsonConvert.DeserializeObject<LotHolder>(rescontent);
            }

            return lh;
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
        public async Task<bool> PostNewPinAsync(double in_longitude, double in_latitude, int in_lot_id, double in_volume, string serviceUri = "/newpin")
        {
            string json = JsonConvert.SerializeObject(new
            {
                longitude = in_longitude,
                latitude = in_latitude,
                lot_id = in_lot_id,
                volume = in_volume
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
        public async Task<ParkingLot> FindLot(double in_latitude, double in_longtitude, double in_accuracy, string serviceUri = "/findlot")
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

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                pl = JsonConvert.DeserializeObject<ParkingLot>(rescontent);
            }

            return pl;
        }

        /// THE FOLLOWING REQUEST WAS MADE BEFORE THE LOT DATA TABLE'S PURPOSE WAS DECIDED, THIS REQUEST
        /// SHOULD NOT BE USED AS IT IS LIKELY PERFORMING STRANGE MEANINGLESS TASKS AT THE MOMENT!
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

            if (response.StatusCode == HttpStatusCode.OK)
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

            if (response.StatusCode == HttpStatusCode.OK)
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

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                uni = JsonConvert.DeserializeObject<University>(rescontent);
            }

            return uni;
        }

        /// <summary>
        /// Peaks at the currently logged in user's survey status through an HTTP get
        /// </summary>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns>a bool indicating if they have taken the survey, 1 = yes</returns>
        public async Task<bool> SeeSurveyStatus(string serviceUri = "/surveystatus")
        {
            bool b = false;
            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformGET(uri);

            if (response.IsSuccessStatusCode)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                if (rescontent == "1")
                    b = true;
            }

            return b;
        }

        /// <summary>
        /// Sets the currently logged in user's survey status to true, indicating that the
        /// survey has been completed
        /// </summary>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>
        /// <returns></returns>
        public async Task SetSurveyStatus(string serviceUri = "/surveyset")
        {
            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformGET(uri);
        }

        /// <summary>
        /// Uses a pin id and an int to cast a vote on the pin with said id. The vote int
        /// can be positive or negative, with the sign indicating it is an upvote or a downvote.
        /// Performs an HTTP post to place the votes on the pin within the database.
        /// </summary>
        /// <param name="in_pin_id"></param>
        /// <param name="in_vote"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        public async Task<bool> VoteOnPin(int in_pin_id, int in_vote, string serviceUri = "/vote")
        {
            string json = JsonConvert.SerializeObject(new
            {
                pin_id = in_pin_id,
                vote = in_vote
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes a list of filled survey data objects in a http post request and translates them 
        /// into rows for the survey data table. Currently does not respond with the success of the
        /// post or handle any http status codes for errors
        /// </summary>
        /// <param name="surveyList">a list of filled survey data objects, representing the user's answers</param>
        /// <param name="serviceUri">uri fragment indicating a particular service</param>>
        /// <returns></returns>
        public async Task PostSurveyResults(List<SurveyData> surveyList, string serviceUri = "/surveyresults")
        {
            string json = JsonConvert.SerializeObject(surveyList);

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);
        }

        //UNTESTED
        public async Task<string> GetUsernameById(int in_id, string serviceUri = "/getuserbyid")
        {
            string name = null;

            string json = JsonConvert.SerializeObject(new
            {
                id = in_id
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                name = JsonConvert.DeserializeObject<string>(rescontent);
            }

            return name; 
        }

        //UNTESTED
        public async Task<ParkingPin> GetPinById(int in_id, string serviceUri = "/getuserbyid")
        {
            ParkingPin pin = null;

            string json = JsonConvert.SerializeObject(new
            {
                id = in_id
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                pin = JsonConvert.DeserializeObject<ParkingPin>(rescontent);
            }

            return pin;
        }

        public async Task<Dictionary<int, Polygon>> GetLotPolygons(string serviceUri = "/getpolylots")
        {
            Dictionary<int, Polygon> dictionary = null;

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformGET(uri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var rescontent = await response.Content.ReadAsStringAsync();
                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Polygon>>(rescontent);
            }

            return dictionary;
        }

        public async Task SendFCMToken(string in_token, string serviceUri = "/fcmtoken")
        {
            string json = JsonConvert.SerializeObject(new
            {
                token = in_token
            });

            Uri uri = makeUri(serviceUri);
            HttpResponseMessage response = await PerformPOST(uri, json);
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
    }
}
