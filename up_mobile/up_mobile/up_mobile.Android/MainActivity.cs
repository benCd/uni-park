using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;

namespace up_mobile.Droid
{
    [Activity(Label = "up_mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            MessagingCenter.Subscribe<BackgroundTasks.Messages.ExecuteScheduleMessage>(this, "ExecuteScheduleMessage", message => {
                var intent = new Intent(this, typeof(ExecuteScheduleService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<BackgroundTasks.Messages.CancelExecuteScheduleMessage>(this, "CancelExecuteScheduleMessage", message => {
                var intent = new Intent(this, typeof(ExecuteScheduleService));
                StopService(intent);
            });

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}