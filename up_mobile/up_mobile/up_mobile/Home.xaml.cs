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
    /// Home page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Home : ContentPage
	{
        /// <summary>
        /// Loads the Home page <see cref="Home.xaml"/>
        /// </summary>
		public Home ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Log In button in <see cref="Home.xaml"/> is pressed, navigates to the Log In page <see cref="Login.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LoginPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new Login());
        }

        /// <summary>
        /// When the Register button in <see cref="Home.xaml"/> is pressed, navigates to the Register page <see cref="Register.xaml"/>
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