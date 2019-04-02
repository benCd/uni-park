using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Android.Content;
//firebase
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using up_mobile.Backend;

namespace up_mobile.Droid
{
    [Activity(Label = "up_mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //firebase business
        static readonly string TAG = "FCM";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
                Log.Debug(TAG, "Google Play Services is available.");
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Init for Rg.Plugins.Popup
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            //Init for Xamarin.Forms.Maps 
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            //Init for Xamarin.Essentials
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            //Subscribing to the necessary messages for scheduling background tasks
            MessagingCenter.Subscribe<Background.Messages.ExecuteScheduleMessage>(this, "ExecuteScheduleMessage", message => {
                var intent = new Intent(this, typeof(ExecuteScheduleService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<Background.Messages.CancelExecuteScheduleMessage>(this, "CancelExecuteScheduleMessage", message => {
                var intent = new Intent(this, typeof(ExecuteScheduleService));
                StopService(intent);
            });

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //firebase ////
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }

            IsPlayServicesAvailable();

            CreateNotificationChannel();

            //subscribe to topic, DEBUG PURPOSES ONLY
            //FirebaseMessaging.Instance.SubscribeToTopic("news");
            //LOG INSTANCE ID, DEBUG PURPOSES ONLY
            //Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            //end of firebase ///

            // the following call to the cross current activity plugin is required by the GPS plugin
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //LoadApplication(new App());

            LoadApplication(new App());

            if (Intent.Extras != null && Helpers.Settings.IsLoggedIn == true)
            {
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new User());
            }
        }

        // GPS plugin requires the following call to the permissions plugin to run on Android
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}