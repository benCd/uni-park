using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Xamarin.Forms;
using Android.Content;

namespace up_mobile.Droid
{
    [Activity(Label = "up_mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

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
            // the following call to the cross current activity plugin is required by the GPS plugin
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        // GPS plugin requires the following call to the permissions plugin to run on Android
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}