using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    
    /// <summary>
    /// User page
    /// </summary>
	public partial class User : TabbedPage
	{
        /// <summary>
        /// Loads the Guest page <see cref="Guest.xaml"/>
        /// </summary>
		public User ()
		{
			InitializeComponent ();

            // Suppresses the back button at the top of the page
            NavigationPage.SetHasBackButton(this, false);
        }
	}
}