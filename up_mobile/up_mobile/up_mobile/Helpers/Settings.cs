﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace up_mobile.Helpers
{
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
        /// Storage for Email string value input by user when logging in on <see cref="Login.xaml"/> page
        /// </summary>
        public static string Email
        {
            get
            {
                return AppSettings.GetValueOrDefault("Email", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Email", value);
            }
        }

        /// <summary>
        /// Storage for Password string value input by user when logging in on <see cref="Login.xaml"/> page
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
        /// Storage for IsLoggedIn boolean value to determine if the user is still logged in or should be sent back to <see cref="Guest.xaml"/>
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

    }
}
