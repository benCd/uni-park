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
    /// ParkedConfirm page - Where the user confirms that they parked after 
    /// pressing I Parked button on <see cref="ParkedButton.xaml"/> page
    /// and the app uses GPS to detect which lot they are in
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParkedConfirm : ContentPage
	{
        /// <summary>
        /// Loads the ParkedConfirm page <see cref="ParkedConfirm.xaml"/>
        /// </summary>
		public ParkedConfirm ()
		{
			InitializeComponent ();
		}

	}
}