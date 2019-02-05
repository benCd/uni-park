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
	public partial class FridaySurvey : ContentPage
    {
        // Constructor for FridaySurvey class - Initializes the page on load
        public FridaySurvey ()
		{
			InitializeComponent ();
		}

        // SaturdaySurveyPage function - takes the user to the Saturday page of the New User Survey
        async void SaturdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new SaturdaySurvey());
        }
    }
}