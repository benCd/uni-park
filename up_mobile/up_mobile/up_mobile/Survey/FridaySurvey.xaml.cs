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
    /// FridaySurvey page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FridaySurvey : ContentPage
    {
        /// <summary>
        /// Loads FridaySurvey page
        /// </summary>
        public FridaySurvey ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on FridaySurvey page 
        /// <see cref="FridaySurvey.xaml"/> is pressed it navigates
        /// to the SaturdaySurvey page <see cref="SaturdaySurvey.xaml"/>
        /// </summary>
        async void SaturdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new SaturdaySurvey());
        }
    }
}