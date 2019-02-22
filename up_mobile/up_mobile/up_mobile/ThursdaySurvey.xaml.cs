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
    /// ThursdaySurvey page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ThursdaySurvey : ContentPage
    {
        /// <summary>
        /// Loads ThursdaySurvey page
        /// </summary>
        public ThursdaySurvey ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on ThursdaySurvey page <see cref="ThursdaySurvey.xaml"/> is pressed it 
        /// will navigate to the FridaySurvey page <see cref="FridaySurvey.xaml"/>
        /// </summary>
        async void FridaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new FridaySurvey());
        }
    }
}