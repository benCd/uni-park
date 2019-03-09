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

            // Suppresses the back button at the top of the page
            NavigationPage.SetHasBackButton(this, false);

            //Setting necessary data
            Setup();
        }

        /// <summary>
        /// Override for OnBackButtonPressed - Preventing Android Hardware Back Button from 
        /// going back to Guest page <see cref="Guest.xaml"/> after logging in
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
            //Retrieving Uni data
            Application.Current.Properties.Add("UniversityId", await RestService.service.GetMyUni());

            //Retrieving Lot data for uni
            //Application.Current.Properties.Add("UniversityLots", await RestService.service.);
        }

    }
}