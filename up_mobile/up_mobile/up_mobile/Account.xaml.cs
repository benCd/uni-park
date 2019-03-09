using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    /// <summary>
    /// Account page - One of the pages a logged in user (User) can see
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Account : ContentPage
	{
        /// <summary>
        /// Loads the Account page <see cref="Account.xaml"/>
        /// </summary>
		public Account ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Log Out button in <see cref="Account.xaml"/> is pressed
        /// logs the user out and navigates them to the Welcome page 
        /// <see cref="Welcome.xaml"/> which is housed within the 
        /// Guest set of tabbed pages <see cref="Guest.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LogOutAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            /// <remarks>
            /// Updating logged in status in <see cref="Settings.cs"/>
            /// </remarks>
            RestService.service.LogoutUser();

            Helpers.Settings.IsLoggedIn = false;

            //Clearing properites
            Application.Current.Properties.Clear();


            await Navigation.PushAsync(new Guest());
        }

    }
}