using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Background;

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
                Application.Current.Properties["execution_schedule_running"] = false;
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

            //TODO sleep properties
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
            if(!Application.Current.Properties.ContainsKey("execution_schedule_running") || !(bool)Application.Current.Properties["execution_schedule_running"])
            {
                var message = new Messages.ExecuteScheduleMessage();
                MessagingCenter.Send<Messages.ExecuteScheduleMessage>(message, "ExecuteScheduleMessage");
            }

            // If user has not been online in one week clear the user_id
            if(Application.Current.Properties.ContainsKey("last_online") && Application.Current.Properties.ContainsKey("last_opened"))
                if (DateTime.Today.Subtract((DateTime)Application.Current.Properties["last_online"]).TotalDays > 7 ||
                    DateTime.Today.Subtract((DateTime)Application.Current.Properties["last_opened"]).TotalDays > 21)
                {
                    Application.Current.Properties["user_id"] = -1;
                }

            if (Application.Current.Properties.ContainsKey("user_id") && (int)Application.Current.Properties["user_id"] < 0)
            {
                //PROMPT TO LOGINZ
            }

            //TaskScheduler.ExecuteSchedule();
        }
    }
}

