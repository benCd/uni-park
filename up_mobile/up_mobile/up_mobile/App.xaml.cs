using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Background;
using up_mobile.Backend;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace up_mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            //Change execution scheduler specific property upon cancellation
            MessagingCenter.Subscribe<Background.Messages.CancelExecuteScheduleMessage>(this, "CancelExecuteScheduleMessage", async message =>
            {
                App.Current.Properties["execution_schedule_running"] = false;
            });

            /// <summary>
            /// Decides which page to load the app on using the value of IsLoggedIn in <see cref="Settings.cs"/> 
            /// <see cref="User.xaml"/> is for logged in users
            /// <see cref="Guest.xaml"/> is for non logged in users
            /// </summary>
            if (Helpers.Settings.IsLoggedIn == true)
            {
                MainPage = new NavigationPage(new User());
            }
            
            else if (Helpers.Settings.IsLoggedIn == false)
            {
                MainPage = new NavigationPage(new Guest());
            }
        }

        protected override void OnStart()
        {
            Load();
        }

        protected override void OnSleep()
        {
            // Handling application sleep properties

            //Sleep properties
            //Save university lots
            if (App.Current.Properties.ContainsKey("UniversityLots"))
                App.Current.Properties["UniversityLots"] = MapContentPage.lotholder;
            
            //Save current cookie
            if (App.Current.Properties.ContainsKey("Cookies"))
                App.Current.Properties["Cookies"] = RestService.service.cookies;

            App.Current.SavePropertiesAsync();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Load();
        }

        /// <summary>
        /// This method is used to load the appplication data on 
        /// both starting and resuming of the app. Usage should be 
        /// limited to the <c>OnResume</c> and <c>OnStart</c> methods.
        /// The following properties are loaded:
        /// <code>
        /// "user_id" // ID for the user
        /// "last_online" // The last time the user was online (mmddyyy)
        /// "last_opened" // The last time the app was opened (mmddyyyy)
        /// /// </code>
        /// </summary>
        private void Load()
        {
            //If the task scheduler is not running an execution schedule start it
            if(!App.Current.Properties.ContainsKey("execution_schedule_running") || !(bool)App.Current.Properties["execution_schedule_running"])
            {
                var message = new Messages.ExecuteScheduleMessage();
                MessagingCenter.Send<Messages.ExecuteScheduleMessage>(message, "ExecuteScheduleMessage");
            }

            if (App.Current.Properties.ContainsKey("Cookies"))
                RestService.service.cookies = App.Current.Properties["Cookies"] as System.Net.CookieContainer;

            //TaskScheduler.ExecuteSchedule();

            //MapContentPage.InitMap();

            if (!App.Current.Properties.ContainsKey("DenyPressed"))
                App.Current.Properties.Add("DenyPressed", 0);

            //if (App.Current.Properties.ContainsKey("UniversityLots"))
            //MapContentPage.lotholder = (Models.LotHolder)App.Current.Properties["UniversityLots"];

        }
    }
}

