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
    /// Welcome page - The first page that the non logged in user (Guest) will see
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Welcome : ContentPage
	{
        /// <summary>
        /// Loads the Welcome page <see cref="Welcome.xaml"/>
        /// </summary>
		public Welcome ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Log In button in <see cref="Welcome.xaml"/> is pressed
        /// navigates to the Log In page <see cref="Login.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LoginPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Login());
        }

        /// <summary>
        /// When the Register button in <see cref="Welcome.xaml"/> is pressed
        /// navigates to the Register page <see cref="Register.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void RegisterPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Register());
        }

    }
}