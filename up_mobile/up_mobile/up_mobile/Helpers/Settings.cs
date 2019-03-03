using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace up_mobile.Helpers
{
    /// <summary>
    /// Settings class for storage of certain information within the app
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        /// <summary>
        /// Storage for Username string value input by user when logging 
        /// in on <see cref="Login.xaml"/> page
        /// </summary>
        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault("Username", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Username", value);
            }
        }

        /// <summary>
        /// Storage for Password string value input by user when logging 
        /// in on <see cref="Login.xaml"/> page
        /// </summary>
        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault("Password", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Password", value);
            }
        }

        /// <summary>
        /// Storage for IsLoggedIn boolean value to determine if the user is 
        /// still logged in or should be sent back to <see cref="Guest.xaml"/>
        /// </summary>
        public static bool IsLoggedIn
        {
            get
            {
                return AppSettings.GetValueOrDefault("IsLoggedIn", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("IsLoggedIn", value);
            }
        }

        /// <summary>
        /// Storage for TookNewUserSurvey boolean value to determine if the 
        /// user has taken the New User Survey <see cref="NewUserSurvey.xaml"/> yet
        /// </summary>
        public static bool TookNewUserSurvey
        {
            get
            {
                return AppSettings.GetValueOrDefault("TookNewUserSurvey", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("TookNewUserSurvey", value);
            }
        }

    }
}
