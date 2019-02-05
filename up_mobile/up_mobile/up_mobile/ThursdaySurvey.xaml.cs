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
	public partial class ThursdaySurvey : ContentPage
    {
        // Constructor for ThursdaySurvey class - Initializes the page on load
        public ThursdaySurvey ()
		{
			InitializeComponent ();
		}

        // FridaySurveyPage function - takes the user to the Friday page of the New User Survey
        async void FridaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new FridaySurvey());
        }
    }
}