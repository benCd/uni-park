using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    /// <summary>
    /// Account page
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
        /// When the Log Out button in <see cref="Account.xaml"/> is pressed, logs the user out and navigates them to 
        /// the Welcome page <see cref="Welcome.xaml"/> which is housed within the tabbed page <see cref="Guest.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LogOutAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            /// <remarks>
            /// Updating logged in status in <see cref="Settings.cs"/>
            /// </remarks>
            Helpers.Settings.IsLoggedIn = false;

            await Navigation.PushAsync(new Guest());
        }

    }
}