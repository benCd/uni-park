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
    /// Home page for logged in users
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
	}
}