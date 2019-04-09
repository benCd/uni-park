using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using up_mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    
    /// <summary>
    /// User page - Tabbed page containing the pages for logged in users
    /// </summary>
	public partial class User : TabbedPage
	{
        /// <summary>
        /// Loads the User page <see cref="User.xaml"/>
        /// </summary>
		public User ()
		{
			InitializeComponent ();

            // Hides the navigation bar at the top of the page
            NavigationPage.SetHasNavigationBar(this, false);

            // Suppresses the back button at the top of the page
            NavigationPage.SetHasBackButton(this, false);

            //Setting necessary data
            Setup();
        }

        /// <summary>
        /// Override for OnBackButtonPressed - Preventing Android Hardware Back Button from 
        /// going back to Login page <see cref="Login.xaml"/> after logging in
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        /// <summary>
        /// Sets up the properties so we can access necessary and continuously used data.
        /// </summary>
        async void Setup()
        {
            if (Application.Current.Properties.ContainsKey("FCMToken"))
                RestService.service.SendFCMToken((string)Application.Current.Properties["FCMToken"]);
        }

    }
}