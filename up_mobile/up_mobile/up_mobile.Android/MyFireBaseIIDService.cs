using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using up_mobile.Backend;
using Xamarin.Forms;

namespace FCMClient
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("FCMToken"))
                Xamarin.Forms.Application.Current.Properties.Add("FCMToken", refreshedToken);
            else
                Xamarin.Forms.Application.Current.Properties["FCMToken"] = refreshedToken;

            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            //SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token)
        {
            RestService.service.SendFCMToken(token);
        }
    }
}