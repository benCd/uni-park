using System;
using System.Collections.Generic;
using System.Linq;
using BackgroundTasks;
using Xamarin.Forms;

using Foundation;
using UIKit;

namespace up_mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            iOSExecuteSchedule executeSchedulerTask = new iOSExecuteSchedule();

            MessagingCenter.Subscribe<BackgroundTasks.Messages.ExecuteScheduleMessage>(this, "ExecuteScheduleMessage", async message =>
            {
               executeSchedulerTask = new iOSExecuteSchedule();
               executeSchedulerTask.Start();
            });

            MessagingCenter.Subscribe<BackgroundTasks.Messages.CancelExecuteScheduleMessage>(this, "CancelExecuteScheduleMessage", async message =>
            {
                executeSchedulerTask.Stop();
            });

            return base.FinishedLaunching(app, options);
        }
    }
}
