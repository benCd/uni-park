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
	public partial class WednesdaySurvey : ContentPage
	{
        // Constructor for WednesdaySurvey class - Initializes the page on load
        public WednesdaySurvey ()
		{
			InitializeComponent ();
		}

        // ThursdaySurveyPage function - takes the user to the Thursday page of the New User Survey
        async void ThursdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new ThursdaySurvey());
        }
    }
}