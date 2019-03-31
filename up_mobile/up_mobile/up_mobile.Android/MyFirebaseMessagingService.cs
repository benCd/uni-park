using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;

using Firebase.Messaging;
using up_mobile.Backend;
using up_mobile.Droid;

namespace FCMNotifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);

            var body = message.Data.Keys.ToString();
            Log.Debug(TAG, "Notification Message Body: " + body);
            SendNotification(body, message.Data);
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.GRANDMAS_STATIONWAGONlarge)
                                      .SetContentTitle("Uni-Park")
                                      .SetContentText(messageBody)
                                      .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NOTIFICATION_ID, notificationBuilder.Build());

            RestService.service.SendFCMToken("USER GOT A NOTEY");
        }


    }
}